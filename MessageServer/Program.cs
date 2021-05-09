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
