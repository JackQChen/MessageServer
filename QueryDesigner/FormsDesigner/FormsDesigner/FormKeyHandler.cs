namespace FormsDesigner
{
    using FormsDesigner.Core;
    using FormsDesigner.Gui;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class FormKeyHandler : IMessageFilter
    {
        public static bool inserted = false;
        private const int keyPressedMessage = 0x100;
        private Hashtable keyTable = new Hashtable();
        private const int leftMouseButtonDownMessage = 0x202;

        public FormKeyHandler()
        {
            this.keyTable[Keys.Left] = new CommandWrapper(MenuCommands.KeyMoveLeft);
            this.keyTable[Keys.Right] = new CommandWrapper(MenuCommands.KeyMoveRight);
            this.keyTable[Keys.Up] = new CommandWrapper(MenuCommands.KeyMoveUp);
            this.keyTable[Keys.Down] = new CommandWrapper(MenuCommands.KeyMoveDown);
            this.keyTable[Keys.Tab] = new CommandWrapper(MenuCommands.KeySelectNext);
            this.keyTable[Keys.Delete] = new CommandWrapper(StandardCommands.Delete);
            this.keyTable[Keys.Back] = new CommandWrapper(StandardCommands.Delete);
            this.keyTable[Keys.Shift | Keys.Left] = new CommandWrapper(MenuCommands.KeySizeWidthDecrease);
            this.keyTable[Keys.Shift | Keys.Right] = new CommandWrapper(MenuCommands.KeySizeWidthIncrease);
            this.keyTable[Keys.Shift | Keys.Up] = new CommandWrapper(MenuCommands.KeySizeHeightDecrease);
            this.keyTable[Keys.Shift | Keys.Down] = new CommandWrapper(MenuCommands.KeySizeHeightIncrease);
            this.keyTable[Keys.Shift | Keys.Tab] = new CommandWrapper(MenuCommands.KeySelectPrevious);
            this.keyTable[Keys.Shift | Keys.Delete] = new CommandWrapper(StandardCommands.Delete);
            this.keyTable[Keys.Shift | Keys.Back] = new CommandWrapper(StandardCommands.Delete);
            this.keyTable[Keys.Control | Keys.Left] = new CommandWrapper(MenuCommands.KeyNudgeLeft);
            this.keyTable[Keys.Control | Keys.Right] = new CommandWrapper(MenuCommands.KeyNudgeRight);
            this.keyTable[Keys.Control | Keys.Up] = new CommandWrapper(MenuCommands.KeyNudgeUp);
            this.keyTable[Keys.Control | Keys.Down] = new CommandWrapper(MenuCommands.KeyNudgeDown);
            this.keyTable[Keys.Control | Keys.Shift | Keys.Left] = new CommandWrapper(MenuCommands.KeyNudgeWidthDecrease);
            this.keyTable[Keys.Control | Keys.Shift | Keys.Right] = new CommandWrapper(MenuCommands.KeyNudgeWidthIncrease);
            this.keyTable[Keys.Control | Keys.Shift | Keys.Up] = new CommandWrapper(MenuCommands.KeyNudgeHeightDecrease);
            this.keyTable[Keys.Control | Keys.Shift | Keys.Down] = new CommandWrapper(MenuCommands.KeyNudgeHeightIncrease);
        }

        private bool HandleMenuCommand(FormsDesignerViewContent formDesigner, IComponent activeComponent, Keys keyPressed)
        {
            System.Type serviceType = typeof(WindowsFormsDesignerOptionService).Assembly.GetType("System.Windows.Forms.Design.ToolStripKeyboardHandlingService");
            object service = formDesigner.Host.GetService(serviceType);
            if (service == null)
            {
                //LoggingService.Debug("no ToolStripKeyboardHandlingService found");
                return false;
            }
            if (activeComponent is ToolStripItem)
            {
                if (keyPressed == Keys.Up)
                {
                    serviceType.InvokeMember("ProcessUpDown", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, service, new object[] { false });
                    return true;
                }
                if (keyPressed == Keys.Down)
                {
                    serviceType.InvokeMember("ProcessUpDown", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, service, new object[] { true });
                    return true;
                }
            }
            return (bool) serviceType.InvokeMember("TemplateNodeActive", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, service, null);
        }

        public static void Insert()
        {
            inserted = true;
            Application.AddMessageFilter(new FormKeyHandler());
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                if (WorkbenchSingleton.Workbench.ActiveContent == null)
                {
                    return false;
                }
                if (!WorkbenchSingleton.Workbench.IsActiveWindow)
                {
                    return false;
                }
                FormsDesignerViewContent activeContent = WorkbenchSingleton.Workbench.ActiveContent as FormsDesignerViewContent;
                if (activeContent == null)
                {
                    return false;
                }
                Keys keyPressed = ((Keys) m.WParam.ToInt32()) | Control.ModifierKeys;
                if ((keyPressed == Keys.Escape) && activeContent.IsTabOrderMode)
                {
                    activeContent.HideTabOrder();
                    return true;
                }
                CommandWrapper wrapper = (CommandWrapper) this.keyTable[keyPressed];
                if (wrapper != null)
                {
                    if ((wrapper.CommandID == StandardCommands.Delete) && !activeContent.EnableDelete)
                    {
                        return false;
                    }
                    //LoggingService.Debug("Run menu command: " + wrapper.CommandID);
                    Control activeControl = WorkbenchSingleton.ActiveControl;
                    IMenuCommandService service = (IMenuCommandService) activeContent.Host.GetService(typeof(IMenuCommandService));
                    ISelectionService service2 = (ISelectionService) activeContent.Host.GetService(typeof(ISelectionService));
                    ICollection selectedComponents = service2.GetSelectedComponents();
                    if (selectedComponents.Count == 1)
                    {
                        foreach (IComponent component in selectedComponents)
                        {
                            if (this.HandleMenuCommand(activeContent, component, keyPressed))
                            {
                                return false;
                            }
                        }
                    }
                    service.GlobalInvoke(wrapper.CommandID);
                    if (wrapper.RestoreSelection)
                    {
                        service2.SetSelectedComponents(selectedComponents);
                    }
                    return true;
                }
            }
            return false;
        }

        private class CommandWrapper
        {
            private System.ComponentModel.Design.CommandID commandID;
            private bool restoreSelection;

            public CommandWrapper(System.ComponentModel.Design.CommandID commandID) : this(commandID, false)
            {
            }

            public CommandWrapper(System.ComponentModel.Design.CommandID commandID, bool restoreSelection)
            {
                this.commandID = commandID;
                this.restoreSelection = restoreSelection;
            }

            public System.ComponentModel.Design.CommandID CommandID
            {
                get
                {
                    return this.commandID;
                }
            }

            public bool RestoreSelection
            {
                get
                {
                    return this.restoreSelection;
                }
            }
        }
    }
}

