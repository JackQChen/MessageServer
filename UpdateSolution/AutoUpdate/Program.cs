using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            MessageBoxEx.time = args.Length == 0 ? 0 : 5000;
            bool createdNew = false;
            Mutex instance = new Mutex(true, Process.GetCurrentProcess().MainModule.FileName.Replace("\\", "/"), out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                UpdateCheck check = new UpdateCheck();
                var currentTime = Convert.ToDateTime(ConfigurationManager.AppSettings["UpdateTime"]);
                var lastTime = check.GetUpdateTime();
                var needUpdate = currentTime < lastTime;
                if (needUpdate)
                {
                    var frm = new FrmMain();
                    //无参启动不更新时间
                    frm.dtLastUpdateTime = args.Length == 0 ? currentTime : lastTime;
                    Application.Run(frm);
                }
                if (args.Length == 0)
                    MessageBoxEx.Show("自动更新完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + args[0], "AutoUpdate " + needUpdate.ToString());
            }
            else
            {
                MessageBoxEx.Show("自动更新正在运行中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
