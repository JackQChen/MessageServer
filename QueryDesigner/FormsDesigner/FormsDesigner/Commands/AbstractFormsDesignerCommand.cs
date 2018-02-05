namespace FormsDesigner.Commands
{
    using FormsDesigner;
    using FormsDesigner.Core;
    using FormsDesigner.Gui;
    using System;
    using System.ComponentModel.Design;

    public abstract class AbstractFormsDesignerCommand : AbstractMenuCommand
    {
        protected AbstractFormsDesignerCommand()
        {
        }

        protected virtual bool CanExecuteCommand(IDesignerHost host)
        {
            return true;
        }

        internal virtual void CommandCallBack(object sender, EventArgs e)
        {
            this.Run();
        }

        public override void Run()
        {
            try
            {
                FormsDesignerViewContent formDesigner = this.FormDesigner;
                if ((formDesigner != null) && this.CanExecuteCommand(formDesigner.Host))
                {
                    ((IMenuCommandService) formDesigner.Host.GetService(typeof(IMenuCommandService))).GlobalInvoke(this.CommandID);
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
        }

        public abstract System.ComponentModel.Design.CommandID CommandID { get; }

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
    }
}

