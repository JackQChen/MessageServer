using System.Net.Sockets;

namespace MessageLib
{
    /// <summary>
    /// SocketArgs
    /// </summary>
    public class SocketArgs : SocketAsyncEventArgs
    {
        public SocketAsyncOperation Operation { get; private set; }

        private object lockObject = new object();
        private bool _inUse;

        public SocketArgs(SocketAsyncOperation operation)
        {
            Operation = operation;
        }

        public bool InUse()
        {
            return _inUse;
        }

        public bool Lock()
        {
            lock (lockObject)
            {
                if (_inUse)
                    return false;
                _inUse = true;
                return true;
            }
        }

        public void Unlock()
        {
            lock (lockObject)
            {
                _inUse = false;
            }
        }

        public void Reset()
        {
            UserToken = null;
            if (Operation == SocketAsyncOperation.Send)
                SetBuffer(null, 0, 0);
            Unlock();
        }
    }
}
