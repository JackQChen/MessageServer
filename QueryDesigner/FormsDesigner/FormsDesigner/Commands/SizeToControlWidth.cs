namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class SizeToControlWidth : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.SizeToControlWidth;
            }
        }
    }
}

