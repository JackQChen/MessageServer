using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CustomSkin.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new F());
            Application.Run(new Form1());
        }
        class F : IMessageFilter
        {

            #region IMessageFilter 成员

            public bool PreFilterMessage(ref Message m)
            {
                return false;
            }

            #endregion
        }
    }
}
