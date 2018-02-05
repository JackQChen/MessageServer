namespace FormsDesigner.Services
{
    using FormsDesigner.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class MenuService
    {
        private static bool isContextMenuOpen;
        public static event EventHandler<ToolStripItemClickedEventArgs> OnMenuClick;

        public static void AddItemsToMenu(ToolStripItemCollection collection, object owner, string addInTreePath)
        {
        }

        private static void ContextMenuClosed(object sender, EventArgs e)
        {
            isContextMenuOpen = false;
        }

        private static void ContextMenuOpened(object sender, EventArgs e)
        {
            isContextMenuOpen = true;
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            foreach (object obj2 in strip.Items)
            {
                if (obj2 is IStatusUpdate)
                {
                    ((IStatusUpdate) obj2).UpdateStatus();
                }
            }
        }

        public static ContextMenuStrip CreateContextMenu(object owner, string addInTreePath)
        {
            if (addInTreePath == null)
            {
                return null;
            }
            try
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add(new ToolStripMenuItem("dummy"));
                contextMenu.Opening += delegate {
                    ToolStripMenuItem item;
                    contextMenu.Items.Clear();
                    if (addInTreePath == "ComponentTrayMenu")
                    {
                        item = new ToolStripMenuItem("锁定控件");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("粘贴");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("属性");
                        contextMenu.Items.Add(item);
                    }
                    else if (addInTreePath.EndsWith("ContainerMenu"))
                    {
                        item = new ToolStripMenuItem("锁定控件");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("粘贴");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("属性");
                        contextMenu.Items.Add(item);
                    }
                    else if (addInTreePath.EndsWith("SelectionMenu"))
                    {
                        item = new ToolStripMenuItem("置于顶层");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("置于底层");
                        contextMenu.Items.Add(item);
                        contextMenu.Items.Add(new ToolStripSeparator());
                        item = new ToolStripMenuItem("对齐到网格");
                        contextMenu.Items.Add(item);
                        contextMenu.Items.Add(new ToolStripSeparator());
                        item = new ToolStripMenuItem("锁定控件");
                        contextMenu.Items.Add(item);
                        contextMenu.Items.Add(new ToolStripSeparator());
                        item = new ToolStripMenuItem("剪切");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("复制");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("粘贴");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("删除");
                        contextMenu.Items.Add(item);
                        contextMenu.Items.Add(new ToolStripSeparator());
                        item = new ToolStripMenuItem("属性");
                        contextMenu.Items.Add(item);
                    }
                    else
                    {
                        if (!addInTreePath.EndsWith("TraySelectionMenu"))
                        {
                            throw new Exception();
                        }
                        item = new ToolStripMenuItem("剪切");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("复制");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("粘贴");
                        contextMenu.Items.Add(item);
                        item = new ToolStripMenuItem("删除");
                        contextMenu.Items.Add(item);
                    }
                };
                contextMenu.Opened += new EventHandler(MenuService.ContextMenuOpened);
                contextMenu.Closed += new ToolStripDropDownClosedEventHandler(MenuService.ContextMenuClosed);
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(contextMenu_ItemClicked);

                return contextMenu;
            }
            catch
            {
                MessageService.ShowError("Warning tree path '" + addInTreePath + "' not found.");
                return null;
            }
        }

        private static void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (OnMenuClick != null)
            {
                OnMenuClick(sender, e);
            }
        }


        public static void CreateQuickInsertMenu(TextBoxBase targetControl, Control popupControl, string[,] quickInsertMenuItems)
        {
            ContextMenuStrip quickInsertMenu = new ContextMenuStrip();
            for (int i = 0; i < quickInsertMenuItems.GetLength(0); i++)
            {
                if (quickInsertMenuItems[i, 0] == "-")
                {
                    quickInsertMenu.Items.Add(new ToolStripSeparator());
                }
                else
                {
                    MenuCommand command = new MenuCommand(quickInsertMenuItems[i, 0], new QuickInsertMenuHandler(targetControl, quickInsertMenuItems[i, 1]).EventHandler);
                    quickInsertMenu.Items.Add(command);
                }
            }
            new QuickInsertHandler(popupControl, quickInsertMenu);
        }

        public static void ShowContextMenu(object owner, string addInTreePath, Control parent, int x, int y)
        {
            ContextMenuStrip strip = CreateContextMenu(owner, addInTreePath);
            if (strip != null)
            {
                strip.Show(parent, new Point(x, y));
            }
        }

        public static bool IsContextMenuOpen
        {
            get
            {
                return isContextMenuOpen;
            }
        }

        private class QuickInsertHandler
        {
            private Control popupControl;
            private ContextMenuStrip quickInsertMenu;

            public QuickInsertHandler(Control popupControl, ContextMenuStrip quickInsertMenu)
            {
                this.popupControl = popupControl;
                this.quickInsertMenu = quickInsertMenu;
                popupControl.Click += new EventHandler(this.showQuickInsertMenu);
            }

            private void showQuickInsertMenu(object sender, EventArgs e)
            {
                Point position = new Point(this.popupControl.Width, 0);
                this.quickInsertMenu.Show(this.popupControl, position);
            }
        }

        private class QuickInsertMenuHandler
        {
            private TextBoxBase targetControl;
            private string text;

            public QuickInsertMenuHandler(TextBoxBase targetControl, string text)
            {
                this.targetControl = targetControl;
                this.text = text;
            }

            private void PopupMenuHandler(object sender, EventArgs e)
            {
                this.targetControl.SelectedText = this.targetControl.SelectedText + this.text;
            }

            public System.EventHandler EventHandler
            {
                get
                {
                    return new System.EventHandler(this.PopupMenuHandler);
                }
            }
        }
    }
}

