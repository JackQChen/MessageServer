namespace FormsDesigner.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime.CompilerServices;

    public class DesignerEventService : IDesignerEventService
    {
        private IDesignerHost activeDesigner = null;
        private List<IDesignerHost> designers = new List<IDesignerHost>();

        public event ActiveDesignerEventHandler ActiveDesignerChanged;

        public event DesignerEventHandler DesignerCreated;

        public event DesignerEventHandler DesignerDisposed;

        public event EventHandler SelectionChanged;

        public void AddDesigner(IDesignerHost host)
        {
            this.designers.Add(host);
            if (this.designers.Count == 1)
            {
                this.SetActiveDesigner(host);
            }
            this.OnDesignerCreated(new DesignerEventArgs(host));
        }

        public void FileSelectionChanged()
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnActiveDesignerChanged(ActiveDesignerEventArgs e)
        {
            if (this.ActiveDesignerChanged != null)
            {
                this.ActiveDesignerChanged(this, e);
            }
        }

        protected virtual void OnDesignerCreated(DesignerEventArgs e)
        {
            if (this.DesignerCreated != null)
            {
                this.DesignerCreated(this, e);
            }
        }

        protected virtual void OnDesignerDisposed(DesignerEventArgs e)
        {
            if (this.DesignerDisposed != null)
            {
                this.DesignerDisposed(this, e);
            }
        }

        public void RemoveDesigner(IDesignerHost host)
        {
            this.designers.Remove(host);
            if (this.activeDesigner == host)
            {
                if (this.designers.Count <= 0)
                {
                    this.SetActiveDesigner(null);
                }
                else
                {
                    this.SetActiveDesigner(this.designers[this.designers.Count - 1]);
                }
            }
            ((IContainer) host).Dispose();
            this.OnDesignerDisposed(new DesignerEventArgs(host));
        }

        public void Reset()
        {
            this.activeDesigner = null;
            this.designers.Clear();
        }

        public void SetActiveDesigner(IDesignerHost host)
        {
            if (this.activeDesigner != host)
            {
                IDesignerHost activeDesigner = this.activeDesigner;
                this.activeDesigner = host;
                this.FileSelectionChanged();
                this.OnActiveDesignerChanged(new ActiveDesignerEventArgs(activeDesigner, host));
            }
        }

        public IDesignerHost ActiveDesigner
        {
            get
            {
                return this.activeDesigner;
            }
        }

        public DesignerCollection Designers
        {
            get
            {
                return new DesignerCollection(this.designers);
            }
        }
    }
}

