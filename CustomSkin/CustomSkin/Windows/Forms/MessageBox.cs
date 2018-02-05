using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public class MessageBox
    {

        public static DialogResult Show(string text)
        {
            return Show(text, "提示信息");
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (FrmMessage frm = new FrmMessage(buttons))
            {
                frm.Content = text;
                frm.Text = caption; 
                return frm.ShowDialog(owner);
            }
        }
    }
}
