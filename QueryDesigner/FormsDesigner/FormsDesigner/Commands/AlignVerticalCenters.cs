namespace FormsDesigner.Commands
{
    using System.ComponentModel.Design;

    public class AlignVerticalCenters : AbstractFormsDesignerCommand
    {
        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.AlignVerticalCenters;
            }
        }
    }
}

