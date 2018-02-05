namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignBottom : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignBottom;
            }
        }
    }
}

