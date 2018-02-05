namespace FormsDesigner.Services
{
    using System;
    using System.Collections;

    public class ServiceManager
    {
        private static ServiceManager defaultServiceManager = new ServiceManager();
        private ArrayList serviceList = new ArrayList();
        private Hashtable servicesHashtable = new Hashtable();

        private ServiceManager()
        {
        }

        public void AddService(IService service)
        {
            this.serviceList.Add(service);
        }

        public void AddServices(IService[] services)
        {
            foreach (IService service in services)
            {
                this.AddService(service);
            }
        }

        public IService GetService(Type serviceType)
        {
            IService service = (IService) this.servicesHashtable[serviceType];
            if (service != null)
            {
                return service;
            }
            foreach (IService service2 in this.serviceList)
            {
                if (this.IsInstanceOfType(serviceType, service2))
                {
                    this.servicesHashtable[serviceType] = service2;
                    return service2;
                }
            }
            return null;
        }

        private bool IsInstanceOfType(Type type, IService service)
        {
            Type baseType = service.GetType();
            foreach (Type type3 in baseType.GetInterfaces())
            {
                if (type3 == type)
                {
                    return true;
                }
            }
            while (baseType != typeof(object))
            {
                if (type == baseType)
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }
            return false;
        }

        public void UnloadAllServices()
        {
            foreach (IService service in this.serviceList)
            {
                service.UnloadService();
            }
        }

        public static ServiceManager Services
        {
            get
            {
                return defaultServiceManager;
            }
        }
    }
}

