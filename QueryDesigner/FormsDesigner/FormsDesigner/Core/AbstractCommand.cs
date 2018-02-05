namespace FormsDesigner.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AbstractCommand : ICommand
    {
        private object owner = null;

        public event EventHandler OwnerChanged;

        protected AbstractCommand()
        {
        }

        protected virtual void OnOwnerChanged(EventArgs e)
        {
            if (this.OwnerChanged != null)
            {
                this.OwnerChanged(this, e);
            }
        }

        public abstract void Run();

        public virtual object Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
                this.OnOwnerChanged(EventArgs.Empty);
            }
        }
    }
}

