namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class HorizSpaceConcatenate : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.HorizSpaceConcatenate;
            }
        }
    }
}

