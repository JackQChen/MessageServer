namespace FormsDesigner.Gui
{
    using System;
    using System.Collections;
    using System.ComponentModel.Design;

    public sealed class PropertyContainer
    {
        private IDesignerHost host;
        private ICollection selectableObjects;
        private object selectedObject;
        private object[] selectedObjects;

        public void Clear()
        {
            this.Host = null;
            this.SelectableObjects = null;
            this.SelectedObject = null;
        }

        public IDesignerHost Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
                PropertyPad.UpdateHostIfActive(this);
            }
        }

        public ICollection SelectableObjects
        {
            get
            {
                return this.selectableObjects;
            }
            set
            {
                this.selectableObjects = value;
                PropertyPad.UpdateSelectableIfActive(this);
            }
        }

        public object SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;
                this.selectedObjects = null;
                PropertyPad.UpdateSelectedObjectIfActive(this);
            }
        }

        public object[] SelectedObjects
        {
            get
            {
                return this.selectedObjects;
            }
            set
            {
                this.selectedObject = null;
                this.selectedObjects = value;
                PropertyPad.UpdateSelectedObjectIfActive(this);
            }
        }
    }
}

