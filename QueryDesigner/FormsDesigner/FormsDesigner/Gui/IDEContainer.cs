namespace FormsDesigner.Gui
{
    using System;
    using System.ComponentModel;

    internal class IDEContainer : Container
    {
        private IServiceProvider serviceProvider;

        public IDEContainer(IServiceProvider sp)
        {
            this.serviceProvider = sp;
        }

        public ISite CreateSite(IComponent component)
        {
            return this.CreateSite(component, "UNKNOWN_SITE");
        }

        protected override ISite CreateSite(IComponent component, string name)
        {
            if (base.CreateSite(component, name) == null)
            {
            }
            return new IDESite(component, this, name);
        }

        protected override object GetService(Type serviceType)
        {
            object service = base.GetService(serviceType);
            if (service == null)
            {
                service = this.serviceProvider.GetService(serviceType);
            }
            return service;
        }

        private class IDESite : ISite, IServiceProvider
        {
            private IComponent component;
            private IDEContainer container;
            private string name = "";

            public IDESite(IComponent sitedComponent, IDEContainer site, string aName)
            {
                this.component = sitedComponent;
                this.container = site;
                this.name = aName;
            }

            public object GetService(Type serviceType)
            {
                return this.container.GetService(serviceType);
            }

            public IComponent Component
            {
                get
                {
                    return this.component;
                }
            }

            public IContainer Container
            {
                get
                {
                    return this.container;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }
            }
        }
    }
}

