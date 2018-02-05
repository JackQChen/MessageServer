namespace FormsDesigner.Services
{
    using FormsDesigner.Core;
    using FormsDesigner.Gui;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class UIService : IUIService
    {
        private IDictionary styles = new Hashtable();

        public UIService()
        {
            this.styles["DialogFont"] = Control.DefaultFont;
            this.styles["HighlightColor"] = Color.LightYellow;
        }

        public bool CanShowComponentEditor(object component)
        {
            return false;
        }

        public IWin32Window GetDialogOwnerWindow()
        {
            return WorkbenchSingleton.MainForm;
        }

        public void SetUIDirty()
        {
        }

        public bool ShowComponentEditor(object component, IWin32Window parent)
        {
            throw new NotImplementedException("Cannot display component editor for " + component);
        }

        public DialogResult ShowDialog(Form form)
        {
            return form.ShowDialog(this.GetDialogOwnerWindow());
        }

        public void ShowError(Exception ex)
        {
            MessageService.ShowError(ex.ToString());
        }

        public void ShowError(string message)
        {
            MessageService.ShowError(message);
        }

        public void ShowError(Exception ex, string message)
        {
            MessageService.ShowError(message + Environment.NewLine + ex.ToString());
        }

        public void ShowMessage(string message)
        {
            this.ShowMessage(message, "", MessageBoxButtons.OK);
        }

        public void ShowMessage(string message, string caption)
        {
            this.ShowMessage(message, caption, MessageBoxButtons.OK);
        }

        public DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this.GetDialogOwnerWindow(), message, caption, buttons);
        }

        public bool ShowToolWindow(Guid toolWindow)
        {
            return false;
        }

        public IDictionary Styles
        {
            get
            {
                return this.styles;
            }
        }
    }
}

