using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessageLib.Logging;

namespace MessageLib
{
    /// <summary>
    /// UDP client is used to read/write data from/into the connected UDP server
    /// </summary>
    /// <remarks>Thread-safe</remarks>
    public class UdpClient : IDisposable
    {
        /// <summary>
        /// IP endpoint
        /// </summary>
        public IPEndPoint Endpoint { get; set; }
        /// <summary>
        /// Socket
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Number of bytes pending sent by the client
        /// </summary>
        public int BytesPending { get; private set; }
        /// <summary>
        /// Number of bytes sending by the client
        /// </summary>
        public int BytesSending { get; private set; }
        /// <summary>
        /// Number of bytes sent by the client
        /// </summary>
        public long BytesSent { get; private set; }
        /// <summary>
        /// Number of bytes received by the client
        /// </summary>
        public long BytesReceived { get; private set; }
        /// <summary>
        /// Number of datagrams sent by the client
        /// </summary>
        public long DatagramsSent { get; private set; }
        /// <summary>
        /// Number of datagrams received by the client
        /// </summary>
        public long DatagramsReceived { get; private set; }

        /// <summary>
        /// Option: dual mode socket
        /// </summary>
        /// <remarks>
        /// Specifies whether the Socket is a dual-mode socket used for both IPv4 and IPv6.
        /// Will work only if socket is bound on IPv6 address.
        /// </remarks>
        public bool OptionDualMode { get; set; }
        /// <summary>
        /// Option: reuse address
        /// </summary>
        /// <remarks>
        /// This option will enable/disable SO_REUSEADDR if the OS support this feature
        /// </remarks>
        public bool OptionReuseAddress { get; set; }
        /// <summary>
        /// Option: enables a socket to be bound for exclusive access
        /// </summary>
        /// <remarks>
        /// This option will enable/disable SO_EXCLUSIVEADDRUSE if the OS support this feature
        /// </remarks>
        public bool OptionExclusiveAddressUse { get; set; }
        /// <summary>
        /// Option: bind the socket to the multicast UDP server
        /// </summary>
        public bool OptionMulticast { get; set; }
        /// <summary>
        /// Option: receive buffer size
        /// </summary>
        public int OptionReceiveBufferSize { get; set; }
        /// <summary>
        /// Option: send buffer size
        /// </summary>
        public int OptionSendBufferSize { get; set; }

        #region Connect/Disconnect client

        protected Logger Logger { get; set; }

        /// <summary>
        /// Is the client connected?
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Initialize UDP client
        /// </summary>
        public UdpClient()
        {
            OptionReceiveBufferSize = 8192;
            OptionSendBufferSize = 8192;
            Endpoint = new IPEndPoint(IPAddress.Any, 0);

            //Setup logger
            Logger = new Logger("UdpClient");
        }

