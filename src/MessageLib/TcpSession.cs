using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MessageLib.Logging;

namespace MessageLib
{
    /// <summary>
    /// TCP session is used to read and write data from the connected TCP client
    /// </summary>
    /// <remarks>Thread-safe</remarks>
    public class TcpSession : IDisposable
    {
        /// <summary>
        /// Initialize the session
        /// </summary>
        public TcpSession()
        {
            // Setup send queue
            _sendQueue = new ConcurrentQueue<ArraySegment<byte>>();
        }

        /// <summary>
        /// Initialize the session
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Setup session
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <param name="server">TCP server</param>
        /// <param name="eventArgs">Event args</param>
        internal void Initialize(int id, TcpServer server, SocketArgs[] eventArgs)
        {
            Id = id;
            Server = server;
            _receiveEventArg = eventArgs[0];
            _sendEventArg = eventArgs[1];
            _receiveEventArg.UserToken = this;
            _sendEventArg.UserToken = this;
            this.Initialize();
        }

        /// <summary>
        /// Session Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Session logger
        /// </summary>
        protected Logger Logger { get { return Server.Logger; } }

        /// <summary>
        /// Server
        /// </summary>
        public TcpServer Server { get; private set; }
        /// <summary>
        /// Socket
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Number of bytes pending sent by the session
        /// </summary>
        public int BytesPending { get; private set; }
        /// <summary>
        /// Number of bytes sending by the session
        /// </summary>
        public int BytesSending { get; private set; }
        /// <summary>
        /// Number of bytes sent by the session
        /// </summary>
        public long BytesSent { get; private set; }
        /// <summary>
        /// Number of bytes received by the session
        /// </summary>
        public long BytesReceived { get; private set; }

        #region Connect/Disconnect session

        /// <summary>
        /// Is the session connected?
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Connect the session
        /// </summary>
        /// <param name="socket">Session socket</param>
        internal void Connect(Socket socket)
        {
            Socket = socket;

            // Apply the option: buffer size
            Socket.ReceiveBufferSize = Server.OptionReceiveBufferSize;
            Socket.SendBufferSize = Server.OptionSendBufferSize;
            // Apply the option: keep alive (keepAliveTime = 10min, keepAliveInterval = 60s)
            if (Server.OptionKeepAlive)
                Socket.SetKeepAlive(600, 60);
            // Apply the option: no delay
            if (Server.OptionNoDelay)
                Socket.NoDelay = true;

            // Reset statistic
            BytesPending = 0;
            BytesSending = 0;
            BytesSent = 0;
            BytesReceived = 0;

            // Call the session connecting handler
            OnConnecting();

            // Call the session connecting handler in the server
            Server.OnConnectingInternal(this);

            // Update the connected flag
            IsConnected = true;

            // Try to receive something from the client
            TryReceive();

            // Call the session connected handler
            OnConnected();

            // Call the session connected handler in the server
            Server.OnConnectedInternal(this);

            // Call the empty send buffer handler
            if (_sendQueue.IsEmpty)
                OnEmpty();
        }

        /// <summary>
        /// Disconnect the session
        /// </summary>
        /// <returns>'true' if the section was successfully disconnected, 'false' if the section is already disconnected</returns>
        public virtual bool Disconnect()
        {
            if (!IsConnected)
                return false;

            // Call the session disconnecting handler
            OnDisconnecting();

            // Call the session disconnecting handler in the server
            Server.OnDisconnectingInternal(this);

            // Update the connected flag
            IsConnected = false;

            Socket.SafeClose();

            // Call the session disconnected handler
            OnDisconnected();

            // Call the session disconnected handler in the server
            Server.OnDisconnectedInternal(this);

            return true;
        }

        #endregion

        #region Send/Recieve data

        private SocketArgs _receiveEventArg;
        private SocketArgs _sendEventArg;
        private ConcurrentQueue<ArraySegment<byte>> _sendQueue;

        /// <summary>
        /// Send data to the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <returns>Size of sent data</returns>
        public virtual int Send(byte[] buffer) { return Send(buffer, 0, buffer.Length); }

        /// <summary>
        /// Send data to the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>Size of sent data</returns>
        public virtual int Send(byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return 0;

            if (size == 0)
                return 0;

            // Sent data to the client
            SocketError ec;
            int sent = Socket.Send(buffer, offset, size, SocketFlags.None, out ec);
            if (sent > 0)
            {
                // Update statistic
                BytesSent += sent;
                Interlocked.Add(ref Server._bytesSent, size);

                // Call the buffer sent handler
                OnSent(sent, BytesPending + BytesSending);
            }

            // Check for socket error
            if (ec != SocketError.Success)
            {
                SendError("Send", ec);
                Disconnect();
            }

            return sent;
        }

