using MessageLib;

namespace CommonService
{
    public class Session : TcpSession
    {
        Service service;
        SessionData data;

        public Session()
        {
        }

        protected override void Initialize()
        {
            this.data = new SessionData();
            this.service = this.Server as Service;
            this.service.handler.MessageReceived += Handler_MessageReceived;
            this.service.handler.RawDataReceived += Handler_RawDataReceived;
        }

        private void Handler_MessageReceived(int id, Message message)
        {
            if (id != this.Id)
                return;
            var data = this.service.handler.FormatterMessageBytes(message);
            this.Server.Multicast(data);
        }

        private void Handler_RawDataReceived(int id, byte[] data)
        {
            if (id != this.Id)
                return;
            this.Server.Multicast(data);
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            this.service.handler.ProcessReceive(this.Id, this.data, buffer, offset, size);
        }

        protected override void Dispose(bool disposingManagedResources)
        {
            this.service.handler.MessageReceived -= Handler_MessageReceived;
            this.data.Dispose();
            base.Dispose(disposingManagedResources);
        }

    }
}
