using MessageLib;

namespace AccessService
{
    public class ForwardingSession : TcpSession
    {

        ForwardingService service;
        TcpAgent agent;
        bool isVerified = false;

        public string ClientIP { get; set; }

        public ForwardingSession()
        {
        }

        protected override void Initialize()
        {
            this.service = this.Server as ForwardingService;
            this.agent = new TcpAgent();
            this.agent.Endpoint = this.service.ForwardingEndPoint;
            this.agent.Received += Agent_Received;
            this.agent.Disconnected += Agent_Disconnected;
        }

        protected override void OnConnecting()
        {
            this.ClientIP = this.Socket.RemoteEndPoint.ToString();
            this.isVerified = this.service.AccessInfo.IsVerified(this.ClientIP);
            if (!isVerified)
                return;
            if (this.agent.Connect())
                this.agent.ReceiveAsync();
            else
                this.isVerified = false;
        }

        protected override void OnConnected()
        {
            if (!isVerified)
            {
                this.Disconnect();
                this.Logger.InfoFormat("[{0}]连接未授权:IP={1}", this.service.Name, this.ClientIP);
            }
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            this.agent.Send(buffer, offset, size);
        }

        protected override void OnDisconnected()
        {
            this.agent.Disconnect();
        }

        private void Agent_Received(byte[] buffer, int offset, int size)
        {
            this.Send(buffer, offset, size);
        }

        private void Agent_Disconnected()
        {
            this.Disconnect();
        }

    }
}
