namespace FormsDesigner.Core
{
    using System;

    public abstract class AbstractCheckableMenuCommand : AbstractMenuCommand, ICheckableMenuCommand, IMenuCommand, ICommand
    {
        private bool isChecked = false;

        protected AbstractCheckableMenuCommand()
        {
        }

        public override void Run()
        {
            this.IsChecked = !this.IsChecked;
        }

        public virtual bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
            }
        }
    }
}

