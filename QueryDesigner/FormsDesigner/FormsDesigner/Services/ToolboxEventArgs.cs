namespace FormsDesigner.Services
{
    using System;
    using System.ComponentModel.Design;
    using System.Drawing.Design;

    public class ToolboxEventArgs : EventArgs
    {
        private string category = null;
        private IDesignerHost host = null;
        private ToolboxItem item = null;

        public ToolboxEventArgs(ToolboxItem item, string category, IDesignerHost host)
        {
            this.item = item;
            this.category = category;
            this.host = host;
        }

        public string Category
        {
            get
            {
                return this.category;
            }
        }

        public IDesignerHost Host
        {
            get
            {
                return this.host;
            }
        }

        public ToolboxItem Item
        {
            get
            {
                return this.item;
            }
        }
    }
}

