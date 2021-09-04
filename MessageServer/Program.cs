using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MessageServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = false;
            Mutex instance = new Mutex(true, Application.ExecutablePath.Replace("\\", "/"), out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    var err = e.ExceptionObject.ToString();
                    MessageBox.Show(err, "未处理异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Exception.txt",
                        string.Format("{0}\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), err));
                };
                Application.Run(new FrmMain());
                instance.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("消息服务器正在运行中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
    }
}
