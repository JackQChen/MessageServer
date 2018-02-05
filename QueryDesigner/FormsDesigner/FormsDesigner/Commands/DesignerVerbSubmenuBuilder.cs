namespace FormsDesigner.Commands
{
    using FormsDesigner.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    public class DesignerVerbSubmenuBuilder
    {
        public ToolStripItem[] BuildSubmenu(object owner)
        {
            IMenuCommandService service = (IMenuCommandService) owner;
            List<ToolStripItem> list = new List<ToolStripItem>();
            foreach (DesignerVerb verb in service.Verbs)
            {
            }
            if (list.Count > 0)
            {
            }
            return list.ToArray();
        }

        private class ContextMenuCommand : FormsDesigner.Core.MenuCommand
        {
            private DesignerVerb verb;

            public ContextMenuCommand(DesignerVerb verb) : base(verb.Text)
            {
                this.Enabled = verb.Enabled;
                this.verb = verb;
                base.Click += new EventHandler(this.InvokeCommand);
            }

            private void InvokeCommand(object sender, EventArgs e)
            {
                try
                {
                    this.verb.Invoke();
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception);
                }
            }
        }
    }
}

