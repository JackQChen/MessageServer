namespace FormsDesigner.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public class Properties
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        public event FormsDesigner.Core.PropertyChangedEventHandler PropertyChanged;

        public bool Contains(string property)
        {
            return this.properties.ContainsKey(property);
        }

        public object Get(string property)
        {
            if (!this.properties.ContainsKey(property))
            {
                return null;
            }
            return this.properties[property];
        }

        public T Get<T>(string property, T defaultValue)
        {
            TypeConverter converter;
            Exception exception;
            if (!this.properties.ContainsKey(property))
            {
                this.properties.Add(property, defaultValue);
                return defaultValue;
            }
            object obj2 = this.properties[property];
            if ((obj2 is string) && (typeof(T) != typeof(string)))
            {
                converter = TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    obj2 = converter.ConvertFromInvariantString(obj2.ToString());
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    MessageService.ShowWarning("Error loading property '" + property + "': " + exception.Message);
                    obj2 = defaultValue;
                }
                this.properties[property] = obj2;
            }
            else if ((obj2 is ArrayList) && typeof(T).IsArray)
            {
                ArrayList list = (ArrayList) obj2;
                Type elementType = typeof(T).GetElementType();
                Array array = Array.CreateInstance(elementType, list.Count);
                converter = TypeDescriptor.GetConverter(elementType);
                try
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (list[i] != null)
                        {
                            array.SetValue(converter.ConvertFromInvariantString(list[i].ToString()), i);
                        }
                    }
                    obj2 = array;
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    MessageService.ShowWarning("Error loading property '" + property + "': " + exception.Message);
                    obj2 = defaultValue;
                }
                this.properties[property] = obj2;
            }
            else if (!(obj2 is string) && (typeof(T) == typeof(string)))
            {
                converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter.CanConvertTo(typeof(string)))
                {
                    obj2 = converter.ConvertToInvariantString(obj2);
                }
                else
                {
                    obj2 = obj2.ToString();
                }
            }
            try
            {
                return (T) obj2;
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
        }

        public static Properties Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (XmlTextReader reader = new XmlTextReader(fileName))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string localName = reader.LocalName;
                            if ((localName != null) && (localName == "Properties"))
                            {
                                Properties properties = new Properties();
                                properties.ReadProperties(reader, "Properties");
                                return properties;
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected virtual void OnPropertyChanged(FormsDesigner.Core.PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        private ArrayList ReadArray(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                return new ArrayList(0);
            }
            ArrayList list = new ArrayList();
            while (reader.Read())
            {
                XmlNodeType nodeType = reader.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if ((nodeType == XmlNodeType.EndElement) && (reader.LocalName == "Array"))
                    {
                        return list;
                    }
                }
                else
                {
                    list.Add(reader.HasAttributes ? reader.GetAttribute(0) : null);
                }
            }
            return list;
        }

        public static Properties ReadFromAttributes(XmlReader reader)
        {
            Properties properties = new Properties();
            if (reader.HasAttributes)
            {
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    properties[reader.Name] = reader.Value;
                }
                reader.MoveToElement();
            }
            return properties;
        }

        public void ReadProperties(XmlReader reader, string endElement)
        {
            if (!reader.IsEmptyElement)
            {
                while (reader.Read())
                {
                    XmlNodeType nodeType = reader.NodeType;
                    if (nodeType != XmlNodeType.Element)
                    {
                        if ((nodeType == XmlNodeType.EndElement) && (reader.LocalName == endElement))
                        {
                            break;
                        }
                    }
                    else
                    {
                        string localName = reader.LocalName;
                        if (localName == "Properties")
                        {
                            localName = reader.GetAttribute(0);
                            Properties properties = new Properties();
                            properties.ReadProperties(reader, "Properties");
                            this.properties[localName] = properties;
                        }
                        else if (localName == "Array")
                        {
                            localName = reader.GetAttribute(0);
                            this.properties[localName] = this.ReadArray(reader);
                        }
                        else
                        {
                            this.properties[localName] = reader.HasAttributes ? reader.GetAttribute(0) : null;
                        }
                    }
                }
            }
        }

        public bool Remove(string property)
        {
            return this.properties.Remove(property);
        }

        public void Save(string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartElement("Properties");
                this.WriteProperties(writer);
                writer.WriteEndElement();
            }
        }

        public void Set<T>(string property, T value)
        {
            T oldValue = default(T);
            if (!this.properties.ContainsKey(property))
            {
                this.properties.Add(property, value);
            }
            else
            {
                oldValue = this.Get<T>(property, value);
                this.properties[property] = value;
            }
            this.OnPropertyChanged(new FormsDesigner.Core.PropertyChangedEventArgs(this, property, oldValue, value));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[Properties:{");
            foreach (KeyValuePair<string, object> pair in this.properties)
            {
                builder.Append(pair.Key);
                builder.Append("=");
                builder.Append(pair.Value);
                builder.Append(",");
            }
            builder.Append("}]");
            return builder.ToString();
        }

        public void WriteProperties(XmlWriter writer)
        {
            foreach (KeyValuePair<string, object> pair in this.properties)
            {
                object val = pair.Value;
                if (val is Properties)
                {
                    writer.WriteStartElement("Properties");
                    writer.WriteAttributeString("name", pair.Key);
                    ((Properties) val).WriteProperties(writer);
                    writer.WriteEndElement();
                }
                else if ((val is Array) || (val is ArrayList))
                {
                    writer.WriteStartElement("Array");
                    writer.WriteAttributeString("name", pair.Key);
                    foreach (object obj3 in (IEnumerable) val)
                    {
                        writer.WriteStartElement("Element");
                        this.WriteValue(writer, obj3);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement(pair.Key);
                    this.WriteValue(writer, val);
                    writer.WriteEndElement();
                }
            }
        }

        private void WriteValue(XmlWriter writer, object val)
        {
            if (val != null)
            {
                if (val is string)
                {
                    writer.WriteAttributeString("value", val.ToString());
                }
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(val.GetType());
                    writer.WriteAttributeString("value", converter.ConvertToInvariantString(val));
                }
            }
        }

        public int Count
        {
            get
            {
                return this.properties.Count;
            }
        }

        public string[] Elements
        {
            get
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, object> pair in this.properties)
                {
                    list.Add(pair.Key);
                }
                return list.ToArray();
            }
        }

        public string this[string property]
        {
            get
            {
                return Convert.ToString(this.Get(property));
            }
            set
            {
                this.Set<string>(property, value);
            }
        }
    }
}

