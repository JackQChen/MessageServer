using System;
using System.Configuration;
using System.Net;
using MessageLib;

namespace AccessService
{
    public class ForwardingService : TcpServer
    {

        public AccessInfo AccessInfo { get { return AccessInfo.instance; } }

        public IPEndPoint ForwardingEndPoint { get; set; }

        public ForwardingService()
        {
        }

        protected override TcpSession CreateSession()
        {
            return new ForwardingSession();
        }

        protected override void OnStarted()
        {
            var forwarding = ConfigurationManager.AppSettings[this.Name + "_Forwarding"].Split(':');
            this.ForwardingEndPoint = new IPEndPoint(IPAddress.Parse(forwarding[0]), Convert.ToInt32(forwarding[1]));
        }

    }
}
