using System;
using System.Text;

namespace MessageLib.Logging
{
    /// <summary>
    /// Logger
    /// </summary>
    public class Logger
    {

        private string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        public Logger(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(object obj)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} - [{1}]{2}{3}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), _name, Environment.NewLine, obj);
            return builder.ToString();
        }

        /// <summary>
        /// Format
        /// </summary>
        private string Format(string format, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} - [{1}]{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), _name, Environment.NewLine);
            if (args[0] is Exception)
                builder.AppendLine(args[0].ToString());
            else
            {
                builder.AppendFormat(format, args);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public void Debug(object message)
        {
            LogWriter.Instance.WriteLog(Format(message), LogLevel.Debug);
        }

        public void DebugFormat(string format, params object[] args)
        {
            LogWriter.Instance.WriteLog(Format(format, args), LogLevel.Debug);
        }

        public void Info(object message)
        {
            LogWriter.Instance.WriteLog(Format(message), LogLevel.Info);
        }

        public void InfoFormat(string format, params object[] args)
        {
            LogWriter.Instance.WriteLog(Format(format, args), LogLevel.Info);
        }

        public void Warn(object message)
        {
            LogWriter.Instance.WriteLog(Format(message), LogLevel.Warn);
        }

        public void WarnFormat(string format, params object[] args)
        {
            LogWriter.Instance.WriteLog(Format(format, args), LogLevel.Warn);
        }

        public void Error(object message)
        {
            LogWriter.Instance.WriteLog(Format(message), LogLevel.Error);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            LogWriter.Instance.WriteLog(Format(format, args), LogLevel.Error);
        }

        public void Fatal(object message)
        {
            LogWriter.Instance.WriteLog(Format(message), LogLevel.Fatal);
        }

        public void FatalFormat(string format, params object[] args)
        {
            LogWriter.Instance.WriteLog(Format(format, args), LogLevel.Fatal);
        }
    }
}
