namespace FormsDesigner
{
    using System;
    using System.ComponentModel.Design.Serialization;

    public class XmlDesignerLoaderProvider : IDesignerLoaderProvider
    {
        private string _XmlContent = string.Empty;

        public XmlDesignerLoaderProvider(string xmlContent)
        {
            this._XmlContent = xmlContent;
        }

        public DesignerLoader CreateLoader(IDesignerGenerator generator)
        {
            return new XmlDesignerLoader(generator, this._XmlContent);
        }
    }
}

