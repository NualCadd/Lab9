using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
namespace QLBanHang
{
    public class frmReport : Form
    {
        private readonly ReportViewer reportViewer1;
        public frmReport(int maHD, string hoten, DateTime ngayLap, List<HangChonMua> dsmua)
        {
            Text="Hóa đơn #" + maHD + " — Xem trước / In / Xuất PDF";
            Width=1000; Height=760;
            StartPosition=FormStartPosition.CenterParent;
            reportViewer1=new ReportViewer { Dock=DockStyle.Fill, ProcessingMode=ProcessingMode.Local };
            Controls.Add(reportViewer1);
            reportViewer1.LocalReport.ReportEmbeddedResource="QLBanHang.Reports.HoaDon.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsHangChonMua",dsmua));
            reportViewer1.LocalReport.SetParameters(new[] {
                new ReportParameter("txtHoTenKH",hoten ?? ""),
                new ReportParameter("MaHD",maHD.ToString(CultureInfo.InvariantCulture)),
                new ReportParameter("NgayLapHD",ngayLap.ToString("dd/MM/yyyy HH:mm"))
            });
            Load += (s,e) => { reportViewer1.SetDisplayMode(DisplayMode.PrintLayout); reportViewer1.ZoomMode=ZoomMode.PageWidth; reportViewer1.RefreshReport(); };
        }
        protected override void Dispose(bool disposing) { if(disposing) reportViewer1?.Dispose(); base.Dispose(disposing); }
    }
}
