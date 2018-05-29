using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
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
            bool createdNew;
            Mutex instance = new Mutex(true, Convert.ToBase64String(Encoding.UTF8.GetBytes(AppDomain.CurrentDomain.BaseDirectory)), out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                UpdateCheck check = new UpdateCheck();
                var lastTime = check.GetUpdateTime();
                var needUpdate = Convert.ToDateTime(ConfigurationManager.AppSettings["UpdateTime"]) < lastTime;
                if (needUpdate)
                {
                    var frm = new FrmMain();
                    frm.dtLastUpdateTime = lastTime;
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
