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
        internal static ServiceLogWriter Instance = new ServiceLogWriter();

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
