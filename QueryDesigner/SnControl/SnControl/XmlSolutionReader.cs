namespace SnControl
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Xml;

    public class XmlSolutionReader
    {
        private Solution solution;

        public XmlSolutionReader(Solution solution)
        {
            this.solution = solution;
        }

        public object CreateObject(string xmlContent)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);
            return this.CreateObject(document.DocumentElement, true);
        }

        private object CreateObject(XmlElement objectElement, bool suspend)
        {
            Type conversionType = Type.GetType(objectElement.Name);
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
            string innerText = null;
            if ((objectElement["Name"] != null) && (objectElement["Name"].Attributes["value"] != null))
            {
                innerText = objectElement["Name"].Attributes["value"].InnerText;
            }
            this.SetUpObject(currentObject, objectElement);
            return currentObject;
        }

        private void SetAttributes(object o, XmlElement el)
        {
            object obj2 = o.GetType().GetProperty(el.Name).GetValue(o, null);
            if ((el.Attributes["value"] != null) && !(obj2 is IList))
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
            else if (obj2 is IList)
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

        public Solution SetUpSolution(string xmlContent)
        {
            new XmlDocument().LoadXml(xmlContent);
            this.solution = this.CreateObject(xmlContent) as Solution;
            return this.solution;
        }

        private void SetValue(object o, string propertyName, string val)
        {
            try
            {
                PropertyInfo property = o.GetType().GetProperty(propertyName);
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(o, Enum.Parse(property.PropertyType, val), null);
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

