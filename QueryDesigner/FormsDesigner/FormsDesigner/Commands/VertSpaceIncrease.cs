namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class VertSpaceIncrease : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.VertSpaceIncrease;
            }
        }
    }
}

