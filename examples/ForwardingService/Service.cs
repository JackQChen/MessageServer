using System;
using System.Configuration;
using System.Net;
using MessageLib;

namespace ForwardingService
{
    public class Service : TcpServer
    {
        public IPEndPoint ForwardingEndPoint { get; set; }

        public Service()
        {
        }

        protected override TcpSession CreateSession()
        {
            return new Session();
        }

        protected override void OnStarted()
        {
            var forwarding = ConfigurationManager.AppSettings[this.Name + "_Forwarding"].Split(':');
            this.ForwardingEndPoint = new IPEndPoint(IPAddress.Parse(forwarding[0]), Convert.ToInt32(forwarding[1]));
        }

    }
}
