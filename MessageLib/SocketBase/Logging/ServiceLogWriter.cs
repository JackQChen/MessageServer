using System.Diagnostics;

namespace MessageLib.SocketBase.Logging
{
    /// <summary>
    /// ServiceLogWriter
    /// </summary>
    internal class ServiceLogWriter
    {
        /// <summary>
        /// Single instance
        /// </summary>
        private static ServiceLogWriter instance;

        internal static ServiceLogWriter Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceLogWriter();
                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ServiceLogWriter()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ServiceLogListener());
        }

        internal void WriteLog(string log, string logLevel)
        {
            Trace.WriteLine(log, logLevel);
        }

    }
}
