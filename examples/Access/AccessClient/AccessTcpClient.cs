using System;
using MessageLib;

namespace AccessClient
{
    public class AccessTcpClient : TcpClient
    {
        public event Action Disconnected;
        public event Action<byte[], int, int> Received;

        public AccessTcpClient()
        {
        }

        protected override void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected();
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            if (Received != null)
                Received(buffer, offset, size);
        }
    }

}
