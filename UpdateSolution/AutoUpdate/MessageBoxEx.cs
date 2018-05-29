using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoUpdate
{
    public class MessageBoxEx
    {

        public static int time = 0;

        [DllImport("user32.dll")]
        static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caption, int type, int DWORD, int time);

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (time == 0)
                return MessageBox.Show(text, caption, buttons, icon);
            else
                return (DialogResult)MessageBoxTimeoutA(IntPtr.Zero, text, caption, buttons.GetHashCode() | icon.GetHashCode() | MessageBoxDefaultButton.Button1.GetHashCode(), 0, time);
        }
    }
}
