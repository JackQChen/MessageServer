namespace FormsDesigner.Gui
{
    using System;

    public class SaveEventArgs : EventArgs
    {
        private bool successful;

        public SaveEventArgs(bool successful)
        {
            this.successful = successful;
        }

        public bool Successful
        {
            get
            {
                return this.successful;
            }
        }
    }
}

