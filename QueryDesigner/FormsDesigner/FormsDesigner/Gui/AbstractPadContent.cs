namespace FormsDesigner.Gui
{
    using FormsDesigner;
    using System;
    using System.Windows.Forms;

    public abstract class AbstractPadContent : IPadContent, IDisposable
    {
        protected AbstractPadContent()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void RedrawContent()
        {
        }

        public abstract System.Windows.Forms.Control Control { get; }

        public bool IsVisible
        {
            get
            {
                return (this.Control.Visible && (this.Control.Width > 0));
            }
        }
    }
}

