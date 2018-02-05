namespace FormsDesigner.Services
{
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class MenuCommandService : System.ComponentModel.Design.MenuCommandService
    {
        private Control panel;

        public MenuCommandService(Control panel, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.panel = panel;
            this.InitializeGlobalCommands();
        }

        private void InitializeGlobalCommands()
        {
        }

        public override void ShowContextMenu(CommandID menuID, int x, int y)
        {
            string addInTreePath = "/SharpDevelop/FormsDesigner/ContextMenus/";
            if (menuID == MenuCommands.ComponentTrayMenu)
            {
                addInTreePath = addInTreePath + "ComponentTrayMenu";
            }
            else if (menuID == MenuCommands.ContainerMenu)
            {
                addInTreePath = addInTreePath + "ContainerMenu";
            }
            else if (menuID == MenuCommands.SelectionMenu)
            {
                addInTreePath = addInTreePath + "SelectionMenu";
            }
            else
            {
                if (menuID != MenuCommands.TraySelectionMenu)
                {
                    throw new Exception();
                }
                addInTreePath = addInTreePath + "TraySelectionMenu";
            }
            Point point = this.panel.PointToClient(new Point(x, y));
            MenuService.ShowContextMenu(this, addInTreePath, this.panel, point.X, point.Y);
        }
    }
}

