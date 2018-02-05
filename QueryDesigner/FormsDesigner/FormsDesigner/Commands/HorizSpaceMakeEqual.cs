namespace FormsDesigner.Commands
{
    using System;
    using System.ComponentModel.Design;

    public class HorizSpaceMakeEqual : AbstractFormsDesignerCommand
    {
        protected override bool CanExecuteCommand(IDesignerHost host)
        {
            ISelectionService service = (ISelectionService) host.GetService(typeof(ISelectionService));
            return (service.SelectionCount > 1);
        }

        public override System.ComponentModel.Design.CommandID CommandID
        {
            get
            {
                return StandardCommands.HorizSpaceMakeEqual;
            }
        }
    }
}

