using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
            string processId = args[0];
            Task.Factory.StartNew(arg =>
            {
                Process serverProc = null;
                try
                {
                    serverProc = Process.GetProcessById(Convert.ToInt32(arg));
                }
                catch
                {
                    Environment.Exit(0);
                }
                while (true)
                {
                    if (serverProc.HasExited)
                        Environment.Exit(0);
                    Thread.Sleep(10000);
                }
            }, processId, TaskCreationOptions.LongRunning);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain(processId));
        }
    }
}
