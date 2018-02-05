namespace FormsDesigner.Gui
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public abstract class AbstractBaseViewContent : IBaseViewContent, IDisposable
    {
        private IWorkbenchWindow workbenchWindow = null;

        public event EventHandler WorkbenchWindowChanged;

        protected AbstractBaseViewContent()
        {
        }

        public virtual void Deselected()
        {
        }

        public virtual void Deselecting()
        {
        }

        public virtual void Dispose()
        {
            this.workbenchWindow = null;
        }

        protected virtual void OnWorkbenchWindowChanged(EventArgs e)
        {
            if (this.WorkbenchWindowChanged != null)
            {
                this.WorkbenchWindowChanged(this, e);
            }
        }

        public virtual void RedrawContent()
        {
        }

        public virtual void Selected()
        {
        }

        public virtual void SwitchedTo()
        {
        }

        public abstract System.Windows.Forms.Control Control { get; }

        public virtual string TabPageText
        {
            get
            {
                return "Abstract Content";
            }
        }

        public virtual IWorkbenchWindow WorkbenchWindow
        {
            get
            {
                return this.workbenchWindow;
            }
            set
            {
                this.workbenchWindow = value;
                this.OnWorkbenchWindowChanged(EventArgs.Empty);
            }
        }
    }
}

