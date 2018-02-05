namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignLeft : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignLeft;
            }
        }
    }
}

