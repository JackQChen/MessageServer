namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignRight : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignRight;
            }
        }
    }
}

