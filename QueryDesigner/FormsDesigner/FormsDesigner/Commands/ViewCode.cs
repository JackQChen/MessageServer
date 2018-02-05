namespace FormsDesigner.Commands
{
    using FormsDesigner;
    using FormsDesigner.Gui;
    using System;
    using System.ComponentModel.Design;

    public class ViewCode : AbstractFormsDesignerCommand
    {
        public override void Run()
        {
            if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null)
            {
                FormsDesignerViewContent formDesigner = this.FormDesigner;
                if (formDesigner != null)
                {
                    formDesigner.ShowSourceCode();
                }
            }
        }

        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.ViewCode;
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
    }
}

