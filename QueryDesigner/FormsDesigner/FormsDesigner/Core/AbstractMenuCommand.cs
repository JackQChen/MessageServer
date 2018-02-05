namespace FormsDesigner.Core
{
    using System;

    public abstract class AbstractMenuCommand : AbstractCommand, IMenuCommand, ICommand
    {
        private bool isEnabled = true;

        protected AbstractMenuCommand()
        {
        }

        public virtual bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
            }
        }
    }
}

