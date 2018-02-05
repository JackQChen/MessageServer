namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class LockControls : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.LockControls;
            }
        }
    }
}

