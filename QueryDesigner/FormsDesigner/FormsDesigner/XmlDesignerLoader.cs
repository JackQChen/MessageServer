namespace FormsDesigner
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics;
    using System.Xml;
    using FormsDesigner.Gui;
    using FormsDesigner.Services;

    public class XmlDesignerLoader : BasicDesignerLoader, IObjectCreator
    {
        private string _XmlContent;
        private IDesignerGenerator generator;
        private IDesignerLoaderHost host;

        public XmlDesignerLoader(IDesignerGenerator generator)
        {
            this._XmlContent = string.Empty;
            this.generator = generator;
        }

        public XmlDesignerLoader(IDesignerGenerator generator, string xmlContent)
        {
            this._XmlContent = string.Empty;
            this.generator = generator;
            this._XmlContent = xmlContent;
        }

        public override void BeginLoad(IDesignerLoaderHost host)
        {
            Debug.Assert(host != null);
            this.host = host;
            host.AddService(typeof(INameCreationService), new NameCreationService(host));
            host.AddService(typeof(ComponentSerializationService), new CodeDomComponentSerializationService(host));
            host.AddService(typeof(IDesignerSerializationService), new DesignerSerializationService(host));
            base.BeginLoad(host);
        }

        object IObjectCreator.CreateObject(string name, XmlElement el, object obj)
        {
            string attribute = null;
            if (el != null)
            {
                foreach (XmlNode node in el)
                {
                    if (node.Name == "Name")
                    {
                        attribute = ((XmlElement)node).GetAttribute("value");
                        break;
                    }
                }
            }
            Debug.Assert(attribute != null);
            System.Type componentClass = null;
            if (el.Attributes["type"] == null)
                componentClass = this.host.GetType(el.Name);
            else
                componentClass = this.host.GetType(el.Attributes["type"].Value);
            Debug.Assert(componentClass != null);
            if (componentClass == null)
                return null;
            object obj2 = null;
            //增加对象为无参构造时创建对象
            if (componentClass.GetConstructor(new Type[] { }) == null)
            {
                obj2 = System.Activator.CreateInstance(componentClass);
            }
            else
            {
                obj2 = this.host.CreateComponent(componentClass, attribute);
            }
            return obj2;
        }

        System.Type IObjectCreator.GetType(string name)
        {
            return this.host.GetType(name);
        }

        protected override void PerformFlush(IDesignerSerializationManager serializationManager)
        {
            this.generator.MergeFormChanges(null);
        }

        protected override void PerformLoad(IDesignerSerializationManager serializationManager)
        {
            FormsDesigner.Xml.XmlLoader loader = new FormsDesigner.Xml.XmlLoader();
            loader.ObjectCreator = this;
            if (this.XmlContent != string.Empty)
            {
                loader.CreateObjectFromXmlDefinition(this.XmlContent);
            }
        }

        public string XmlContent
        {
            get
            {
                return this._XmlContent;
            }
        }

        public class NameCreationService : INameCreationService
        {
            private IDesignerHost host;

            public NameCreationService(IDesignerHost host)
            {
                this.host = host;
            }

            public string CreateName(System.Type dataType)
            {
                return this.CreateName(this.host.Container, dataType);
            }

            public string CreateName(IContainer container, System.Type dataType)
            {
                string str = char.ToLower(dataType.Name[0]) + dataType.Name.Substring(1);
                int num = 1;
                //创建部分内部控件的container为空
                if (container == null)
                    return "UnName" + Guid.NewGuid().ToString().Split('-')[0];
                else
                    while (container.Components[str + num.ToString()] != null)
                    {
                        num++;
                    }
                return (str + num.ToString());
            }

            public bool IsValidName(string name)
            {
                if (((name == null) || (name.Length == 0)) || (!char.IsLetter(name[0]) && (name[0] != '_')))
                {
                    return false;
                }
                foreach (char ch in name)
                {
                    if (!(char.IsLetterOrDigit(ch) || (ch == '_')))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void ValidateName(string name)
            {
                if (!this.IsValidName(name))
                {
                    throw new Exception("Invalid name " + name);
                }
            }
        }
    }
}

