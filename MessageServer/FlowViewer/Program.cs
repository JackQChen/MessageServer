using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlowViewer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var frm = new FrmMain();
            frm.serverProc = Process.GetProcessById(Convert.ToInt32(args[0]));
            Application.Run(frm);
        }
    }
}
