using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SystemFramework.BaseControl
{
    public class MessageBoxEx
    {
        public static DialogResult Show(string text)
        {
            return Show(text, "提示信息");
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public static DialogResult Show(string text, string caption, MessageBoxIcon icon)
        {
            return Show(null, text, caption, MessageBoxButtons.OK, icon);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return XtraMessageBox.Show(owner, text, caption, buttons, icon);
        }
    }
}
