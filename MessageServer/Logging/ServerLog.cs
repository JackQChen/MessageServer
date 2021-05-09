using MessageLib.SocketBase.Logging;

namespace MessageServer.Logging
{
    /// <summary>
    /// Server log
    /// </summary>
    public class ServerLog : ILog
    {

        private ServerLogHandler log;
        private ServiceLog serviceLog;
        private string m_Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLog"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="logHandler">The logHandler.</param>
        public ServerLog(string name, ServerLogHandler logHandler)
        {
            m_Name = name;
            log = logHandler;
            serviceLog = new ServiceLog(m_Name);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {
            serviceLog.Debug(message);
            log(m_Name, LogLevel.Debug, message.ToString());
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(string format, params object[] args)
        {
            serviceLog.DebugFormat(format, args);
            log(m_Name, LogLevel.Debug, string.Format(format, args));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            serviceLog.Info(message);
            log(m_Name, LogLevel.Info, message.ToString());
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(string format, params object[] args)
        {
            serviceLog.InfoFormat(format, args);
            log(m_Name, LogLevel.Info, string.Format(format, args));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            serviceLog.Warn(message);
            log(m_Name, LogLevel.Warn, message.ToString());
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(string format, params object[] args)
        {
            serviceLog.WarnFormat(format, args);
            log(m_Name, LogLevel.Warn, string.Format(format, args));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            serviceLog.Error(message);
            log(m_Name, LogLevel.Error, message.ToString());
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(string format, params object[] args)
        {
            serviceLog.ErrorFormat(format, args);
            log(m_Name, LogLevel.Error, string.Format(format, args));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            serviceLog.Fatal(message);
            log(m_Name, LogLevel.Fatal, message.ToString());
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(string format, params object[] args)
        {
            serviceLog.FatalFormat(format, args);
            log(m_Name, LogLevel.Fatal, string.Format(format, args));
        }
    }
}
