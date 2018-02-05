using System;
using System.Windows.Forms;

namespace QueryDesigner
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。

        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                string filePath = "";
                for (int i = 0; i < args.Length; i++)
                {
                    filePath += " " + args[i];
                }

                Application.Run(new FormMain(filePath));
            }
            else
            {
                Application.Run(new FormMain(string.Empty));
            }

        }
    }
}
