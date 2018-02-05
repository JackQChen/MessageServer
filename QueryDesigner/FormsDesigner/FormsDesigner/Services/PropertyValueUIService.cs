namespace FormsDesigner.Services
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;

    public class PropertyValueUIService : IPropertyValueUIService
    {
        private PropertyValueUIHandler propertyValueUIHandler;

        public event EventHandler PropertyUIValueItemsChanged;

        public void AddPropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            this.propertyValueUIHandler = (PropertyValueUIHandler) Delegate.Combine(this.propertyValueUIHandler, newHandler);
        }

        public PropertyValueUIItem[] GetPropertyUIValueItems(ITypeDescriptorContext context, PropertyDescriptor propDesc)
        {
            ArrayList valueUIItemList = new ArrayList();
            if (this.propertyValueUIHandler != null)
            {
                this.propertyValueUIHandler(context, propDesc, valueUIItemList);
            }
            PropertyValueUIItem[] array = new PropertyValueUIItem[valueUIItemList.Count];
            if (valueUIItemList.Count > 0)
            {
                valueUIItemList.CopyTo(array);
            }
            return array;
        }

        public void NotifyPropertyValueUIItemsChanged()
        {
            this.OnPropertyUIValueItemsChanged(EventArgs.Empty);
        }

        protected virtual void OnPropertyUIValueItemsChanged(EventArgs e)
        {
            if (this.PropertyUIValueItemsChanged != null)
            {
                this.PropertyUIValueItemsChanged(this, e);
            }
        }

        public void RemovePropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            this.propertyValueUIHandler = (PropertyValueUIHandler) Delegate.Remove(this.propertyValueUIHandler, newHandler);
        }
    }
}

