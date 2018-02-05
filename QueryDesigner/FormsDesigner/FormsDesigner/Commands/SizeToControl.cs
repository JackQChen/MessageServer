namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class SizeToControl : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.SizeToControl;
            }
        }
    }
}

