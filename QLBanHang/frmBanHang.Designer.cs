using System.Drawing;
using System.Windows.Forms;
namespace QLBanHang
{
    partial class frmBanHang
    {
        private ComboBox cboHangHoa;
        private NumericUpDown nudSoLuong, nudMaHD;
        private TextBox txtKhachHang;
        private DataGridView dgvHangHoa;
        private Label lblDonGia, lblTongTien, lblTrangThai;
        private Button btnThem, btnXoa, btnMoi, btnLapHoaDon, btnTaiLai, btnInLai;
        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "LAB 09 • Bán hàng — LINQ & Report";
            ClientSize = new Size(1000,710);
            MinimumSize = new Size(1016,749);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI",10F);
            BackColor = Color.FromArgb(245,247,251);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(new Label { Text="LẬP HÓA ĐƠN BÁN HÀNG", Location=new Point(24,18), AutoSize=true, Font=new Font("Segoe UI",20F,FontStyle.Bold), ForeColor=Color.FromArgb(25,55,105) });
            LabelAt("Khách hàng",24,85);
            txtKhachHang = new TextBox { Name="txtKhachHang", Location=new Point(145,80), Width=530, MaxLength=50 };
            Controls.Add(txtKhachHang);
            LabelAt("Hàng hóa",24,140);
            cboHangHoa = new ComboBox { Name="cboHangHoa", Location=new Point(145,135), Width=330, DropDownStyle=ComboBoxStyle.DropDownList };
            Controls.Add(cboHangHoa);
            LabelAt("Số lượng",500,140);
            nudSoLuong = new NumericUpDown { Name="nudSoLuong", Location=new Point(585,135), Width=90, Minimum=1, Maximum=100000, Increment=1, Value=1 };
            Controls.Add(nudSoLuong);
            btnThem = ButtonAt("Thêm / cập nhật",700,130,150);
            btnTaiLai = ButtonAt("Tải hàng",860,130,110);
            lblDonGia = new Label { Text="Đơn giá: —", Location=new Point(145,175), AutoSize=true };
            Controls.Add(lblDonGia);
            dgvHangHoa = new DataGridView { Name="dgvHangHoa", Location=new Point(24,220), Size=new Size(946,300), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom,
                AutoGenerateColumns=false, ReadOnly=true, AllowUserToAddRows=false, AllowUserToDeleteRows=false, MultiSelect=false, SelectionMode=DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible=false, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor=Color.White };
            string[] names={"MaHH","TenHH","SoLuong","DonGia","ThanhTien"};
            string[] captions={"Mã hàng","Tên hàng hóa","Số lượng","Đơn giá (đ)","Thành tiền (đ)"};
            for(int i=0;i<names.Length;i++) dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn { Name=names[i], DataPropertyName=names[i], HeaderText=captions[i], FillWeight=i==1?220:100 });
            dgvHangHoa.Columns["DonGia"].DefaultCellStyle.Format="N0";
            dgvHangHoa.Columns["ThanhTien"].DefaultCellStyle.Format="N0";
            dgvHangHoa.AlternatingRowsDefaultCellStyle.BackColor=Color.FromArgb(240,245,252);
            Controls.Add(dgvHangHoa);
            btnXoa=ButtonAt("Xóa dòng",24,535,110);
            btnMoi=ButtonAt("Đơn mới",146,535,110);
            btnLapHoaDon=ButtonAt("Lập hóa đơn & xem in",270,535,210);
            foreach(var b in new[]{btnXoa,btnMoi,btnLapHoaDon}) b.Anchor=AnchorStyles.Left|AnchorStyles.Bottom;
            lblTongTien=new Label { Text="TỔNG CỘNG: 0 đ", Location=new Point(550,540), Size=new Size(420,35), TextAlign=ContentAlignment.MiddleRight, Font=new Font("Segoe UI",15F,FontStyle.Bold), Anchor=AnchorStyles.Bottom|AnchorStyles.Right };
            Controls.Add(lblTongTien);
            var reprintLabel=LabelAt("Mã hóa đơn",24,604); reprintLabel.Anchor=AnchorStyles.Bottom|AnchorStyles.Left;
            nudMaHD=new NumericUpDown { Location=new Point(145,600), Width=110, Minimum=1, Maximum=int.MaxValue, Value=1, Anchor=AnchorStyles.Left|AnchorStyles.Bottom };
            Controls.Add(nudMaHD);
            btnInLai=ButtonAt("Xem / in lại",270,595,150); btnInLai.Anchor=AnchorStyles.Left|AnchorStyles.Bottom;
            lblTrangThai=new Label { Text="Chạy Database/QLBH.sql trước lần sử dụng đầu tiên.", Location=new Point(24,663), Size=new Size(946,30), Anchor=AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right };
            Controls.Add(lblTrangThai);
            ResumeLayout(false); PerformLayout();
        }
        private Label LabelAt(string text,int x,int y) { var c=new Label { Text=text,Location=new Point(x,y),AutoSize=true }; Controls.Add(c); return c; }
        private Button ButtonAt(string text,int x,int y,int width) { var b=new Button { Text=text,Location=new Point(x,y),Size=new Size(width,38),BackColor=Color.White,FlatStyle=FlatStyle.Flat }; Controls.Add(b); return b; }
    }
}
