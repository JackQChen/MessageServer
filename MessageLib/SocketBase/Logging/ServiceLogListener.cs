using System;
using System.Diagnostics;
using System.IO;

namespace MessageLib.SocketBase.Logging
{
    public class ServiceLogListener : TraceListener
    {
        private string m_fileDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceLogListener()
        {
            this.m_fileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }

        private string GetFilePath(string category)
        {
            var dir = Path.Combine(m_fileDirectory, category);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Path.Combine(dir, string.Format("{0}_{1}.log", category, DateTime.Now.ToString("yyyyMMdd")));
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public override void WriteLine(string message, string category)
        {
            File.AppendAllText(GetFilePath(category), message);
        }
    }
}
