namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignHorizontalCenters : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignHorizontalCenters;
            }
        }
    }
}

