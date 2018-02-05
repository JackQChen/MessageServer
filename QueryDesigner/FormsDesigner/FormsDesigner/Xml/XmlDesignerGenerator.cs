namespace FormsDesigner.Xml
{
    using FormsDesigner;
    using FormsDesigner.Core;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public class XmlDesignerGenerator : IDesignerGenerator
    {
        private FormsDesignerViewContent viewContent;

        public void Attach(FormsDesignerViewContent viewContent)
        {
            this.viewContent = viewContent;
        }

        public void Detach()
        {
            this.viewContent = null;
        }

        public ICollection GetCompatibleMethods(EventDescriptor edesc)
        {
            return new object[0];
        }

        public ICollection GetCompatibleMethods(EventInfo edesc)
        {
            return new object[0];
        }

        public XmlElement GetElementFor(XmlDocument doc, IDesignerHost host)
        {
            XmlElement element = doc.CreateElement("Components");
            XmlAttribute node = doc.CreateAttribute("version");
            node.InnerText = "1.0";
            element.Attributes.Append(node);
            Hashtable visitedControls = new Hashtable();
            element.AppendChild(this.GetElementFor(doc, host.RootComponent, visitedControls));
            foreach (IComponent component in host.Container.Components)
            {
                if (!((component is Control) || visitedControls.ContainsKey(component)))
                {
                    element.AppendChild(this.GetElementFor(doc, component, visitedControls));
                }
            }
            return element;
        }

        public XmlElement GetElementFor(XmlDocument doc, object o, Hashtable visitedControls)
        {
            Exception exception;
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            visitedControls[o] = null;
            try
            {
                //使用检索控件处理遍历时出现无限递归问题
                if (o.GetType().ToString().Contains("GridColumnSummaryItem"))
                    return null;
                XmlElement element2;
                XmlAttribute attribute2;
                XmlElement element = doc.CreateElement(XmlConvert.EncodeName(o.GetType().FullName));
                XmlAttribute node = doc.CreateAttribute("type");
                node.Value = o.GetType().AssemblyQualifiedName;
                element.Attributes.Append(node);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(o);
                Control control = o as Control;
                bool flag = false;
                if (control != null)
                {
                    element2 = doc.CreateElement("Name");
                    attribute2 = doc.CreateAttribute("value");
                    //处理遍历检索控件时不需要检索其内部控件 
                    if (control.Parent.GetType().ToString().StartsWith("SnControl.Search"))
                        return null;
                    //控件名称为空时不添加
                    if (control.Name == string.Empty)
                        return null;
                    attribute2.InnerText = control.Name;
                    element2.Attributes.Append(attribute2);
                    element.AppendChild(element2);
                    flag = true;
                }
                ArrayList list = new ArrayList();
                foreach (PropertyDescriptor descriptor in properties)
                {
                    //检索控件创建该属性时出现类型不匹配问题
                    if (o.GetType().ToString().StartsWith("SnControl.Search"))
                        if (descriptor.Name.Contains("Properties"))
                            continue;
                    if (o.GetType().ToString().StartsWith("SnControl.ParamTextBox"))
                        if (descriptor.Name.Contains("Lines"))
                            continue;
                    if (((descriptor.Name == "Name") && flag) || ((((descriptor.Name == "DataBindings") || (descriptor.Name == "FlatAppearance")) || ((o.GetType().FullName == "System.Windows.Forms.TabControl") && (descriptor.Name == "Controls"))) || ((o.GetType().FullName == "System.Windows.Forms.DataGridView") && (descriptor.Name == "Controls"))))
                    {
                        continue;
                    }
                    element2 = null;
                    if (((descriptor.Name == "Size") && (control != null)) && ((control is UserControl) || (control is Form)))
                    {
                        element2 = doc.CreateElement("ClientSize");
                        element2.SetAttribute("value", control.ClientSize.ToString());
                        list.Insert(0, element2);
                        continue;
                    }
                    element2 = doc.CreateElement(XmlConvert.EncodeName(descriptor.Name));
                    object obj2 = null;
                    try
                    {
                        obj2 = descriptor.GetValue(o);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        //LoggingService.Warn(exception);
                        continue;
                    }
                    if ((obj2 is IList) && !(control is PropertyGrid))
                    {
                        foreach (object obj3 in (IList)obj2)
                        {
                            XmlElement newChild = this.GetElementFor(doc, obj3, visitedControls);
                            if (!((newChild == null) || newChild.Name.StartsWith("System.Windows.Forms.Design.")))
                            {
                                element2.AppendChild(newChild);
                            }
                        }
                        if (element2.ChildNodes.Count > 0)
                        {
                            list.Add(element2);
                        }
                    }
                    else if (descriptor.ShouldSerializeValue(o) && descriptor.IsBrowsable)
                    {
                        attribute2 = doc.CreateAttribute("value");
                        if (obj2 == null)
                        {
                            attribute2.InnerText = null;
                        }
                        else if (obj2 is Color)
                        {
                            attribute2.InnerText = obj2.ToString();
                        }
                        else
                        {
                            attribute2.InnerText = TypeDescriptor.GetConverter(descriptor.PropertyType).ConvertToInvariantString(obj2);
                        }
                        element2.Attributes.Append(attribute2);
                        list.Insert(0, element2);
                    }
                }
                foreach (XmlElement element3 in list)
                {
                    element.AppendChild(element3);
                }
                if (element.ChildNodes.Count == 0)
                {
                    attribute2 = doc.CreateAttribute("value");
                    attribute2.InnerText = o.ToString();
                    element.Attributes.Append(attribute2);
                }
                return element;
            }
            catch (Exception exception2)
            {
                exception = exception2;
                MessageService.ShowError(exception);
            }
            return null;
        }

        public bool InsertComponentEvent(IComponent component, EventDescriptor edesc, string eventMethodName, string body, out string file, out int position)
        {
            position = 0;
            file = this.viewContent.Control.Name;
            return false;
        }

        public void MergeFormChanges(CodeCompileUnit unit)
        {
            StringWriter w = new StringWriter();
            XmlTextWriter writer2 = new XmlTextWriter(w);
            writer2.Formatting = Formatting.Indented;
            XmlElement elementFor = this.GetElementFor(new XmlDocument(), this.viewContent.Host);
            writer2.WriteStartElement(elementFor.Name);
            writer2.WriteAttributeString("version", "1.0");
            foreach (XmlNode node in elementFor.ChildNodes)
            {
                node.WriteTo(writer2);
            }
            writer2.WriteEndElement();
        }

        public System.CodeDom.Compiler.CodeDomProvider CodeDomProvider
        {
            get
            {
                return new CSharpCodeProvider();
            }
        }
    }
}

