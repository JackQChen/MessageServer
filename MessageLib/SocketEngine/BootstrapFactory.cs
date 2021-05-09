using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase;
using MessageLib.SocketEngine.Configuration;
using MessageLib.SocketBase.Config;
using System.Configuration;

namespace MessageLib.SocketEngine
{
    /// <summary>
    /// Bootstrap Factory
    /// </summary>
    public static class BootstrapFactory
    {
        /// <summary>
        /// Creates the bootstrap.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap(IConfigurationSource config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            IBootstrap bootstrap;

            if (config.Isolation == IsolationMode.AppDomain)
                bootstrap = new AppDomainBootstrap(config);
            else if (config.Isolation == IsolationMode.Process)
                bootstrap = new ProcessBootstrap(config);
            else
                bootstrap = new DefaultBootstrap(config);

            var section = config as ConfigurationSection;

            if (section != null)
                ConfigurationWatcher.Watch(section, bootstrap);

            return bootstrap;
        }

        /// <summary>
        /// Creates the bootstrap from app configuration's socketServer section.
        /// </summary>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap()
        {
            var configSection = ConfigurationManager.GetSection("configSocket");

            if (configSection == null)//to keep compatible with old version
                configSection = ConfigurationManager.GetSection("socketServer");

            if(configSection == null)
                throw new ConfigurationErrorsException("Missing 'configSocket' or 'socketServer' configuration section.");

            var configSource = configSection as IConfigurationSource;
            if(configSource == null)
                throw new ConfigurationErrorsException("Invalid 'configSocket' or 'socketServer' configuration section.");

            return CreateBootstrap(configSource);
        }

        /// <summary>
        /// Creates the bootstrap.
        /// </summary>
        /// <param name="configSectionName">Name of the config section.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap(string configSectionName)
        {
            var configSource = ConfigurationManager.GetSection(configSectionName) as IConfigurationSource;

            if (configSource == null)
                throw new ArgumentException("Invalid section name.");

            return CreateBootstrap(configSource);
        }

        /// <summary>
        /// Creates the bootstrap from configuration file.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrapFromConfigFile(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;

            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var configSection = config.GetSection("configSocket");

            if (configSection == null)
                configSection = config.GetSection("socketServer");

            return CreateBootstrap(configSection as IConfigurationSource);
        }
    }
}
