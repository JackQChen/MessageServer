using System;
using System.Net.Sockets;
using CommonService;

namespace CommonClient
{
    public class CommonClient : MessageLib.TcpClient
    {
        internal Handler handler = new Handler();
        SessionData data = new SessionData();
        public event Action<SocketError> Error;
        public event Action<Message> MessageReceived;
        public event Action<byte[]> RawDataReceived;
        public event Action Disconnected;

        public CommonClient()
        {
            handler.MessageReceived += Handler_MessageReceived;
            handler.RawDataReceived += Handler_RawDataReceived;
        }

        public bool SendMessage(Message message)
        {
            if (this.data.UseRawData)
                return false;
            var bytes = this.handler.FormatterMessageBytes(message);
            this.SendAsync(bytes);
            return true;
        }

        private void Handler_MessageReceived(int id, Message msg)
        {
            if (MessageReceived != null)
                MessageReceived(msg);
        }

        private void Handler_RawDataReceived(int id, byte[] data)
        {
            if (RawDataReceived != null)
                RawDataReceived(data);
        }

        public override bool Connect()
        {
            if (base.Connect())
            {
                this.ReceiveAsync();
                return true;
            }
            else
                return false;
        }

        protected override void OnError(SocketError error)
        {
            if (this.Error != null)
                this.Error(error);
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            handler.ProcessReceive(0, data, buffer, offset, size);
        }

        protected override void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected();
        }

        protected override void Dispose(bool disposingManagedResources)
        {
            data.Dispose();
            handler.MessageReceived -= Handler_MessageReceived;
            base.Dispose(disposingManagedResources);
        }
    }
}
