using System;
using System.Diagnostics;
using System.IO;

namespace MessageLib.Logging
{
    internal class LogListener : TraceListener
    {
        private string _fileDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogListener()
        {
            _fileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }

        private string GetFilePath(string category)
        {
            var dir = Path.Combine(_fileDirectory, category);
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
