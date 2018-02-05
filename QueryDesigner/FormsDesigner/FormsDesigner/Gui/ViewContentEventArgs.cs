namespace FormsDesigner.Gui
{
    using System;

    public class ViewContentEventArgs : EventArgs
    {
        private IViewContent content;

        public ViewContentEventArgs(IViewContent content)
        {
            this.content = content;
        }

        public IViewContent Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
    }
}