        /// <summary>
        /// Send text to the client (synchronous)
        /// </summary>
        /// <param name="text">Text string to send</param>
        /// <returns>Size of sent data</returns>
        public virtual int Send(string text) { return Send(Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        public virtual bool SendAsync(byte[] buffer) { return SendAsync(buffer, 0, buffer.Length); }

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        public virtual bool SendAsync(byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return false;

            if (size == 0)
                return true;

            // Fill the send buffer  
            _sendQueue.Enqueue(new ArraySegment<byte>(buffer, offset, size));

            // Update statistic
            BytesPending = _sendQueue.Count;

            // Try to send
            TrySend();

            return true;
        }

        /// <summary>
        /// Send text to the client (asynchronous)
        /// </summary>
        /// <param name="text">Text string to send</param>
        /// <returns>'true' if the text was successfully sent, 'false' if the session is not connected</returns>
        public virtual bool SendAsync(string text) { return SendAsync(Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Receive data from the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to receive</param>
        /// <returns>Size of received data</returns>
        public virtual int Receive(byte[] buffer) { return Receive(buffer, 0, buffer.Length); }

        /// <summary>
        /// Receive data from the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to receive</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>Size of received data</returns>
        public virtual int Receive(byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return 0;

            if (size == 0)
                return 0;

            // Receive data from the client
            SocketError ec;
            int received = Socket.Receive(buffer, offset, size, SocketFlags.None, out ec);
            if (received > 0)
            {
                // Update statistic
                BytesReceived += received;
                Interlocked.Add(ref Server._bytesReceived, received);

                // Call the buffer received handler
                OnReceived(buffer, 0, received);
            }

            // Check for socket error
            if (ec != SocketError.Success)
            {
                SendError("Receive", ec);
                Disconnect();
            }

            return received;
        }

        /// <summary>
        /// Receive text from the client (synchronous)
        /// </summary>
        /// <param name="size">Text size to receive</param>
        /// <returns>Received text</returns>
        public virtual string Receive(int size)
        {
            var buffer = new byte[size];
            var length = Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        /// <summary>
        /// Receive data from the client (asynchronous)
        /// </summary>
        public virtual void ReceiveAsync()
        {
            // Try to receive data from the client
            TryReceive();
        }

        /// <summary>
        /// Try to receive new data
        /// </summary>
        private void TryReceive()
        {
            if (!_receiveEventArg.Lock())
                return;

            try
            {
                // Async receive with the receive handler
                if (!Socket.ReceiveAsync(_receiveEventArg))
                    OnAsyncCompleted(null, _receiveEventArg);
            }
            catch (ObjectDisposedException)
            {
                _receiveEventArg.Unlock();
                Release();
            }
        }

        /// <summary>
        /// Try to send pending data
        /// </summary>
        private void TrySend()
        {
            if (!IsConnected)
            {
                Release();
                return;
            }

            if (!_sendEventArg.Lock())
                return;

            // Update statistic
            BytesPending = 0;
            BytesSending += _sendQueue.Count;

            if (_sendQueue.IsEmpty)
            {
                _sendEventArg.Unlock();

                if (!IsConnected)
                    Release();

                OnEmpty();

                return;
            }

            try
            {
                // Async send with the send handler
                ArraySegment<byte> buffer;
                _sendQueue.TryDequeue(out buffer);
                _sendEventArg.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);
                if (!Socket.SendAsync(_sendEventArg))
                    OnAsyncCompleted(null, _sendEventArg);
            }
            catch (ObjectDisposedException)
            {
                _sendEventArg.Unlock();
                Release();
            }
        }

        #endregion

        #region Release resources

        private bool _released;
        private object _releasedLock = new object();

        /// <summary>
        /// Release resources
        /// </summary>
        private void Release()
        {
            if (IsConnected)
                Disconnect();

            if (_receiveEventArg.InUse() || _sendEventArg.InUse())
                return;

            lock (_releasedLock)
            {
                if (_released)
                    return;
                _released = true;
            }

            // Update statistic
            BytesPending = 0;
            BytesSending = 0;

            _receiveEventArg.Reset();
            _sendEventArg.Reset();

            // Unregister session
            Server.UnregisterSession(Id, new SocketArgs[]
            {
                _receiveEventArg,
                _sendEventArg
            });
        }

        #endregion

        #region IO processing

        /// <summary>
        /// This method is called whenever a receive or send operation is completed on a socket
        /// </summary>
        internal void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    if (ProcessReceive(e))
                        TryReceive();
                    break;
                case SocketAsyncOperation.Send:
                    if (ProcessSend(e))
                        TrySend();
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }

        /// <summary>
        /// This method is invoked when an asynchronous receive operation completes
        /// </summary>
        private bool ProcessReceive(SocketAsyncEventArgs e)
        {
            int size = e.BytesTransferred;

            // Received some data from the client
            if (size > 0)
            {
                // Update statistic
                BytesReceived += size;
                Interlocked.Add(ref Server._bytesReceived, size);

                // Call the buffer received handler
                OnReceived(e.Buffer, e.Offset, size);
            }

            _receiveEventArg.Unlock();

            // Try to receive again if the session is valid
            if (e.SocketError == SocketError.Success && size > 0)
                return true;
            else if (e.SocketError != SocketError.Success)
                SendError("ProcessReceive", e.SocketError);

            Release();

            return false;
        }

        /// <summary>
        /// This method is invoked when an asynchronous send operation completes
        /// </summary>
        private bool ProcessSend(SocketAsyncEventArgs e)
        {
            int size = e.BytesTransferred;

            // Send some data to the client
            if (size > 0)
            {
                // Reset send buffer
                e.SetBuffer(null, 0, 0);

                // Update statistic
                BytesSending -= size;
                BytesSent += size;
                Interlocked.Add(ref Server._bytesSent, size);

                // Call the buffer sent handler
                OnSent(size, BytesPending + BytesSending);
            }

            _sendEventArg.Unlock();

            // Try to send again if the session is valid
            if (e.SocketError == SocketError.Success)
                return true;
            else
                SendError("ProcessSend", e.SocketError);

            Release();

            return false;
        }

        #endregion

        #region Session handlers

        /// <summary>
        /// Handle client connecting notification
        /// </summary>
        protected virtual void OnConnecting() { }
        /// <summary>
        /// Handle client connected notification
        /// </summary>
        protected virtual void OnConnected() { }
        /// <summary>
        /// Handle client disconnecting notification
        /// </summary>
        protected virtual void OnDisconnecting() { }
        /// <summary>
        /// Handle client disconnected notification
        /// </summary>
        protected virtual void OnDisconnected() { }

        /// <summary>
        /// Handle buffer received notification
        /// </summary>
        /// <param name="buffer">Received buffer</param>
        /// <param name="offset">Received buffer offset</param>
        /// <param name="size">Received buffer size</param>
        /// <remarks>
        /// Notification is called when another chunk of buffer was received from the client
        /// </remarks>
        protected virtual void OnReceived(byte[] buffer, int offset, int size) { }
        /// <summary>
        /// Handle buffer sent notification
        /// </summary>
        /// <param name="sent">Size of sent buffer</param>
        /// <param name="pending">Size of pending buffer</param>
        /// <remarks>
        /// Notification is called when another chunk of buffer was sent to the client.
        /// This handler could be used to send another buffer to the client for instance when the pending size is zero.
        /// </remarks>
        protected virtual void OnSent(int sent, int pending) { }

        /// <summary>
        /// Handle empty send buffer notification
        /// </summary>
        /// <remarks>
        /// Notification is called when the send buffer is empty and ready for a new data to send.
        /// This handler could be used to send another buffer to the client.
        /// </remarks>
        protected virtual void OnEmpty() { }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected virtual void OnError(SocketError error) { }

        #endregion

        #region Error handling

        /// <summary>
        /// Send error notification
        /// </summary>
        /// <param name="operation">Socket operation</param>
        /// <param name="error">Socket error code</param>
        private void SendError(string operation, SocketError error)
        {
            // Skip disconnect errors
            if (error == SocketError.ConnectionAborted ||
                error == SocketError.ConnectionRefused ||
                error == SocketError.ConnectionReset ||
                error == SocketError.OperationAborted ||
                error == SocketError.Shutdown)
                return;

            Logger.ErrorFormat("{0}:{1}", operation, error);
            OnError(error);
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Disposed flag
        /// </summary>
        public bool IsDisposed { get; private set; }

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            // The idea here is that Dispose(Boolean) knows whether it is
            // being called to do explicit cleanup (the Boolean is true)
            // versus being called due to a garbage collection (the Boolean
            // is false). This distinction is useful because, when being
            // disposed explicitly, the Dispose(Boolean) method can safely
            // execute code using reference type fields that refer to other
            // objects knowing for sure that these other objects have not been
            // finalized or disposed of yet. When the Boolean is false,
            // the Dispose(Boolean) method should not execute code that
            // refer to reference type fields because those objects may
            // have already been finalized."

            if (!IsDisposed)
            {
                if (disposingManagedResources)
                {
                    // Dispose managed resources here...
                    Disconnect();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                IsDisposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~TcpSession()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
