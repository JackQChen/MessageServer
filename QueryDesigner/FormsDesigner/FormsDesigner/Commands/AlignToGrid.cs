namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignToGrid : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignToGrid;
            }
        }
    }
}

