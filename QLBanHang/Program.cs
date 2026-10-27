using System;
using System.Windows.Forms;
namespace QLBanHang
{
    internal static class Program
    {
        [STAThread] static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBanHang());
        }
    }
}
