namespace MessageLib.SocketBase.Logging
{
    /// <summary>
    /// Log interface
    /// </summary>
    public interface ILog
    {

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(object message);

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(object message);

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(object message);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(object message);

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(object message);

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void FatalFormat(string format, params object[] args);
    }
}
