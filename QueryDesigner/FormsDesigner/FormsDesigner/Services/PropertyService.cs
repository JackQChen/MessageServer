namespace FormsDesigner.Services
{
    using FormsDesigner.Core;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public static class PropertyService
    {
        private static string configDirectory;
        private static string dataDirectory;
        private static Properties properties;
        private static string propertyFileName;
        private static string propertyXmlRootNodeName;

        public static  event PropertyChangedEventHandler PropertyChanged;

        public static string Get(string property)
        {
            return properties[property];
        }

        public static T Get<T>(string property, T defaultValue)
        {
            return properties.Get<T>(property, defaultValue);
        }

        public static void InitializeService(string configDirectory, string dataDirectory, string propertiesName)
        {
            if (properties != null)
            {
                throw new InvalidOperationException("Service is already initialized.");
            }
            if (((configDirectory == null) || (dataDirectory == null)) || (propertiesName == null))
            {
                throw new ArgumentNullException();
            }
            properties = new Properties();
            PropertyService.configDirectory = configDirectory;
            PropertyService.dataDirectory = dataDirectory;
            propertyXmlRootNodeName = propertiesName;
            propertyFileName = propertiesName + ".xml";
            properties.PropertyChanged += new PropertyChangedEventHandler(PropertyService.PropertiesPropertyChanged);
        }

        public static void Load()
        {
            if (properties == null)
            {
                throw new InvalidOperationException("Service is not initialized.");
            }
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
        }

        public static bool LoadPropertiesFromStream(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    using (XmlTextReader reader = new XmlTextReader(fileName))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement() && (reader.LocalName == propertyXmlRootNodeName))
                            {
                                properties.ReadProperties(reader, propertyXmlRootNodeName);
                                return true;
                            }
                        }
                    }
                }
                catch (XmlException exception)
                {
                    MessageService.ShowError("Error loading properties: " + exception.Message + "\nSettings have been restored to default values.");
                }
            }
            return false;
        }

        private static void PropertiesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, e);
            }
        }

        public static void Save()
        {
            using (XmlTextWriter writer = new XmlTextWriter(Path.Combine(configDirectory, propertyFileName), Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartElement(propertyXmlRootNodeName);
                properties.WriteProperties(writer);
                writer.WriteEndElement();
            }
        }

        public static void Set<T>(string property, T value)
        {
            properties.Set<T>(property, value);
        }

        public static string ConfigDirectory
        {
            get
            {
                return configDirectory;
            }
        }

        public static string DataDirectory
        {
            get
            {
                return dataDirectory;
            }
        }

        public static bool Initialized
        {
            get
            {
                return (properties != null);
            }
        }
    }
}

