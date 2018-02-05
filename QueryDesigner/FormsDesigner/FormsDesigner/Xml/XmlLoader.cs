namespace FormsDesigner.Xml
{
    using FormsDesigner.Gui;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    public class XmlLoader
    {
        private string acceptButton = "";
        private string acceptButtonName = string.Empty;
        private string cancelButton = "";
        private string cancelButtonName = string.Empty;
        private Dictionary<string, Control> controlDictionary = new Dictionary<string, Control>();
        private object customizationObject;
        private Form mainForm = null;
        private int num = 0;
        private IObjectCreator objectCreator = new DefaultObjectCreator();
        private static readonly Regex propertySet = new Regex(@"(?<Property>[\w]+)\s*=\s*(?<Value>[\w\d]+)", RegexOptions.Compiled);
        private IPropertyValueCreator propertyValueCreator = null;
        private IStringValueFilter stringValueFilter = null;
        private Hashtable tooltips = new Hashtable();

        public object CreateObjectFromFileDefinition(string fileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            XmlElement documentElement = document.DocumentElement;
            if (document.DocumentElement.Attributes["version"] != null)
            {
                documentElement = (XmlElement)document.DocumentElement.ChildNodes[0];
            }
            this.customizationObject = this.objectCreator.CreateObject(XmlConvert.DecodeName(documentElement.Name), documentElement, null);
            this.SetUpObject(this.customizationObject, documentElement);
            return this.customizationObject;
        }

        public object CreateObjectFromXmlDefinition(string xmlContent)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);
            XmlElement documentElement = document.DocumentElement;
            if (document.DocumentElement.Attributes["version"] != null)
            {
                documentElement = (XmlElement)document.DocumentElement.ChildNodes[0];
            }
            this.customizationObject = this.objectCreator.CreateObject(XmlConvert.DecodeName(documentElement.Name), documentElement, null);
            this.SetUpObject(this.customizationObject, documentElement);
            return this.customizationObject;
        }

        public T Get<T>(string name) where T : Control
        {
            string key = name + typeof(T).Name;
            if (!this.ControlDictionary.ContainsKey(key))
            {
                throw new ArgumentException("Control " + key + " not found!", "name");
            }
            return (this.ControlDictionary[key] as T);
        }

        public void LoadObjectFromFileDefinition(object customizationObject, string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            this.LoadObjectFromXmlDocument(customizationObject, doc);
        }

        public void LoadObjectFromStream(object customizationObject, Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            this.LoadObjectFromXmlDocument(customizationObject, doc);
        }

        public void LoadObjectFromXmlDefinition(object customizationObject, string xmlContent)
        {
            this.customizationObject = customizationObject;
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);
            XmlElement documentElement = document.DocumentElement;
            if (document.DocumentElement.Attributes["version"] != null)
            {
                documentElement = (XmlElement)document.DocumentElement.ChildNodes[0];
            }
            this.SetUpObject(customizationObject, document.DocumentElement);
        }

        public void LoadObjectFromXmlDocument(object customizationObject, XmlDocument doc)
        {
            this.customizationObject = customizationObject;
            XmlElement documentElement = doc.DocumentElement;
            if (doc.DocumentElement.Attributes["version"] != null)
            {
                documentElement = (XmlElement)doc.DocumentElement.ChildNodes[0];
            }
            this.SetUpObject(customizationObject, documentElement);
            if (customizationObject is Form)
            {
                Form form = (Form)customizationObject;
                if ((this.acceptButtonName != null) && (this.acceptButtonName.Length > 0))
                {
                    form.AcceptButton = (Button)this.controlDictionary[this.acceptButtonName];
                }
                if ((this.cancelButtonName != null) && (this.cancelButtonName.Length > 0))
                {
                    form.CancelButton = (Button)this.controlDictionary[this.cancelButtonName];
                }
            }
            if (this.tooltips.Count > 0)
            {
                ToolTip tip = new ToolTip();
                foreach (DictionaryEntry entry in this.tooltips)
                {
                    tip.SetToolTip((Control)entry.Key, entry.Value.ToString());
                }
            }
        }

        private void SetAttributes(object o, XmlElement el)
        {
            if (el.Name == "AcceptButton")
            {
                this.mainForm = (Form)o;
                this.acceptButtonName = el.Attributes["value"].InnerText.Split(new char[] { ' ' })[0];
            }
            else if (el.Name == "CancelButton")
            {
                this.mainForm = (Form)o;
                this.cancelButtonName = el.Attributes["value"].InnerText.Split(new char[] { ' ' })[0];
            }
            else
            {
                string innerText;
                if (el.Name == "ToolTip")
                {
                    innerText = el.Attributes["value"].InnerText;
                    this.tooltips[o] = (this.stringValueFilter != null) ? this.stringValueFilter.GetFilteredValue(innerText) : innerText;
                }
                else if (el.Attributes["value"] != null)
                {
                    innerText = el.Attributes["value"].InnerText;
                    try
                    {
                        this.SetValue(o, el.Name, (this.stringValueFilter != null) ? this.stringValueFilter.GetFilteredValue(innerText) : innerText);
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (el.Attributes["event"] != null)
                {
                    try
                    {
                        EventInfo info = o.GetType().GetEvent(el.Name);
                        info.AddEventHandler(o, Delegate.CreateDelegate(info.EventHandlerType, this.customizationObject, el.Attributes["event"].InnerText));
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    string name;
                    PropertyInfo property = o.GetType().GetProperty(el.Name);
                    object obj2 = property.GetValue(o, null);
                    if (obj2 is IList)
                    {
                        foreach (XmlNode node in el.ChildNodes)
                        {
                            if (node is XmlElement)
                            {
                                XmlElement element = (XmlElement)node;
                                if (element.Name.IndexOf("System.String") >= 0)
                                {
                                    if (obj2.GetType().ToString() == "System.Windows.Forms.ComboBox+ObjectCollection")
                                        ((ComboBox)o).Items.Add(node.Attributes["value"].Value);
                                    continue;
                                }
                                object currentObject = this.objectCreator.CreateObject(XmlConvert.DecodeName(element.Name), element, o);
                                if (currentObject == null)
                                {
                                    continue;
                                }
                                if (currentObject is IComponent)
                                {
                                    name = null;
                                    if ((element["Name"] != null) && (element["Name"].Attributes["value"] != null))
                                    {
                                        name = element["Name"].Attributes["value"].InnerText;
                                    }
                                    if ((name == null) || (name.Length == 0))
                                    {
                                        name = "CreatedObject" + this.num++;
                                    }
                                }
                                this.SetUpObject(currentObject, element);
                                if (currentObject is Control)
                                {
                                    name = ((Control)currentObject).Name;
                                    if ((name != null) && (name.Length > 0))
                                    {
                                        this.ControlDictionary[name] = (Control)currentObject;
                                    }
                                }
                                if (currentObject != null)
                                {
                                    ((IList)obj2).Add(currentObject);
                                }
                            }
                        }
                    }
                    else
                    {
                        object obj4 = this.objectCreator.CreateObject(o.GetType().GetProperty(el.Name).PropertyType.Name, el, o);
                        if (obj4 is IComponent)
                        {
                            PropertyInfo info3 = obj4.GetType().GetProperty("Name");
                            name = null;
                            if ((el["Name"] != null) && (el["Name"].Attributes["value"] != null))
                            {
                                name = el["Name"].Attributes["value"].InnerText;
                            }
                            if ((name == null) || (name.Length == 0))
                            {
                                name = "CreatedObject" + this.num++;
                            }
                            obj4 = this.objectCreator.CreateObject(name, el, o);
                        }
                        this.SetUpObject(obj4, el);
                        property.SetValue(o, obj4, null);
                    }
                }
            }
        }

        private void SetUpObject(object currentObject, XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement el = (XmlElement)node;
                    this.SetAttributes(currentObject, el);
                }
            }
            if (currentObject is Control)
            {
                ((Control)currentObject).ResumeLayout(false);
            }
        }

        private void SetValue(object o, string propertyName, string val)
        {
            try
            { 
                PropertyInfo property = o.GetType().GetProperty(propertyName);
                if (propertyName == "AcceptButton")
                {
                    this.acceptButton = val.Split(new char[] { ' ' })[0];
                    return;
                }
                if (propertyName == "CancelButton")
                {
                    this.cancelButton = val.Split(new char[] { ' ' })[0];
                    return;
                }
                if (propertyName == "ToolTip")
                {
                    this.tooltips[o] = val;
                    return;
                }
                if (!val.StartsWith("{") || !val.EndsWith("}"))
                {
                    goto Label_0198;
                }
                val = val.Substring(1, val.Length - 2);
                object obj2 = null;
                if (property.CanWrite)
                {
                    obj2 = this.objectCreator.GetType(property.PropertyType.AssemblyQualifiedName).Assembly.CreateInstance(property.PropertyType.FullName);
                }
                else
                {
                    obj2 = property.GetValue(o, null);
                }
                Match match = propertySet.Match(val);
                goto Label_0172;
            Label_013C:
                if (!match.Success)
                {
                    goto Label_0177;
                }
                this.SetValue(obj2, match.Result("${Property}"), match.Result("${Value}"));
                match = match.NextMatch();
            Label_0172: 
                goto Label_013C;
            Label_0177:
                if (property.CanWrite)
                {
                    property.SetValue(o, obj2, null);
                }
                return;
            Label_0198:
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(o, Enum.Parse(property.PropertyType, val), null);
                }
                else if (property.PropertyType == typeof(Color))
                {
                    string name = val.Substring(val.IndexOf('[') + 1).Replace("]", "");
                    string[] strArray = name.Split(new char[] { ',', '=' });
                    if (strArray.Length > 1)
                    {
                        property.SetValue(o, Color.FromArgb(int.Parse(strArray[1]), int.Parse(strArray[3]), int.Parse(strArray[5]), int.Parse(strArray[7])), null);
                    }
                    else
                    {
                        property.SetValue(o, Color.FromName(name), null);
                    }
                }
                else if (val.Length > 0)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
                    property.SetValue(o, converter.ConvertFromInvariantString(val), null);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("error while setting property " + propertyName + " of object " + o.ToString() + " to value '" + val + "'", exception);
            }
        }

        public Dictionary<string, Control> ControlDictionary
        {
            get
            {
                return this.controlDictionary;
            }
        }

        public IObjectCreator ObjectCreator
        {
            get
            {
                return this.objectCreator;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.objectCreator = value;
            }
        }

        public IPropertyValueCreator PropertyValueCreator
        {
            get
            {
                return this.propertyValueCreator;
            }
            set
            {
                this.propertyValueCreator = value;
            }
        }

        public IStringValueFilter StringValueFilter
        {
            get
            {
                return this.stringValueFilter;
            }
            set
            {
                this.stringValueFilter = value;
            }
        }
    }
}