        /// <summary>
        /// Create a new socket object
        /// </summary>
        /// <remarks>
        /// Method may be override if you need to prepare some specific socket object in your implementation.
        /// </remarks>
        /// <returns>Socket object</returns>
        protected virtual Socket CreateSocket()
        {
            return new Socket(Endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// Connect the client (synchronous)
        /// </summary>
        /// <returns>'true' if the client was successfully connected, 'false' if the client failed to connect</returns>
        public virtual bool Connect()
        {
            if (IsConnected)
                return false;

            // Setup event args
            _receiveEventArg = new SocketArgs(SocketAsyncOperation.Receive);
            _receiveEventArg.SetBuffer(new byte[OptionReceiveBufferSize], 0, OptionReceiveBufferSize);
            _receiveEventArg.Completed += OnAsyncCompleted;
            _sendEventArg = new SocketArgs(SocketAsyncOperation.Send);
            _sendEventArg.Completed += OnAsyncCompleted;

            // Create a new client socket
            Socket = CreateSocket();

            // Apply the option: reuse address
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, OptionReuseAddress);
            // Apply the option: exclusive address use
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, OptionExclusiveAddressUse);
            // Apply the option: dual mode (this option must be applied before recieving/sending)
            if (Socket.AddressFamily == AddressFamily.InterNetworkV6)
                Socket.DualMode(OptionDualMode);

            // Call the client connecting handler
            OnConnecting();

            try
            {
                // Bind the acceptor socket to the IP endpoint
                if (OptionMulticast)
                    Socket.Bind(Endpoint);
                else
                {
                    var endpoint = new IPEndPoint((Endpoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);
                    Socket.Bind(endpoint);
                }
            }
            catch (SocketException ex)
            {
                // Call the client error handler
                SendError("Connect", ex.SocketErrorCode);

                // Reset event args
                _receiveEventArg.Completed -= OnAsyncCompleted;
                _sendEventArg.Completed -= OnAsyncCompleted;

                // Call the client disconnecting handler
                OnDisconnecting();

                // Close the client socket
                Socket.Close();

                // Dispose the client socket
                Socket.Dispose();

                // Dispose event arguments
                _receiveEventArg.Dispose();
                _sendEventArg.Dispose();

                // Call the client disconnected handler
                OnDisconnected();

                return false;
            }

            // Prepare receive endpoint
            _receiveEndpoint = new IPEndPoint((Endpoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);

            // Reset statistic
            BytesPending = 0;
            BytesSending = 0;
            BytesSent = 0;
            BytesReceived = 0;
            DatagramsSent = 0;
            DatagramsReceived = 0;

            // Update the connected flag
            IsConnected = true;

            // Call the client connected handler
            OnConnected();

            return true;
        }

        /// <summary>
        /// Disconnect the client (synchronous)
        /// </summary>
        /// <returns>'true' if the client was successfully disconnected, 'false' if the client is already disconnected</returns>
        public virtual bool Disconnect()
        {
            if (!IsConnected)
                return false;

            // Reset event args
            _receiveEventArg.Completed -= OnAsyncCompleted;
            _sendEventArg.Completed -= OnAsyncCompleted;

            // Call the client disconnecting handler
            OnDisconnecting();

            try
            {
                // Close the client socket
                Socket.Close();

                // Dispose the client socket
                Socket.Dispose();

                // Dispose event arguments
                _receiveEventArg.Dispose();
                _sendEventArg.Dispose();

            }
            catch (ObjectDisposedException) { }

            // Update the connected flag
            IsConnected = false;

            // Call the client disconnected handler
            OnDisconnected();

            return true;
        }

        /// <summary>
        /// Reconnect the client (synchronous)
        /// </summary>
        /// <returns>'true' if the client was successfully reconnected, 'false' if the client is already reconnected</returns>
        public virtual bool Reconnect()
        {
            if (!Disconnect())
                return false;

            return Connect();
        }

        #endregion

        #region Multicast group

        /// <summary>
        /// Setup multicast: bind the socket to the multicast UDP server
        /// </summary>
        /// <param name="enable">Enable/disable multicast</param>
        public virtual void SetupMulticast(bool enable)
        {
            OptionReuseAddress = enable;
            OptionMulticast = enable;
        }

        /// <summary>
        /// Join multicast group with a given IP address (synchronous)
        /// </summary>
        /// <param name="address">IP address</param>
        public virtual void JoinMulticastGroup(IPAddress address)
        {
            if (Endpoint.AddressFamily == AddressFamily.InterNetworkV6)
                Socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, new IPv6MulticastOption(address));
            else
                Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(address));

            // Call the client joined multicast group notification
            OnJoinedMulticastGroup(address);
        }

        /// <summary>
        /// Join multicast group with a given IP address (synchronous)
        /// </summary>
        /// <param name="address">IP address</param>
        public virtual void JoinMulticastGroup(string address) { JoinMulticastGroup(IPAddress.Parse(address)); }

        /// <summary>
        /// Leave multicast group with a given IP address (synchronous)
        /// </summary>
        /// <param name="address">IP address</param>
        public virtual void LeaveMulticastGroup(IPAddress address)
        {
            if (Endpoint.AddressFamily == AddressFamily.InterNetworkV6)
                Socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, new IPv6MulticastOption(address));
            else
                Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, new MulticastOption(address));

