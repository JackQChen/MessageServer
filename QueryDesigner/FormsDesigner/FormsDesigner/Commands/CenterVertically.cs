namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class CenterVertically : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.CenterVertically;
            }
        }
    }
}

