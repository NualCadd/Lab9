IF DB_ID(N'QLBanHang_Lab09') IS NULL CREATE DATABASE QLBanHang_Lab09;
GO
USE QLBanHang_Lab09;
GO
IF OBJECT_ID(N'dbo.HangHoa', N'U') IS NULL
CREATE TABLE dbo.HangHoa (
 MaHH int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 TenHH nvarchar(50) NOT NULL,
 DonViTinh nvarchar(50) NULL,
 DonGia float NOT NULL CHECK (DonGia >= 0)
);
IF OBJECT_ID(N'dbo.HoaDon', N'U') IS NULL
CREATE TABLE dbo.HoaDon (
 MaHD int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 NgayLapHD datetime NULL DEFAULT GETDATE(),
 KhachHang nvarchar(50) NULL,
 TongTien float NULL
);
IF OBJECT_ID(N'dbo.CTHoaDon', N'U') IS NULL
CREATE TABLE dbo.CTHoaDon (
 MaHD int NOT NULL REFERENCES dbo.HoaDon(MaHD),
 MaHH int NOT NULL REFERENCES dbo.HangHoa(MaHH),
 SoLuong int NULL CHECK (SoLuong > 0),
 CONSTRAINT PK_CTHoaDon PRIMARY KEY (MaHD, MaHH)
);
GO
IF NOT EXISTS (SELECT 1 FROM dbo.HangHoa WHERE TenHH=N'Bút bi')
INSERT dbo.HangHoa (TenHH,DonViTinh,DonGia) VALUES (N'Bút bi',N'Cây',5000);
IF NOT EXISTS (SELECT 1 FROM dbo.HangHoa WHERE TenHH=N'Vở 100 trang')
INSERT dbo.HangHoa (TenHH,DonViTinh,DonGia) VALUES (N'Vở 100 trang',N'Quyển',12000);
IF NOT EXISTS (SELECT 1 FROM dbo.HangHoa WHERE TenHH=N'Thước kẻ')
INSERT dbo.HangHoa (TenHH,DonViTinh,DonGia) VALUES (N'Thước kẻ',N'Cây',8000);
GO
