using MessageLib;

namespace ForwardingService
{
    public class Session : TcpSession
    {
        Service service;
        TcpAgent agent;

        public Session()
        {
        }

        protected override void Initialize()
        {
            this.service = this.Server as Service;
            this.agent = new TcpAgent();
            this.agent.Endpoint = this.service.ForwardingEndPoint;
            this.agent.Received += Agent_Received;
            this.agent.Disconnected += Agent_Disconnected;
        }

        protected override void OnConnecting()
        {
            if (this.agent.Connect())
                this.agent.ReceiveAsync();
            else
                this.Disconnect();
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
