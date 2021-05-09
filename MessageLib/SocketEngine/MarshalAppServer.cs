﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Provider;
using MessageLib.SocketBase.Config;
using System.Reflection;
using MessageLib.SocketBase.Metadata;

namespace MessageLib.SocketEngine
{
    class MarshalAppServer : MarshalByRefObject, IWorkItem, IStatusInfoSource
    {
        private IWorkItem m_AppServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDomainAppServer"/> class.
        /// </summary>
        /// <param name="serviceTypeName">Name of the service type.</param>
        public MarshalAppServer(string serviceTypeName)
        {
            var serviceType = Type.GetType(serviceTypeName);
            m_AppServer = (IWorkItem)Activator.CreateInstance(serviceType);
        }

        /// <summary>
        /// Gets the name of the server instance.
        /// </summary>
        public string Name
        {
            get { return m_AppServer.Name; }
        }

        /// <summary>
        /// Setups the specified root config.
        /// </summary>
        /// <param name="bootstrap">The bootstrap.</param>
        /// <param name="config">The socket server instance config.</param>
        /// <param name="factories">The providers.</param>
        /// <returns></returns>
        public bool Setup(IBootstrap bootstrap, IServerConfig config, ProviderFactoryInfo[] factories)
        {
            return m_AppServer.Setup(bootstrap, config, factories);
        }

        /// <summary>
        /// Gets the server's config.
        /// </summary>
        /// <value>
        /// The server's config.
        /// </value>
        public IServerConfig Config
        {
            get
            {
                if (m_AppServer == null)
                    return null;

                return m_AppServer.Config;
            }
        }

        /// <summary>
        /// Reports the potential configuration change.
        /// </summary>
        /// <param name="config">The server config which may be changed.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ReportPotentialConfigChange(IServerConfig config)
        {
            m_AppServer.ReportPotentialConfigChange(config);
        }

        /// <summary>
        /// Starts this server instance.
        /// </summary>
        /// <returns>
        /// return true if start successfull, else false
        /// </returns>
        public bool Start()
        {
            return m_AppServer.Start();
        }

        /// <summary>
        /// Stops this server instance.
        /// </summary>
        public void Stop()
        {
            m_AppServer.Stop();
        }

        /// <summary>
        /// Gets the current state of the work item.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public ServerState State
        {
            get { return m_AppServer.State; }
        }

        /// <summary>
        /// Gets the total session count.
        /// </summary>
        public int SessionCount
        {
            get { return m_AppServer.SessionCount; }
        }

        StatusInfoAttribute[] IStatusInfoSource.GetServerStatusMetadata()
        {
            return m_AppServer.GetServerStatusMetadata();
        }

        StatusInfoCollection IStatusInfoSource.CollectServerStatus(StatusInfoCollection nodeStatus)
        {
            return m_AppServer.CollectServerStatus(nodeStatus);
        }

        void IStatusInfoSource.UpdateServerStatus(string name, object value)
        {
            m_AppServer.UpdateServerStatus(name, value);
        }

        public void TransferSystemMessage(string messageType, object messageData)
        {
            m_AppServer.TransferSystemMessage(messageType, messageData);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        ///   </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
