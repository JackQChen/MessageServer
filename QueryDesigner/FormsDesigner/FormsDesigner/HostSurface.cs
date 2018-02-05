namespace FormsDesigner
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class HostSurface : DesignSurface
    {
        private BasicDesignerLoader _loader;
        private ISelectionService _selectionService;

        public HostSurface()
        {
            this.AddService(typeof(IMenuCommandService), new MenuCommandService(this));
        }

        public HostSurface(IServiceProvider parentProvider) : base(parentProvider)
        {
            this.AddService(typeof(IMenuCommandService), new MenuCommandService(this));
        }

        public void AddService(System.Type type, object serviceInstance)
        {
            base.ServiceContainer.AddService(type, serviceInstance);
        }

        internal void Initialize()
        {
            Control view = null;
            IDesignerHost service = (IDesignerHost) base.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                try
                {
                    System.Type type = service.RootComponent.GetType();
                    if (type == typeof(Form))
                    {
                        view = base.View as Control;
                        view.BackColor = Color.White;
                    }
                    else if (type == typeof(UserControl))
                    {
                        view = base.View as Control;
                        view.BackColor = Color.White;
                    }
                    else
                    {
                        if (type != typeof(Component))
                        {
                            throw new Exception("Undefined Host Type: " + type.ToString());
                        }
                        view = base.View as Control;
                        view.BackColor = Color.FloralWhite;
                    }
                    this._selectionService = (ISelectionService) base.ServiceContainer.GetService(typeof(ISelectionService));
                    this._selectionService.SelectionChanged += new EventHandler(this.selectionService_SelectionChanged);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception.ToString());
                }
            }
        }

        private void selectionService_SelectionChanged(object sender, EventArgs e)
        {
            if (this._selectionService != null)
            {
                ICollection selectedComponents = this._selectionService.GetSelectedComponents();
                PropertyGrid service = (PropertyGrid) base.GetService(typeof(PropertyGrid));
                object[] objArray = new object[selectedComponents.Count];
                int index = 0;
                foreach (object obj2 in selectedComponents)
                {
                    objArray[index] = obj2;
                    index++;
                }
                service.SelectedObjects = objArray;
            }
        }

        public BasicDesignerLoader Loader
        {
            get
            {
                return this._loader;
            }
            set
            {
                this._loader = value;
            }
        }
    }
}

