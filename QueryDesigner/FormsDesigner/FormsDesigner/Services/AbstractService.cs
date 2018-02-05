namespace FormsDesigner.Services
{
    using System;
    using System.Runtime.CompilerServices;

    public class AbstractService : IService
    {
        public event EventHandler Initialize;

        public event EventHandler Unload;

        public virtual void InitializeService()
        {
            this.OnInitialize(EventArgs.Empty);
        }

        protected virtual void OnInitialize(EventArgs e)
        {
            if (this.Initialize != null)
            {
                this.Initialize(this, e);
            }
        }

        protected virtual void OnUnload(EventArgs e)
        {
            if (this.Unload != null)
            {
                this.Unload(this, e);
            }
        }

        public virtual void UnloadService()
        {
            this.OnUnload(EventArgs.Empty);
        }
    }
}

