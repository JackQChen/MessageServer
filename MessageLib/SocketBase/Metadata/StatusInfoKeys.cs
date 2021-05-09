﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageLib.SocketBase.Metadata;

namespace MessageLib.SocketBase.Metadata
{
    /// <summary>
    /// Server StatusInfo Metadata
    /// </summary>
    public class StatusInfoKeys
    {
        #region Shared

        /// <summary>
        /// The cpu usage
        /// </summary>
        public const string CpuUsage = "CpuUsage";

        /// <summary>
        /// The memory usage
        /// </summary>
        public const string MemoryUsage = "MemoryUsage";

        /// <summary>
        /// The total thread count
        /// </summary>
        public const string TotalThreadCount = "TotalThreadCount";

        /// <summary>
        /// The available working threads count
        /// </summary>
        public const string AvailableWorkingThreads = "AvailableWorkingThreads";

        /// <summary>
        /// The available completion port threads count
        /// </summary>
        public const string AvailableCompletionPortThreads = "AvailableCompletionPortThreads";

        /// <summary>
        /// The max working threads count
        /// </summary>
        public const string MaxWorkingThreads = "MaxWorkingThreads";

        /// <summary>
        /// The max completion port threads count
        /// </summary>
        public const string MaxCompletionPortThreads = "MaxCompletionPortThreads";

        #endregion

        #region For server instance

        /// <summary>
        /// The started time.
        /// </summary>
        public const string StartedTime = "StartedTime";


        /// <summary>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </summary>
        public const string IsRunning = "IsRunning";

        /// <summary>
        /// The total count of the connections.
        /// </summary>
        public const string TotalConnections = "TotalConnections";

        /// <summary>
        /// The max connection number.
        /// </summary>
        public const string MaxConnectionNumber = "MaxConnectionNumber";

        /// <summary>
        /// The total handled requests count.
        /// </summary>
        public const string TotalHandledRequests = "TotalHandledRequests";

        /// <summary>
        /// Gets or sets the request handling speed, per second.
        /// </summary>
        /// <value>
        /// The request handling speed.
        /// </value>
        public const string RequestHandlingSpeed = "RequestHandlingSpeed";

        /// <summary>
        /// The total sent count.
        /// </summary>
        public const string TotalSent = "TotalSent";

        /// <summary>
        /// Gets or sets the sending speed, per second.
        /// </summary>
        /// <value>
        /// The sending speed.
        /// </value>
        public const string SendingSpeed = "SendingSpeed";

        /// <summary>
        /// The total received count.
        /// </summary>
        public const string TotalReceived = "TotalReceived";

        /// <summary>
        /// Gets or sets the receiving speed, per second.
        /// </summary>
        /// <value>
        /// The receiving speed.
        /// </value>
        public const string ReceivingSpeed = "ReceivingSpeed";

        /// <summary>
        /// Gets or sets the listeners.
        /// </summary>
        public const string Listeners = "Listeners";

        /// <summary>
        /// The avialable sending queue items.
        /// </summary>
        public const string AvialableSendingQueueItems = "AvialableSendingQueueItems";

        /// <summary>
        /// The total sending queue items.
        /// </summary>
        public const string TotalSendingQueueItems = "TotalSendingQueueItems";

        #endregion

    }
}
