namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class SendToBack : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.SendToBack;
            }
        }
    }
}

