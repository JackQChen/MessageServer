using System;
using System.IO;
using MessageLib;

namespace TelnetService
{
    public class Service : TcpServer
    {
        internal string logDir;

        public Service()
        {
            this.logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }

        protected override TcpSession CreateSession()
        {
            return new Session();
        }

    }
}
