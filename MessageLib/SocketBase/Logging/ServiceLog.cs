using System;
using System.Text;

namespace MessageLib.SocketBase.Logging
{
    /// <summary>
    /// Service Log
    /// </summary>
    public class ServiceLog : ILog
    {

        private string m_Name;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceLog(string name)
        {
            m_Name = name;
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(object obj)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} - [{1}]{2}{3}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), m_Name, Environment.NewLine, obj);
            return builder.ToString();
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(string format, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} - [{1}]{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), m_Name, Environment.NewLine);
            if (args[0] is Exception)
            {
                builder.AppendLine(args[0].ToString());
            }
            else
            {
                builder.AppendFormat(format, args);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public void Debug(object message)
        {
            ServiceLogWriter.Instance.WriteLog(Format(message), LogLevel.Debug);
        }

        public void DebugFormat(string format, params object[] args)
        {
            ServiceLogWriter.Instance.WriteLog(Format(format, args), LogLevel.Debug);
        }

        public void Info(object message)
        {
            ServiceLogWriter.Instance.WriteLog(Format(message), LogLevel.Info);
        }

        public void InfoFormat(string format, params object[] args)
        {
            ServiceLogWriter.Instance.WriteLog(Format(format, args), LogLevel.Info);
        }

        public void Warn(object message)
        {
            ServiceLogWriter.Instance.WriteLog(Format(message), LogLevel.Warn);
        }

        public void WarnFormat(string format, params object[] args)
        {
            ServiceLogWriter.Instance.WriteLog(Format(format, args), LogLevel.Warn);
        }

        public void Error(object message)
        {
            ServiceLogWriter.Instance.WriteLog(Format(message), LogLevel.Error);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            ServiceLogWriter.Instance.WriteLog(Format(format, args), LogLevel.Error);
        }

        public void Fatal(object message)
        {
            ServiceLogWriter.Instance.WriteLog(Format(message), LogLevel.Fatal);
        }

        public void FatalFormat(string format, params object[] args)
        {
            ServiceLogWriter.Instance.WriteLog(Format(format, args), LogLevel.Fatal);
        }
    }
}
