namespace FormsDesigner.Commands
{
    using FormsDesigner;
    using FormsDesigner.Core;
    using FormsDesigner.Gui;
    using System;

    public class ViewTabOrder : AbstractCheckableMenuCommand
    {
        private void SetTabOrder(bool show)
        {
            FormsDesignerViewContent formDesigner = this.FormDesigner;
            if (formDesigner != null)
            {
                if (show)
                {
                    formDesigner.ShowTabOrder();
                }
                else
                {
                    formDesigner.HideTabOrder();
                }
            }
        }

        private FormsDesignerViewContent FormDesigner
        {
            get
            {
                IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                if (activeWorkbenchWindow == null)
                {
                    return null;
                }
                return (activeWorkbenchWindow.ActiveViewContent as FormsDesignerViewContent);
            }
        }

        public override bool IsChecked
        {
            get
            {
                FormsDesignerViewContent formDesigner = this.FormDesigner;
                return ((formDesigner != null) && formDesigner.IsTabOrderMode);
            }
            set
            {
                this.SetTabOrder(value);
            }
        }
    }
}

