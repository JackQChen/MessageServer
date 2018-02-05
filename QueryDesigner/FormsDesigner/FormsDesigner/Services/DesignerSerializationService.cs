namespace FormsDesigner.Services
{
    using System;
    using System.Collections;
    using System.ComponentModel.Design.Serialization;

    public class DesignerSerializationService : IDesignerSerializationService
    {
        private IServiceProvider serviceProvider;

        public DesignerSerializationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICollection Deserialize(object serializationData)
        {
            ComponentSerializationService service = (ComponentSerializationService) this.serviceProvider.GetService(typeof(ComponentSerializationService));
            return service.Deserialize((SerializationStore) serializationData);
        }

        public object Serialize(ICollection objects)
        {
            ComponentSerializationService service = (ComponentSerializationService) this.serviceProvider.GetService(typeof(ComponentSerializationService));
            SerializationStore store = service.CreateStore();
            foreach (object obj2 in objects)
            {
                service.Serialize(store, obj2);
            }
            store.Close();
            return store;
        }
    }
}

