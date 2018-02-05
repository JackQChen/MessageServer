namespace FormsDesigner.Commands
{
    using FormsDesigner;
    using FormsDesigner.Gui;
    using System;
    using System.ComponentModel.Design;

    public class ShowProperties : AbstractFormsDesignerCommand
    {
        public override void Run()
        {
            IPadContent pad = WorkbenchSingleton.Workbench.GetPad(typeof(PropertyPad));
        }

        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.PropertiesWindow;
            }
        }
    }
}