            // Call the client left multicast group notification
            OnLeftMulticastGroup(address);
        }

        /// <summary>
        /// Leave multicast group with a given IP address (synchronous)
        /// </summary>
        /// <param name="address">IP address</param>
        public virtual void LeaveMulticastGroup(string address) { LeaveMulticastGroup(IPAddress.Parse(address)); }

        #endregion

        #region Send/Recieve data

        // Receive and send endpoints
        EndPoint _receiveEndpoint;
        EndPoint _sendEndpoint;

        ArraySegment<byte> _sendBuffer;
        private SocketArgs _receiveEventArg;
        private SocketArgs _sendEventArg;

        /// <summary>
        /// Send datagram to the connected server (synchronous)
        /// </summary>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(byte[] buffer) { return Send(buffer, 0, buffer.Length); }

        /// <summary>
        /// Send datagram to the connected server (synchronous)
        /// </summary>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(byte[] buffer, int offset, int size) { return Send(Endpoint, buffer, offset, size); }

        /// <summary>
        /// Send text to the connected server (synchronous)
        /// </summary>
        /// <param name="text">Text string to send</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(string text) { return Send(Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Send datagram to the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(EndPoint endpoint, byte[] buffer) { return Send(endpoint, buffer, 0, buffer.Length); }

        /// <summary>
        /// Send datagram to the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(EndPoint endpoint, byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return 0;

            if (size == 0)
                return 0;

            try
            {
                // Sent datagram to the server
                int sent = Socket.SendTo(buffer, offset, size, SocketFlags.None, endpoint);
                if (sent > 0)
                {
                    // Update statistic
                    DatagramsSent++;
                    BytesSent += sent;

                    // Call the datagram sent handler
                    OnSent(endpoint, sent);
                }

                return sent;
            }
            catch (ObjectDisposedException) { return 0; }
            catch (SocketException ex)
            {
                SendError("Send", ex.SocketErrorCode);
                Disconnect();
                return 0;
            }
        }

        /// <summary>
        /// Send text to the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="text">Text string to send</param>
        /// <returns>Size of sent datagram</returns>
        public virtual int Send(EndPoint endpoint, string text) { return Send(endpoint, Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Send datagram to the connected server (asynchronous)
        /// </summary>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        public virtual bool SendAsync(byte[] buffer) { return SendAsync(buffer, 0, buffer.Length); }

        /// <summary>
        /// Send datagram to the connected server (asynchronous)
        /// </summary>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        public virtual bool SendAsync(byte[] buffer, int offset, int size) { return SendAsync(Endpoint, buffer, offset, size); }

        /// <summary>
        /// Send text to the connected server (asynchronous)
        /// </summary>
        /// <param name="text">Text string to send</param>
        /// <returns>'true' if the text was successfully sent, 'false' if the text was not sent</returns>
        public virtual bool SendAsync(string text) { return SendAsync(Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Send datagram to the given endpoint (asynchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        public virtual bool SendAsync(EndPoint endpoint, byte[] buffer) { return SendAsync(endpoint, buffer, 0, buffer.Length); }

        /// <summary>
        /// Send datagram to the given endpoint (asynchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="buffer">Datagram buffer to send</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
        public virtual bool SendAsync(EndPoint endpoint, byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return false;

            if (size == 0)
                return true;

            // Update send endpoint
            _sendEndpoint = endpoint;

            // Try to send
            _sendBuffer = new ArraySegment<byte>(buffer, offset, size);
            TrySend();

            return true;
        }

        /// <summary>
        /// Send text to the given endpoint (asynchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to send</param>
        /// <param name="text">Text string to send</param>
        /// <returns>'true' if the text was successfully sent, 'false' if the text was not sent</returns>
        public virtual bool SendAsync(EndPoint endpoint, string text) { return SendAsync(endpoint, Encoding.UTF8.GetBytes(text)); }

        /// <summary>
        /// Receive a new datagram from the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to receive from</param>
        /// <param name="buffer">Datagram buffer to receive</param>
        /// <returns>Size of received datagram</returns>
        public virtual int Receive(ref EndPoint endpoint, byte[] buffer) { return Receive(ref endpoint, buffer, 0, buffer.Length); }

        /// <summary>
        /// Receive a new datagram from the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to receive from</param>
        /// <param name="buffer">Datagram buffer to receive</param>
        /// <param name="offset">Datagram buffer offset</param>
        /// <param name="size">Datagram buffer size</param>
        /// <returns>Size of received datagram</returns>
        public virtual int Receive(ref EndPoint endpoint, byte[] buffer, int offset, int size)
        {
            if (!IsConnected)
                return 0;

            if (size == 0)
                return 0;

            try
            {
                // Receive datagram from the server
                int received = Socket.ReceiveFrom(buffer, offset, size, SocketFlags.None, ref endpoint);

                // Update statistic
                DatagramsReceived++;
                BytesReceived += received;

                // Call the datagram received handler
                OnReceived(endpoint, buffer, offset, size);

                return received;
            }
            catch (ObjectDisposedException) { return 0; }
            catch (SocketException ex)
            {
                SendError("Receive", ex.SocketErrorCode);
                Disconnect();
                return 0;
            }
        }

        /// <summary>
        /// Receive text from the given endpoint (synchronous)
        /// </summary>
        /// <param name="endpoint">Endpoint to receive from</param>
        /// <param name="size">Text size to receive</param>
        /// <returns>Received text</returns>
        public virtual string Receive(ref EndPoint endpoint, int size)
        {
            var buffer = new byte[size];
            var length = Receive(ref endpoint, buffer);
            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        /// <summary>
        /// Receive datagram from the server (asynchronous)
        /// </summary>
        public virtual void ReceiveAsync()
        {
            // Try to receive datagram
            TryReceive();
        }

        /// <summary>
        /// Try to receive new data
        /// </summary>
        private void TryReceive()
        {
            if (!IsConnected)
                return;

            if (!_receiveEventArg.Lock())
                return;

            try
            {
                // Async receive with the receive handler
                _receiveEventArg.RemoteEndPoint = _receiveEndpoint;
                if (!Socket.ReceiveFromAsync(_receiveEventArg))
                    OnAsyncCompleted(null, _receiveEventArg);
            }
            catch (ObjectDisposedException)
            {
                _receiveEventArg.Unlock();
            }
        }

        /// <summary>
        /// Try to send pending data
        /// </summary>
        private void TrySend()
        {
            if (!IsConnected)
                return;

            if (!_sendEventArg.Lock())
                return;

            try
            {
                // Async send with the send handler
                _sendEventArg.RemoteEndPoint = _sendEndpoint;
                _sendEventArg.SetBuffer(_sendBuffer.Array, _sendBuffer.Offset, _sendBuffer.Count);
                if (!Socket.SendToAsync(_sendEventArg))
                    OnAsyncCompleted(null, _sendEventArg);
            }
            catch (ObjectDisposedException)
            {
                _sendEventArg.Unlock();
            }
        }

        #endregion

        #region IO processing

        /// <summary>
        /// This method is called whenever a receive or send operation is completed on a socket
        /// </summary>
        private void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.ReceiveFrom:
                    ProcessReceiveFrom(e);
                    break;
                case SocketAsyncOperation.SendTo:
                    ProcessSendTo(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }

        /// <summary>
        /// This method is invoked when an asynchronous receive from operation completes
        /// </summary>
        private void ProcessReceiveFrom(SocketAsyncEventArgs e)
        {
            _receiveEventArg.Unlock();

            if (!IsConnected)
                return;

            // Disconnect on error
            if (e.SocketError != SocketError.Success)
            {
                SendError("ProcessReceiveFrom", e.SocketError);
                Disconnect();
                return;
            }

            // Received some data from the server
            int size = e.BytesTransferred;

            // Update statistic
            DatagramsReceived++;
            BytesReceived += size;

            // Call the datagram received handler
            OnReceived(e.RemoteEndPoint, _receiveEventArg.Buffer, _receiveEventArg.Offset, size);
        }

        /// <summary>
        /// This method is invoked when an asynchronous send to operation completes
        /// </summary>
        private void ProcessSendTo(SocketAsyncEventArgs e)
        {
            _sendEventArg.Unlock();

            if (!IsConnected)
                return;

            // Disconnect on error
            if (e.SocketError != SocketError.Success)
            {
                SendError("ProcessSendTo", e.SocketError);
                Disconnect();
                return;
            }

            int sent = e.BytesTransferred;

            // Send some data to the server
            if (sent > 0)
            {
                // Update statistic
                BytesSending = 0;
                BytesSent += sent;

                // Call the buffer sent handler
                OnSent(_sendEndpoint, sent);
            }
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
        /// Handle client joined multicast group notification
        /// </summary>
        /// <param name="address">IP address</param>
        protected virtual void OnJoinedMulticastGroup(IPAddress address) { }
        /// <summary>
        /// Handle client left multicast group notification
        /// </summary>
        /// <param name="address">IP address</param>
        protected virtual void OnLeftMulticastGroup(IPAddress address) { }

        /// <summary>
        /// Handle datagram received notification
        /// </summary>
        /// <param name="endpoint">Received endpoint</param>
        /// <param name="buffer">Received datagram buffer</param>
        /// <param name="offset">Received datagram buffer offset</param>
        /// <param name="size">Received datagram buffer size</param>
        /// <remarks>
        /// Notification is called when another datagram was received from some endpoint
        /// </remarks>
        protected virtual void OnReceived(EndPoint endpoint, byte[] buffer, int offset, int size) { }
        /// <summary>
        /// Handle datagram sent notification
        /// </summary>
        /// <param name="endpoint">Endpoint of sent datagram</param>
        /// <param name="sent">Size of sent datagram buffer</param>
        /// <remarks>
        /// Notification is called when a datagram was sent to the server.
        /// This handler could be used to send another datagram to the server for instance when the pending size is zero.
        /// </remarks>
        protected virtual void OnSent(EndPoint endpoint, int sent) { }

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
        ~UdpClient()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
