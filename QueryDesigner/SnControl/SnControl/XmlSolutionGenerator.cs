namespace SnControl
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Xml;

    public class XmlSolutionGenerator
    {
        public XmlElement GetElementFor(XmlDocument doc, Solution solution)
        {
            XmlElement element = doc.CreateElement("Solution");
            XmlAttribute node = doc.CreateAttribute("version");
            node.InnerText = "1.0";
            element.Attributes.Append(node);
            XmlElement newChild = doc.CreateElement(solution.GetType().FullName);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(solution);
            ArrayList list = new ArrayList();
            foreach (PropertyDescriptor descriptor in properties)
            {
                XmlElement element3 = doc.CreateElement(descriptor.Name);
                object obj2 = null;
                try
                {
                    obj2 = descriptor.GetValue(solution);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    continue;
                }
                if (obj2 is IList)
                {
                    foreach (object obj3 in (IList)obj2)
                    {
                        XmlElement elementFor = this.GetElementFor(doc, obj3);
                        if (elementFor != null)
                        {
                            element3.AppendChild(elementFor);
                        }
                    }
                }
                if (descriptor.ShouldSerializeValue(solution) && descriptor.IsBrowsable)
                {
                    XmlAttribute attribute2 = doc.CreateAttribute("value");
                    attribute2.InnerText = (obj2 == null) ? null : obj2.ToString();
                    element3.Attributes.Append(attribute2);
                    list.Insert(0, element3);
                }
            }
            foreach (XmlElement element3 in list)
            {
                newChild.AppendChild(element3);
            }
            element.AppendChild(newChild);
            return element;
        }

        public XmlElement GetElementFor(XmlDocument doc, object o)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            try
            {
                XmlAttribute attribute;
                XmlElement element = doc.CreateElement(o.GetType().FullName);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(o);
                ArrayList list = new ArrayList();
                foreach (PropertyDescriptor descriptor in properties)
                {
                    XmlElement element2 = null;
                    element2 = doc.CreateElement(descriptor.Name);
                    object obj2 = null;
                    try
                    {
                        obj2 = descriptor.GetValue(o);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Console.WriteLine(exception);
                        continue;
                    }
                    if (obj2 is IList)
                    {
                        foreach (object obj3 in (IList)obj2)
                        {
                            XmlElement elementFor = this.GetElementFor(doc, obj3);
                            if (elementFor != null)
                            {
                                element2.AppendChild(elementFor);
                            }
                        }
                        if (element2.ChildNodes.Count > 0)
                        {
                            list.Add(element2);
                        }
                    }
                    else if (descriptor.ShouldSerializeValue(o) && descriptor.IsBrowsable)
                    {
                        attribute = doc.CreateAttribute("value");
                        attribute.InnerText = (obj2 == null) ? null : obj2.ToString();
                        element2.Attributes.Append(attribute);
                        list.Insert(0, element2);
                    }
                }
                foreach (XmlElement element2 in list)
                {
                    element.AppendChild(element2);
                }
                if (element.ChildNodes.Count == 0)
                {
                    attribute = doc.CreateAttribute("value");
                    attribute.InnerText = o.ToString();
                    element.Attributes.Append(attribute);
                }
                return element;
            }
            catch (Exception exception2)
            {
                Console.WriteLine(exception2.ToString());
            }
            return null;
        }
    }
}

