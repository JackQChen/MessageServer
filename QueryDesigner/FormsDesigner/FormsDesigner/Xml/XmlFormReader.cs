namespace FormsDesigner.Xml
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    public class XmlFormReader
    {
        private string acceptButton = null;
        private string cancelButton = null;
        private static Regex fontRegex = new Regex(@"Name=(.+)\,\s+Size=(\d+)\,.+\s+Style=(.+)\]");
        private IDesignerHost host;
        private static readonly Regex propertySet = new Regex(@"(?<Property>[\w]+)\s*=\s*(?<Value>[\w\d]+)", RegexOptions.Compiled);
        private Hashtable tooltips = new Hashtable();

        public XmlFormReader(IDesignerHost host)
        {
            this.host = host;
        }

        public object CreateObject(string xmlContent)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);
            return this.CreateObject(document.DocumentElement, true);
        }

        private object CreateObject(XmlElement objectElement, bool suspend)
        {
            System.Type conversionType = this.host.GetType(objectElement.Attributes["type"].Value);
            if (objectElement.Attributes["value"] != null)
            {
                try
                {
                    return Convert.ChangeType(objectElement.Attributes["value"].InnerText, conversionType);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            if (conversionType == null)
            {
                return null;
            }
            object currentObject = conversionType.Assembly.CreateInstance(conversionType.FullName);
            string name = null;
            if ((objectElement["Name"] != null) && (objectElement["Name"].Attributes["value"] != null))
            {
                name = objectElement["Name"].Attributes["value"].InnerText;
            }
            if (currentObject is IComponent)
            {
                currentObject = this.host.CreateComponent(currentObject.GetType(), name);
            }
            bool flag = false;
            if (suspend && (currentObject is ContainerControl))
            {
                flag = true;
                ((Control) currentObject).SuspendLayout();
            }
            this.SetUpObject(currentObject, objectElement);
            if ((this.acceptButton != null) && (currentObject is Form))
            {
                ((Form) currentObject).AcceptButton = (IButtonControl) this.host.Container.Components[this.acceptButton];
                this.acceptButton = null;
            }
            if ((this.cancelButton != null) && (currentObject is Form))
            {
                ((Form) currentObject).CancelButton = (IButtonControl) this.host.Container.Components[this.cancelButton];
                this.cancelButton = null;
            }
            if (flag)
            {
                ((Control) currentObject).ResumeLayout(false);
            }
            return currentObject;
        }

        private void SetAttributes(object o, XmlElement el)
        {
            if (el.Attributes["value"] != null)
            {
                string innerText = el.Attributes["value"].InnerText;
                try
                {
                    this.SetValue(o, el.Name, innerText);
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    Console.WriteLine(exception);
                }
            }
            else
            {
                object obj2 = o.GetType().GetProperty(el.Name).GetValue(o, null);
                if (obj2 is IList)
                {
                    foreach (XmlNode node in el.ChildNodes)
                    {
                        if (node is XmlElement)
                        {
                            XmlElement objectElement = (XmlElement) node;
                            object obj3 = this.CreateObject(objectElement, false);
                            if (obj3 != null)
                            {
                                try
                                {
                                    ((IList) obj2).Add(obj3);
                                }
                                catch (Exception exception2)
                                {
                                    Console.WriteLine("Exception while adding to a collection:" + exception2.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetUpDesignerHost(string xmlContent)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);
            if (document.DocumentElement.Attributes["version"] == null)
            {
                this.CreateObject(xmlContent);
            }
            else
            {
                foreach (XmlElement element in document.DocumentElement.ChildNodes)
                {
                    this.CreateObject(element, true);
                }
            }
            foreach (object obj2 in this.host.Container.Components)
            {
                if (obj2 is ToolTip)
                {
                    ToolTip tip = (ToolTip) obj2;
                    foreach (DictionaryEntry entry in this.tooltips)
                    {
                        tip.SetToolTip((Control) entry.Key, entry.Value.ToString());
                    }
                    break;
                }
            }
        }

        private void SetUpObject(object currentObject, XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node is XmlElement)
                {
                    XmlElement el = (XmlElement) node;
                    this.SetAttributes(currentObject, el);
                }
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
                    obj2 = this.host.GetType(property.PropertyType.FullName).Assembly.CreateInstance(property.PropertyType.FullName);
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
                else if (property.PropertyType == typeof(Font))
                {
                    Match match2 = fontRegex.Match(val);
                    if (match2.Success)
                    {
                        FontStyle style = (FontStyle) Enum.Parse(typeof(FontStyle), match2.Groups[3].ToString());
                        property.SetValue(o, new Font(match2.Groups[1].Value, (float) int.Parse(match2.Groups[2].Value), style), null);
                    }
                    else
                    {
                        property.SetValue(o, SystemInformation.MenuFont, null);
                    }
                }
                else if (property.PropertyType == typeof(Cursor))
                {
                    string[] strArray2 = val.Split(new char[] { '[', ']', ' ', ':' });
                    PropertyInfo info2 = typeof(Cursors).GetProperty(strArray2[3]);
                    if (info2 != null)
                    {
                        property.SetValue(o, info2.GetValue(null, null), null);
                    }
                }
                else if (val.Length > 0)
                {
                    property.SetValue(o, Convert.ChangeType(val, property.PropertyType), null);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new ApplicationException("error while setting property " + propertyName + " of object " + o.ToString() + " to value '" + val + "'");
            }
        }
    }
}

