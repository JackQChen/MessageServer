namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class LineUpIcons : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.LineupIcons;
            }
        }
    }
}

