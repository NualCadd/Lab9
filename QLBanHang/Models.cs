using System;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
namespace QLBanHang
{
    public class QLBHDataContext : DataContext
    {
        public QLBHDataContext() : base(ConfigurationManager.ConnectionStrings["QLBH"].ConnectionString) { }
        public Table<HangHoa> HangHoas => GetTable<HangHoa>();
        public Table<HoaDon> HoaDons => GetTable<HoaDon>();
        public Table<CTHoaDon> CTHoaDons => GetTable<CTHoaDon>();
    }
    [Table(Name="dbo.HangHoa")]
    public class HangHoa
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)] public int MaHH { get; set; }
        [Column] public string TenHH { get; set; }
        [Column] public string DonViTinh { get; set; }
        [Column] public double DonGia { get; set; }
    }
    [Table(Name="dbo.HoaDon")]
    public class HoaDon
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)] public int MaHD { get; set; }
        [Column] public DateTime? NgayLapHD { get; set; }
        [Column] public string KhachHang { get; set; }
        [Column] public double? TongTien { get; set; }
    }
    [Table(Name="dbo.CTHoaDon")]
    public class CTHoaDon
    {
        [Column(IsPrimaryKey=true)] public int MaHD { get; set; }
        [Column(IsPrimaryKey=true)] public int MaHH { get; set; }
        [Column] public int? SoLuong { get; set; }
    }
    public class HangChonMua
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien => SoLuong * DonGia;
    }
}
