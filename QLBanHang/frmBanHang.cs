using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBanHang
{
    public partial class frmBanHang : Form
    {
        private readonly List<HangChonMua> dsmua = new List<HangChonMua>();
        private List<HangHoa> danhMuc = new List<HangHoa>();
        public frmBanHang()
        {
            InitializeComponent();
            Load += (s,e) => Safe(LoadProducts);
            cboHangHoa.SelectedIndexChanged += (s,e) => ShowPrice();
            btnTaiLai.Click += (s,e) => Safe(LoadProducts);
            btnThem.Click += (s,e) => Safe(AddItem);
            btnXoa.Click += (s,e) => {
                if (dgvHangHoa.CurrentRow?.DataBoundItem is HangChonMua row) { dsmua.Remove(row); RefreshCart(); }
            };
            btnMoi.Click += (s,e) => {
                if (dsmua.Count > 0 && MessageBox.Show(this,"Xóa giỏ hàng hiện tại?","Đơn mới",MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                dsmua.Clear(); txtKhachHang.Clear(); RefreshCart();
            };
            btnLapHoaDon.Click += (s,e) => Safe(CreateInvoice);
            btnInLai.Click += (s,e) => Safe(ReprintInvoice);
            dgvHangHoa.SelectionChanged += (s,e) => {
                if (dgvHangHoa.CurrentRow?.DataBoundItem is HangChonMua row) {
                    cboHangHoa.SelectedValue = row.MaHH;
                    nudSoLuong.Value = row.SoLuong;
                }
            };
        }

        private void Safe(Action action)
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message + "\n\nNếu lỗi kết nối: chạy Database/QLBH.sql và kiểm tra App.config.","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
        }
        private void LoadProducts()
        {
            using (var db = new QLBHDataContext()) danhMuc = db.HangHoas.OrderBy(x=>x.TenHH).ToList();
            cboHangHoa.DisplayMember = "TenHH";
            cboHangHoa.ValueMember = "MaHH";
            cboHangHoa.DataSource = danhMuc;
            ShowPrice();
            lblTrangThai.Text = "Đã tải " + danhMuc.Count + " mặt hàng. Chọn hàng và nhập số lượng.";
        }
        private void ShowPrice()
        {
            var item = cboHangHoa.SelectedItem as HangHoa;
            lblDonGia.Text = item == null ? "Đơn giá: —" : "Đơn giá: " + item.DonGia.ToString("N0") + " đ / " + item.DonViTinh;
        }
        private void AddItem()
        {
            var product = cboHangHoa.SelectedItem as HangHoa;
            if (product == null) { MessageBox.Show(this,"Chưa có hàng hóa. Hãy tải danh mục."); return; }
            int quantity = decimal.ToInt32(nudSoLuong.Value);
            var existing = dsmua.SingleOrDefault(x=>x.MaHH == product.MaHH);
            if (existing == null) dsmua.Add(new HangChonMua { MaHH=product.MaHH, TenHH=product.TenHH, DonGia=product.DonGia, SoLuong=quantity });
            else existing.SoLuong = quantity; // Theo đề: thay số lượng cũ bằng số lượng vừa nhập.
            RefreshCart();
            lblTrangThai.Text = "Đã thêm/cập nhật " + product.TenHH + ". Chọn lại cùng hàng để thay số lượng.";
        }
        private void RefreshCart()
        {
            dgvHangHoa.DataSource = null;
            dgvHangHoa.DataSource = dsmua.ToList();
            lblTongTien.Text = "TỔNG CỘNG: " + dsmua.Sum(x=>x.ThanhTien).ToString("N0") + " đ";
        }
        private void CreateInvoice()
        {
            if (string.IsNullOrWhiteSpace(txtKhachHang.Text)) { MessageBox.Show(this,"Vui lòng nhập tên khách hàng."); txtKhachHang.Focus(); return; }
            if (dsmua.Count == 0) { MessageBox.Show(this,"Giỏ hàng đang trống."); return; }
            string customer = txtKhachHang.Text.Trim();
            DateTime date = DateTime.Now;
            int invoiceId;
            var snapshot = dsmua.Select(x=>new HangChonMua { MaHH=x.MaHH, TenHH=x.TenHH, SoLuong=x.SoLuong, DonGia=x.DonGia }).ToList();
            using (var db = new QLBHDataContext())
            {
                db.Connection.Open();
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Transaction = transaction;
                    try
                    {
                        var hd = new HoaDon { KhachHang=customer, NgayLapHD=date, TongTien=snapshot.Sum(x=>x.ThanhTien) };
                        db.HoaDons.InsertOnSubmit(hd);
                        db.SubmitChanges(); // Nhận mã hóa đơn IDENTITY trong cùng giao dịch.
                        foreach (var row in snapshot)
                            db.CTHoaDons.InsertOnSubmit(new CTHoaDon { MaHD=hd.MaHD, MaHH=row.MaHH, SoLuong=row.SoLuong });
                        db.SubmitChanges();
                        transaction.Commit();
                        invoiceId = hd.MaHD;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
            // Xóa giỏ chỉ sau khi đã lưu thành công. Report nhận bản sao riêng.
            dsmua.Clear(); txtKhachHang.Clear(); RefreshCart();
            nudMaHD.Value = invoiceId;
            lblTrangThai.Text = "Đã lưu hóa đơn #" + invoiceId + ". Có thể xem/in lại bằng mã hóa đơn.";
            OpenReport(invoiceId, customer, date, snapshot);
        }
        private void ReprintInvoice()
        {
            int id = decimal.ToInt32(nudMaHD.Value);
            using (var db = new QLBHDataContext())
            {
                var hd = db.HoaDons.SingleOrDefault(x=>x.MaHD == id);
                if (hd == null) { MessageBox.Show(this,"Không tìm thấy hóa đơn #" + id); return; }
                var rows = (from ct in db.CTHoaDons join hh in db.HangHoas on ct.MaHH equals hh.MaHH
                            where ct.MaHD == id
                            select new HangChonMua { MaHH=hh.MaHH, TenHH=hh.TenHH, DonGia=hh.DonGia, SoLuong=ct.SoLuong ?? 0 }).ToList();
                if (Math.Abs(rows.Sum(x=>x.ThanhTien) - (hd.TongTien ?? 0)) > 0.01)
                    throw new InvalidOperationException("Đơn giá danh mục đã thay đổi so với hóa đơn cũ. Theo cấu trúc đề, chi tiết không lưu đơn giá lịch sử nên không thể in lại chính xác hóa đơn này.");
                OpenReport(id, hd.KhachHang, hd.NgayLapHD ?? DateTime.Today, rows);
            }
        }
        private void OpenReport(int id, string customer, DateTime date, List<HangChonMua> rows)
        {
            try { using (var report = new frmReport(id,customer,date,rows)) report.ShowDialog(this); }
            catch (Exception ex) { MessageBox.Show(this,"Hóa đơn #" + id + " đã được lưu. Chưa mở được báo cáo: " + ex.Message + "\nBạn có thể dùng Xem / in lại sau."); }
        }
    }
}
