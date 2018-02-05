namespace FormsDesigner.Core
{
    using System;
    using System.Windows.Forms;

    public static class MessageService
    {
        public static bool AskQuestion(string question)
        {
            return AskQuestion(question, "提示框");
        }

        public static bool AskQuestion(string question, string caption)
        {
            return (MessageBox.Show(question, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        public static bool AskQuestionFormatted(string formatstring, params string[] formatitems)
        {
            return false;
        }

        public static bool AskQuestionFormatted(string caption, string formatstring, params string[] formatitems)
        {
            return false;
        }

        public static int ShowCustomDialog(string caption, string dialogText, params string[] buttontexts)
        {
            return 0;
        }

        public static void ShowError(Exception ex)
        {
            ShowError(ex, null);
        }

        public static void ShowError(string message)
        {
            ShowError(null, message);
        }

        public static void ShowError(Exception ex, string message)
        {
            string text = string.Empty;
            if (message != null)
            {
                text = text + message;
            }
            if ((message != null) && (ex != null))
            {
                text = text + "\n\n";
            }
            if (ex != null)
            {
                text = text + "Exception occurred: " + ex.ToString();
            }
            MessageBox.Show(text, "提示框", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static void ShowErrorFormatted(string formatstring, params string[] formatitems)
        {
        }

        public static void ShowMessage(string message)
        {
            ShowMessage(message, "SharpDevelop");
        }

        public static void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowMessageFormatted(string formatstring, params string[] formatitems)
        {
        }

        public static void ShowMessageFormatted(string caption, string formatstring, params string[] formatitems)
        {
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "提示框", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void ShowWarningFormatted(string formatstring, params string[] formatitems)
        {
        }
    }
}

