namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class HorizSpaceDecrease : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.HorizSpaceDecrease;
            }
        }
    }
}

