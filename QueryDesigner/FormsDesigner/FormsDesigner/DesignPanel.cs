namespace FormsDesigner
{
    using FormsDesigner.Services;
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class DesignPanel : Panel
    {
        private DesignSurface _HostSurface;
        private Control view;

        public DesignPanel(DesignSurface hostSurface)
        {
            Debug.Assert(hostSurface != null);
            this._HostSurface = hostSurface;
            this.BackColor = Color.White;
        }

        public void Disable()
        {
            base.Controls.Clear();
        }

        public void Enable()
        {
            base.Controls.Add(this.view);
        }

        public void SetErrorState(string errors)
        {
            this.Disable();
        }

        public bool SetRootDesigner()
        {
            if (this._HostSurface == null)
            {
                ((IMessageService)this.Host.GetService(typeof(IMessageService))).ShowError("不能为 " + this.Host.RootComponent + "构建设计器！");
            }
            else
            {
                if (this.view != null)
                {
                    base.Controls.Clear();
                    this.view.Dispose();
                }
                try
                {
                    this.view = (Control)this._HostSurface.View;
                    this.view.BackColor = Color.White;
                    this.view.Dock = DockStyle.Fill;
                    this.view.Visible = true;
                    base.Controls.Add(this.view);
                }
                catch { return false; }
            }
            return true;
        }

        public IDesignerHost Host
        {
            get
            {
                return (IDesignerHost)this._HostSurface.GetService(typeof(IDesignerHost));
            }
        }

        public Control View
        {
            get
            {
                return this.view;
            }
        }
    }
}

