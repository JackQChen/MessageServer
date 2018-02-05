using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text;

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
            bool createdNew;
            Mutex instance = new Mutex(true, Convert.ToBase64String(Encoding.UTF8.GetBytes(AppDomain.CurrentDomain.BaseDirectory)), out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    var err = e.ExceptionObject.ToString();
                    MessageBox.Show(err, "未处理异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Exception.txt",
                        string.Format("{0}\r\n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), err));
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
