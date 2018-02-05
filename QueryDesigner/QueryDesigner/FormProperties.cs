using System.Windows.Forms;
using FormsDesigner.Gui;
using FormsDesigner;
using WinFormsUI.Docking;

namespace QueryDesigner
{
    public partial class FormProperties : DockContent, IPadContent
    {
        public FormProperties()
        {
            InitializeComponent();

            Icon = global::QueryDesigner.Properties.Resources.Property;
            _propertyPad = new PropertyPad();
            _propertyPad.Control.Dock = DockStyle.Fill;
            CloseButtonVisible = false;
            Controls.Add(_propertyPad.Control);
        }

        private PropertyPad _propertyPad;
        public PropertyPad PropertyPad
        {
            get { return _propertyPad; }
        }

        #region IPadContent 成员

        public void RedrawContent()
        {
            
        }

        public Control Control
        {
            get { return this; }
        }

        #endregion
    }
}
