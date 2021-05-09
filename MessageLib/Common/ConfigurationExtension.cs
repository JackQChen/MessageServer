﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.IO;

namespace MessageLib.Common
{
    /// <summary>
    /// Configuration extension class
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Gets the value from namevalue collection by key.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection collection, string key)
        {
            return GetValue(collection, key, string.Empty);
        }

        /// <summary>
        /// Gets the value from namevalue collection by key.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetValue(this NameValueCollection collection, string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (collection == null)
                return defaultValue;

            var e = collection[key];

            if (e == null)
                return defaultValue;

            return e;
        }

        /// <summary>
        /// Deserializes the specified configuration section.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="section">The section.</param>
        /// <param name="reader">The reader.</param>
        public static void Deserialize<TElement>(this TElement section, XmlReader reader)
            where TElement : ConfigurationElement
        {
            if (section is ConfigurationElementCollection)
            {
                var collectionType = section.GetType();
                var att = collectionType.GetCustomAttributes(typeof(ConfigurationCollectionAttribute), true).FirstOrDefault() as ConfigurationCollectionAttribute;

                if (att != null)
                {
                    var property = collectionType.GetProperty("AddElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.AddItemName, null);

                    property = collectionType.GetProperty("RemoveElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.RemoveItemName, null);

                    property = collectionType.GetProperty("ClearElementName", BindingFlags.NonPublic | BindingFlags.Instance);
                    property.SetValue(section, att.ClearItemsName, null);
                }
            }

            var deserializeElementMethod = typeof(TElement).GetMethod("DeserializeElement", BindingFlags.NonPublic | BindingFlags.Instance);
            deserializeElementMethod.Invoke(section, new object[] { reader, false });
        }

        /// <summary>
        /// Deserializes the child configuration.
        /// </summary>
        /// <typeparam name="TConfig">The type of the configuration.</typeparam>
        /// <param name="childConfig">The child configuration string.</param>
        /// <returns></returns>
        public static TConfig DeserializeChildConfig<TConfig>(string childConfig)
            where TConfig : ConfigurationElement, new()
        {
            // removed extra namespace prefix
            childConfig = childConfig.Replace("xmlns=\"http://schema.socket.net/socket\"", string.Empty);

            XmlReader reader = new XmlTextReader(new StringReader(childConfig));

            var config = new TConfig();

            reader.Read();
            config.Deserialize(reader);

            return config;
        }

        /// <summary>
        /// Gets the child config.
        /// </summary>
        /// <typeparam name="TConfig">The type of the config.</typeparam>
        /// <param name="childElements">The child elements.</param>
        /// <param name="childConfigName">Name of the child config.</param>
        /// <returns></returns>
        public static TConfig GetChildConfig<TConfig>(this NameValueCollection childElements, string childConfigName)
            where TConfig : ConfigurationElement, new()
        {
            var childConfig = childElements.GetValue(childConfigName, string.Empty);

            if (string.IsNullOrEmpty(childConfig))
                return default(TConfig);

            return DeserializeChildConfig<TConfig>(childConfig);
        }

        /// <summary>
        /// Gets the config source path.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static string GetConfigSource(this ConfigurationElement config)
        {
            var source = config.ElementInformation.Source;

            if (!string.IsNullOrEmpty(source) || !Platform.IsMono)
                return source;

            var configProperty = typeof(ConfigurationElement).GetProperty("Configuration", BindingFlags.Instance | BindingFlags.NonPublic);

            if (configProperty == null)
                return string.Empty;

            var configuration = (Configuration)configProperty.GetValue(config, null);
            return configuration.FilePath;
        }

        /// <summary>
        /// Loads configuration element's node information from a model.
        /// </summary>
        /// <param name="configElement">The config element.</param>
        /// <param name="source">The source.</param>
        /// <exception cref="System.Exception">Cannot find expected property 'Item' from the type 'ConfigurationElement'.</exception>
        public static void LoadFrom(this ConfigurationElement configElement, object source)
        {
            // get index property setter
            var indexPropertySetter = configElement.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty)
                    .FirstOrDefault(p =>
                    {
                        if (!p.Name.Equals("Item"))
                            return false;

                        var parameters = p.GetIndexParameters();

                        if (parameters == null || parameters.Length != 1)
                            return false;

                        return parameters[0].ParameterType == typeof(string);
                    });

            if (indexPropertySetter == null)
                throw new Exception("Cannot find expected property 'Item' from the type 'ConfigurationElement'.");

            // source properties
            var properties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var targetProperties = configElement.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                    .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            var emptyObjectArr = new object[0];

            var writableAttrs = new List<KeyValuePair<PropertyInfo, PropertyInfo>>();

            foreach (var sourceProperty in properties)
            {
                if (!sourceProperty.PropertyType.IsSerializable)
                    continue;

                PropertyInfo targetProperty;

                if (targetProperties.TryGetValue(sourceProperty.Name, out targetProperty))
                {
                    if (targetProperty.CanWrite)
                    {
                        writableAttrs.Add(new KeyValuePair<PropertyInfo, PropertyInfo>(sourceProperty, targetProperty));
                        continue;
                    }
                }

                var value = sourceProperty.GetValue(source, emptyObjectArr);

                // lower the first char
                var nameChars = sourceProperty.Name.ToArray();
                nameChars[0] = char.ToLower(nameChars[0]);
                var propertyName = new string(nameChars);

                indexPropertySetter.SetValue(configElement, value, new object[] { propertyName });
            }

            foreach (var pair in writableAttrs)
            {
                var value = pair.Key.GetValue(source, emptyObjectArr);
                pair.Value.SetValue(configElement, value, emptyObjectArr);
            }
        }

        /// <summary>
        /// Gets the current configuration of the configuration element.
        /// </summary>
        /// <returns>The current configuration.</returns>
        /// <param name="configElement">Configuration element.</param>
        public static Configuration GetCurrentConfiguration(this ConfigurationElement configElement)
        {
            var configElementType = typeof(ConfigurationElement);

            var configProperty = configElementType.GetProperty("CurrentConfiguration", BindingFlags.Instance | BindingFlags.Public);

            if(configProperty == null)
                configProperty = configElementType.GetProperty("Configuration", BindingFlags.Instance | BindingFlags.NonPublic);

            if (configProperty == null)
                return null;

            return (Configuration)configProperty.GetValue(configElement, null);
        }

        private static void ResetConfigurationForMono(AppDomain appDomain, string configFilePath)
        {
            appDomain.SetupInformation.ConfigurationFile = configFilePath;

            var configSystem = typeof(ConfigurationManager)
                .GetField("configSystem", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null);

            // clear previous state
            typeof(ConfigurationManager)
                .Assembly.GetTypes()
                .Where(x => x.FullName == "System.Configuration.ClientConfigurationSystem")
                .First()
                .GetField("cfg", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(configSystem, null);
        }

        private static void ResetConfigurationForDotNet(AppDomain appDomain, string configFilePath)
        {
            appDomain.SetData("APP_CONFIG_FILE", configFilePath);

            // clear previous state
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;

            typeof(ConfigurationManager)
                .GetField("s_initState", flags)
                .SetValue(null, 0);

            typeof(ConfigurationManager)
                .GetField("s_configSystem", flags)
                .SetValue(null, null);

            typeof(ConfigurationManager)
                .Assembly.GetTypes()
                .Where(x => x.FullName == "System.Configuration.ClientConfigPaths")
                .First()
                .GetField("s_current", flags)
                .SetValue(null, null);
        }

        /// <summary>
        /// Reset application's configuration to a another config file
        /// </summary>
        /// <param name="appDomain">the assosiated AppDomain</param>
        /// <param name="configFilePath">the config file path want to reset to</param>
        public static void ResetConfiguration(this AppDomain appDomain, string configFilePath)
        {
            if (Platform.IsMono)
                ResetConfigurationForMono(appDomain, configFilePath);
            else
                ResetConfigurationForDotNet(appDomain, configFilePath);
        }

    }
}
