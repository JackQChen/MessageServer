using System;

namespace MessageLib.SocketBase.Logging
{
    /// <summary>
    /// Console Log
    /// </summary>
    public class ConsoleLog : ILog
    {
        private string m_Name;

        private const string m_MessageTemplate = "{0}-{1}: {2}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLog"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ConsoleLog(string name)
        {
            m_Name = name;
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(string format, params object[] args)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Debug, string.Format(format, args));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Info, message);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(string format, params object[] args)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Info, string.Format(format, args));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(string format, params object[] args)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Warn, string.Format(format, args));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Error, message);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(string format, params object[] args)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Error, string.Format(format, args));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Fatal, message);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(string format, params object[] args)
        {
            Console.WriteLine(m_MessageTemplate, m_Name, LogLevel.Fatal, string.Format(format, args));
        }

    }
}
