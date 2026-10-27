# Lab09 - LINQ to SQL và ReportViewer

## Chuẩn bị lần đầu
1. Trong Visual Studio, mở SQL Server Object Explorer, kết nối `(localdb)\MSSQLLocalDB`.
2. Mở `Database/QLBH.sql` trong cửa sổ SQL Query rồi Execute. Script tạo database riêng `QLBanHang_Lab09` và 3 mặt hàng mẫu, không đụng database học sinh Lab08.
3. Mở `CSLab09_LINQ_Report.slnx`. Nếu Visual Studio chưa hỗ trợ slnx, mở `QLBanHang/QLBanHang.csproj`.
4. Cho phép Visual Studio khôi phục NuGet. Dự án dùng gói `Microsoft.ReportingServices.ReportViewerControl.Winforms` 150.1652.0.
5. Build, chọn QLBanHang làm startup project và nhấn F5.

Yêu cầu: Visual Studio có workload .NET desktop development, .NET Framework 4.8 targeting pack, SQL Server LocalDB và Internet cho lần đầu khôi phục NuGet. Gói ReportViewer là thư viện chạy; không cần extension RDLC để chạy. Muốn chỉnh mẫu báo cáo bằng kéo thả thì cài extension Microsoft RDLC Report Designer tương ứng Visual Studio.

## Kết nối
App.config đã dùng server của bạn: `(localdb)\MSSQLLocalDB`.
Giữ `name="QLBH"`; database là `Initial Catalog=QLBanHang_Lab09`.
Nếu đổi server, chỉ sửa Data Source. Không dùng QuanLyHocSinh cho bài bán hàng.

## Sử dụng
- Nhập khách hàng, chọn hàng, số lượng từ 1, bấm Thêm / cập nhật.
- Thêm lại cùng hàng sẽ THAY số lượng bằng giá trị đang nhập, theo ví dụ trong đề.
- Chọn dòng để đổi số lượng hoặc Xóa dòng.
- Bấm Lập hóa đơn & xem in: lưu đầu hóa đơn và chi tiết trong một giao dịch, lấy mã tự tăng, mở ReportViewer.
- Trên ReportViewer dùng biểu tượng máy in hoặc Export để xuất PDF/Excel/Word.
- Dùng Mã hóa đơn và Xem / in lại để mở hóa đơn đã lưu.
- Khi lỗi mở báo cáo sau khi lưu, không cần lập lại hóa đơn; dùng mã đã báo để xem lại.

## Mã nguồn
- Models.cs: QLBHDataContext, các bảng LINQ to SQL và HangChonMua.
- frmBanHang.cs / .Designer.cs: giao diện, giỏ hàng và giao dịch lưu dữ liệu.
- frmReport.cs: nguồn dsHangChonMua và tham số txtHoTenKH, MaHD, NgayLapHD.
- Reports/HoaDon.rdlc: mẫu hóa đơn, tên khách, ngày lập, bảng hàng, tổng tiền, số trang, tiêu đề lặp khi qua trang.
- Database/QLBH.sql: cấu trúc và dữ liệu mẫu theo đề. SQL float ánh xạ C# double; không nhầm với SQL real của Lab08.

## Lưu ý về cấu trúc đề
Chi tiết hóa đơn chỉ có MaHD, MaHH, SoLuong, không có đơn giá lịch sử. Vì vậy giữ nguyên giá danh mục nếu muốn in lại chính xác hóa đơn cũ. Chương trình kiểm tra tổng tiền trước khi in lại và báo nếu giá đã thay đổi. Không có chức năng sửa giá trong giao diện.

## Kiểm tra trong môi trường tạo dự án
Đã kiểm tra cấu trúc XML của RDLC và mã nguồn. Việc khôi phục thư viện ReportViewer gặp lỗi SSL NuGet, nên chưa xác nhận build đầy đủ, kết nối SQL Server hoặc hiển thị/in báo cáo. Máy chạy cần khôi phục NuGet thành công trước khi build. Không thay thư viện ReportViewer bằng báo cáo giả.
