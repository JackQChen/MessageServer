using System;
using System.IO;
using MessageLib;

namespace WebSocketService
{
    public class Service : WsServer
    {

        public Service()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shared");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            this.AddStaticContent(dir);
        }

        protected override TcpSession CreateSession()
        {
            return new Session();
        }

    }
}
