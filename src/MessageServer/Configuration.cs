﻿using System;
using System.Configuration;

namespace MessageServer
{
    public class ServiceConfig : ConfigurationSection
    {
        private static readonly ConfigurationProperty s_property = new ConfigurationProperty(string.Empty, typeof(ConfigCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ConfigCollection Configs
        {
            get
            {
                return (ConfigCollection)base[s_property];
            }
        }
    }

    [ConfigurationCollection(typeof(Config))]
    public class ConfigCollection : ConfigurationElementCollection
    {
        new public Config this[string name]
        {
            get
            {
                return (Config)base.BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Config();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Config)element).Name;
        }
    }

    public class Config : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return this["Name"].ToString(); }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Type", IsRequired = true)]
        public string Type
        {
            get { return this["Type"].ToString(); }
            set { this["Type"] = value; }
        }

        [ConfigurationProperty("Port", IsRequired = true)]
        public int Port
        {
            get { return Convert.ToInt32(this["Port"]); }
            set { this["Port"] = value; }
        }

        [ConfigurationProperty("Backlog", DefaultValue = 100)]
        public int Backlog
        {
            get { return Convert.ToInt32(this["Backlog"]); }
            set { this["Backlog"] = value; }
        }

        [ConfigurationProperty("MaxConnectionCount", DefaultValue = 100)]
        public int MaxConnectionCount
        {
            get { return Convert.ToInt32(this["MaxConnectionCount"]); }
            set { this["MaxConnectionCount"] = value; }
        }
    }
}
