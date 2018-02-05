namespace FormsDesigner.Services
{
    using FormsDesigner.Core;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    public class DefaultServiceContainer : IServiceContainer, IServiceProvider, IDisposable
    {
        private bool inDispose;
        private IServiceContainer serviceContainer;
        private Hashtable services;

        public DefaultServiceContainer()
        {
            this.services = new Hashtable();
            this.inDispose = false;
            this.serviceContainer = new ServiceContainer();
        }

        public DefaultServiceContainer(IServiceContainer parent)
        {
            this.services = new Hashtable();
            this.inDispose = false;
            this.serviceContainer = new ServiceContainer(parent);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            if (this.IsServiceMissing(serviceType))
            {
                this.serviceContainer.AddService(serviceType, callback);
            }
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            if (this.IsServiceMissing(serviceType))
            {
                this.serviceContainer.AddService(serviceType, serviceInstance);
                this.services.Add(serviceType, serviceInstance);
            }
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            if (this.IsServiceMissing(serviceType))
            {
                this.serviceContainer.AddService(serviceType, callback, promote);
            }
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            if (this.IsServiceMissing(serviceType))
            {
                this.serviceContainer.AddService(serviceType, serviceInstance, promote);
                this.services.Add(serviceType, serviceInstance);
            }
        }

        public virtual void Dispose()
        {
            this.inDispose = true;
            foreach (DictionaryEntry entry in this.services)
            {
                if (entry.Value == this)
                {
                    continue;
                }
                IDisposable disposable = entry.Value as IDisposable;
                if (disposable != null)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (Exception exception)
                    {
                        MessageService.ShowError(exception, "Exception while disposing " + disposable);
                    }
                }
            }
            this.services.Clear();
            this.services = null;
            this.inDispose = false;
        }

        public object GetService(Type serviceType)
        {
            return this.serviceContainer.GetService(serviceType);
        }

        private bool IsServiceMissing(Type serviceType)
        {
            return (this.serviceContainer.GetService(serviceType) == null);
        }

        public void RemoveService(Type serviceType)
        {
            if (!this.inDispose)
            {
                this.serviceContainer.RemoveService(serviceType);
                if (this.services.Contains(serviceType))
                {
                    this.services.Remove(serviceType);
                }
            }
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            if (!this.inDispose)
            {
                this.serviceContainer.RemoveService(serviceType, promote);
                if (this.services.Contains(serviceType))
                {
                    this.services.Remove(serviceType);
                }
            }
        }
    }
}

