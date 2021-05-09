using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Config;
using MessageLib.SocketBase.Logging;
using MessageLib.SocketBase.Provider;
using MessageLib.SocketBase.Metadata;

namespace MessageLib.SocketEngine
{
    class DefaultBootstrapProcessWrap : DefaultBootstrapAppDomainWrap
    {
        public DefaultBootstrapProcessWrap(IBootstrap bootstrap, IConfigurationSource config, string startupConfigFile)
            : base(bootstrap, config, startupConfigFile)
        {

        }

        protected override IWorkItem CreateWorkItemInstance(string serviceTypeName, StatusInfoAttribute[] serverStatusMetadata)
        {
            return new ProcessAppServer(serviceTypeName, serverStatusMetadata);
        }
    }

    class ProcessBootstrap : AppDomainBootstrap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessBootstrap" /> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ProcessBootstrap(IConfigurationSource config)
            : base(config)
        {
            var clientChannel = ChannelServices.RegisteredChannels.FirstOrDefault(c => c is IpcClientChannel);

            if(clientChannel == null)
            {
                // Create the channel.
                clientChannel = new IpcClientChannel();
                // Register the channel.
                ChannelServices.RegisterChannel(clientChannel, false);
            }
        }

        protected override IBootstrap CreateBootstrapWrap(IBootstrap bootstrap, IConfigurationSource config, string startupConfigFile)
        {
            return new DefaultBootstrapProcessWrap(bootstrap, config, startupConfigFile);
        }
    }
}
