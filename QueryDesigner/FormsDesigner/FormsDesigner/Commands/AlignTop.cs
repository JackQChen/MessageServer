namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignTop : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignTop;
            }
        }
    }
}

