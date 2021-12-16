﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MessageLib.Logging;

namespace MessageLib
{
    /// <summary>
    /// TCP server is used to connect, disconnect and manage TCP sessions
    /// </summary>
    /// <remarks>Thread-safe</remarks>
    public class TcpServer : IDisposable
    {
        /// <summary>
        /// Server name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Server logger
        /// </summary>
        protected internal Logger Logger { get; private set; }

        /// <summary>
        /// IP endpoint
        /// </summary>
        public IPEndPoint Endpoint { get; set; }

        /// <summary>
        /// Number of sessions connected to the server
        /// </summary>
        public int SessionCount { get { return Sessions.Count; } }
        /// <summary>
        /// Number of bytes pending sent by the server
        /// </summary>
        public int BytesPending { get { return _bytesPending; } }
        /// <summary>
        /// Number of bytes sent by the server
        /// </summary>
        public long BytesSent { get { return _bytesSent; } }
        /// <summary>
        /// Number of bytes received by the server
        /// </summary>
        public long BytesReceived { get { return _bytesReceived; } }

        /// <summary>
        /// Option: dual mode socket
        /// </summary>
        /// <remarks>
        /// Specifies whether the Socket is a dual-mode socket used for both IPv4 and IPv6.
        /// Will work only if socket is bound on IPv6 address.
        /// </remarks>
        public bool OptionDualMode { get; set; }
        /// <summary>
        /// Option: keep alive
        /// </summary>
        /// <remarks>
        /// This option will setup SO_KEEPALIVE if the OS support this feature
        /// </remarks>
        public bool OptionKeepAlive { get; set; }
        /// <summary>
        /// Option: no delay
        /// </summary>
        /// <remarks>
        /// This option will enable/disable Nagle's algorithm for TCP protocol
        /// </remarks>
        public bool OptionNoDelay { get; set; }
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
        /// Option: accepting backlog size
        /// </summary>
        public int OptionBacklog { get; set; }
        /// <summary>
        /// Option: max connection count
        /// </summary>
        public int OptionMaxConnectionCount { get; set; }
        /// <summary>
        /// Option: receive buffer size
        /// </summary>
        public int OptionReceiveBufferSize { get; set; }
        /// <summary>
        /// Option: send buffer size
        /// </summary>
        public int OptionSendBufferSize { get; set; }

        //Server private variable
        private BufferManager _bufferManager;
        private ConcurrentStack<SocketArgs[]> _readWritePool;
        private int _sessionId;

        // Server accept
        private Socket _acceptSocket;
        private SocketAsyncEventArgs _acceptEventArg;

        // Server statistic
        internal int _bytesPending;
        internal long _bytesSent;
        internal long _bytesReceived;

        /// <summary>
        /// Is the server started?
        /// </summary>
        public bool IsStarted { get; private set; }
        /// <summary>
        /// Is the server accepting new clients?
        /// </summary>
        public bool IsAccepting { get; private set; }

        #region Initialize

        /// <summary>
        /// Initialize TCP server
        /// </summary>
        public TcpServer()
        {
            OptionBacklog = 100;
            OptionMaxConnectionCount = 100;
            OptionReceiveBufferSize = 8192;
            OptionSendBufferSize = 8192;
            OptionKeepAlive = true;
            IsSocketDisposed = true;
            Endpoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void Initialize()
        {
            // Initial logger
            Logger = new Logger(Name);

            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds against memory fragmentation
            _bufferManager = new BufferManager(OptionReceiveBufferSize * OptionMaxConnectionCount, OptionReceiveBufferSize);
            _bufferManager.InitBuffer();

            _readWritePool = new ConcurrentStack<SocketArgs[]>();
            for (int i = 0; i < OptionMaxConnectionCount; i++)
            {
                // add SocketAsyncEventArg to the pool
                _readWritePool.Push(new SocketArgs[]
                {
                    InitializeEventArgs(SocketAsyncOperation.Receive),
                    InitializeEventArgs(SocketAsyncOperation.Send)
                });
            }
        }

        private SocketArgs InitializeEventArgs(SocketAsyncOperation operation)
        {
            // Pre-allocate a set of reusable SocketAsyncEventArgs
            SocketArgs eventArg = new SocketArgs(operation);
            eventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
            if (operation == SocketAsyncOperation.Receive)
                _bufferManager.SetBuffer(eventArg);
            return eventArg;
        }

        #endregion

        #region Start/Stop server

        /// <summary>
        /// Create a new socket object
        /// </summary>
        /// <remarks>
        /// Method may be override if you need to prepare some specific socket object in your implementation.
        /// </remarks>
        /// <returns>Socket object</returns>
        protected virtual Socket CreateSocket()
        {
            return new Socket(Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start the server
        /// </summary>
        /// <returns>'true' if the server was successfully started, 'false' if the server failed to start</returns>
        public virtual bool Start()
        {
            if (IsStarted)
                return false;

            // Setup accept event arg
            _acceptEventArg = new SocketAsyncEventArgs();
            _acceptEventArg.Completed += AcceptEventArg_Completed;

            // Create a new accept socket
            _acceptSocket = CreateSocket();

            // Update the accept socket disposed flag
            IsSocketDisposed = false;

            // Apply the option: buffer size
            _acceptSocket.ReceiveBufferSize = OptionReceiveBufferSize;
            _acceptSocket.SendBufferSize = OptionSendBufferSize;
            // Apply the option: reuse address
            _acceptSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, OptionReuseAddress);
            // Apply the option: exclusive address use
            _acceptSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, OptionExclusiveAddressUse);
            // Apply the option: dual mode (this option must be applied before listening)
            if (_acceptSocket.AddressFamily == AddressFamily.InterNetworkV6)
                _acceptSocket.DualMode(OptionDualMode);

            // Bind the accept socket to the IP endpoint
            _acceptSocket.Bind(Endpoint);
            // Refresh the endpoint property based on the actual endpoint created
            Endpoint = (IPEndPoint)_acceptSocket.LocalEndPoint;

            // Call the server starting handler
            OnStarting();

            // Start listen to the accept socket with the given accepting backlog size
            _acceptSocket.Listen(OptionBacklog);

            // Reset statistic
            _bytesPending = 0;
            _bytesSent = 0;
            _bytesReceived = 0;

            // Update the started flag
            IsStarted = true;

            // Call the server started handler
            OnStarted();

            // Perform the first server accept
            IsAccepting = true;
            StartAccept(_acceptEventArg);

            return true;
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        /// <returns>'true' if the server was successfully stopped, 'false' if the server is already stopped</returns>
        public virtual bool Stop()
        {
            if (!IsStarted)
                return false;

            // Stop accepting new clients
            IsAccepting = false;

            // Reset accept event arg
            _acceptEventArg.Completed -= AcceptEventArg_Completed;

            // Call the server stopping handler
            OnStopping();

            try
            {
                // Close the accept socket
                _acceptSocket.Close();

                // Dispose the accept socket
                _acceptSocket.Dispose();

                // Dispose event arguments
                _acceptEventArg.Dispose();

                // Update the accept socket disposed flag
                IsSocketDisposed = true;
            }
            catch (ObjectDisposedException) { }

            // Disconnect all sessions
            DisconnectAll();

            // Update the started flag
            IsStarted = false;

            // Call the server stopped handler
            OnStopped();

            return true;
        }

        /// <summary>
        /// Restart the server
        /// </summary>
        /// <returns>'true' if the server was successfully restarted, 'false' if the server failed to restart</returns>
        public virtual bool Restart()
        {
            if (!Stop())
                return false;

            while (IsStarted)
                Thread.Yield();

            return Start();
        }

        #endregion

        #region Accepting clients

        /// <summary>
        /// Start accept a new client connection
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            // Socket must be cleared since the context object is being reused
            e.AcceptSocket = null;

            // Async accept a new client connection
            if (!_acceptSocket.AcceptAsync(e))
                ProcessAccept(e);
        }

        /// <summary>
        /// Process accepted client connection
        /// </summary>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                SocketArgs[] eventArgs;
                if (_readWritePool.TryPop(out eventArgs))
                {
                    // Create a new session to register
                    var session = CreateSession();

                    // Register the session
                    RegisterSession(session, eventArgs);

                    // Connect new session
                    session.Connect(e.AcceptSocket);
                }
                else
                    e.AcceptSocket.SafeClose();
            }
            else
                SendError("ProcessAccept", e.SocketError);

            // Accept the next client connection
            if (IsAccepting)
                StartAccept(e);
        }

        /// <summary>
        /// This method is the callback method associated with Socket.AcceptAsync()
        /// operations and is invoked when an accept operation is complete
        /// </summary>
        /// <summary>
        /// This method is the callback method associated with Socket.AcceptAsync()
        /// operations and is invoked when an accept operation is complete
        /// </summary>
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (IsSocketDisposed)
                return;

            ProcessAccept(e);
        }

        #endregion

        #region IO processing

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            var session = e.UserToken as TcpSession;
            if (session == null)
                return;
            session.OnAsyncCompleted(sender, e);
        }

        #endregion

        #region Session factory

        /// <summary>
        /// Create TCP session factory method
        /// </summary>
        /// <returns>TCP session</returns>
        protected virtual TcpSession CreateSession() { return new TcpSession(); }

        #endregion

        #region Session management

        // Server sessions
        protected readonly ConcurrentDictionary<int, TcpSession> Sessions = new ConcurrentDictionary<int, TcpSession>();

        /// <summary>
        /// Disconnect all connected sessions
        /// </summary>
        /// <returns>'true' if all sessions were successfully disconnected, 'false' if the server is not started</returns>
        public virtual bool DisconnectAll()
        {
            if (!IsStarted)
                return false;

            // Disconnect all sessions
            foreach (var session in Sessions.Values)
                session.Disconnect();

            return true;
        }

        /// <summary>
        /// Get all session Ids
        /// </summary>
        /// <returns>SessionIds</returns>
        public IEnumerable<int> GetSessionIds()
        {
            return Sessions.Keys;
        }

        /// <summary>
        /// Get a session with a given Id
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <returns>Session with a given Id or null if the session it not connected</returns>
        public TcpSession GetSession(int id)
        {
            // Try to find the required session
            TcpSession result;
            return Sessions.TryGetValue(id, out result) ? result : null;
        }

        /// <summary>
        /// Register a new session
        /// </summary>
        /// <param name="session">Session to register</param>
        /// <param name="eventArgs">Event args</param>
        internal void RegisterSession(TcpSession session, SocketArgs[] eventArgs)
        {
            // Register a new session
            session.Initialize(Interlocked.Increment(ref _sessionId), this, eventArgs);
            Sessions.TryAdd(session.Id, session);
        }

        /// <summary>
        /// Unregister session by Id
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <param name="eventArgs">Event args</param>
        internal void UnregisterSession(int id, SocketArgs[] eventArgs)
        {
            // Unregister session by Id
            TcpSession temp;
            if (!Sessions.TryRemove(id, out temp))
                return;
            temp.Dispose();
            // Free the SocketAsyncEventArg so they can be reused by another client
            _readWritePool.Push(eventArgs);
        }

        #endregion

        #region Multicasting

        /// <summary>
        /// Multicast data to all connected sessions
        /// </summary>
        /// <param name="buffer">Buffer to multicast</param>
        /// <returns>'true' if the data was successfully multicasted, 'false' if the data was not multicasted</returns>
        public virtual bool Multicast(byte[] buffer) { return Multicast(buffer, 0, buffer.Length); }

        /// <summary>
        /// Multicast data to all connected clients
        /// </summary>
        /// <param name="buffer">Buffer to multicast</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>'true' if the data was successfully multicasted, 'false' if the data was not multicasted</returns>
        public virtual bool Multicast(byte[] buffer, int offset, int size)
        {
            if (!IsStarted)
                return false;

            if (size == 0)
                return true;

            // Multicast data to all sessions
            foreach (var session in Sessions.Values)
                session.SendAsync(buffer, offset, size);

            return true;
        }

        /// <summary>
        /// Multicast text to all connected clients
        /// </summary>
        /// <param name="text">Text string to multicast</param>
        /// <returns>'true' if the text was successfully multicasted, 'false' if the text was not multicasted</returns>
        public virtual bool Multicast(string text) { return Multicast(Encoding.UTF8.GetBytes(text)); }

        #endregion

        #region Server handlers

        /// <summary>
        /// Handle server starting notification
        /// </summary>
        protected virtual void OnStarting() { }
        /// <summary>
        /// Handle server started notification
        /// </summary>
        protected virtual void OnStarted() { }
        /// <summary>
        /// Handle server stopping notification
        /// </summary>
        protected virtual void OnStopping() { }
        /// <summary>
        /// Handle server stopped notification
        /// </summary>
        protected virtual void OnStopped() { }

        /// <summary>
        /// Handle session connecting notification
        /// </summary>
        /// <param name="session">Connecting session</param>
        protected virtual void OnConnecting(TcpSession session) { }
        /// <summary>
        /// Handle session connected notification
        /// </summary>
        /// <param name="session">Connected session</param>
        protected virtual void OnConnected(TcpSession session) { }
        /// <summary>
        /// Handle session disconnecting notification
        /// </summary>
        /// <param name="session">Disconnecting session</param>
        protected virtual void OnDisconnecting(TcpSession session) { }
        /// <summary>
        /// Handle session disconnected notification
        /// </summary>
        /// <param name="session">Disconnected session</param>
        protected virtual void OnDisconnected(TcpSession session) { }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected virtual void OnError(SocketError error) { }

        internal void OnConnectingInternal(TcpSession session) { OnConnecting(session); }
        internal void OnConnectedInternal(TcpSession session) { OnConnected(session); }
        internal void OnDisconnectingInternal(TcpSession session) { OnDisconnecting(session); }
        internal void OnDisconnectedInternal(TcpSession session) { OnDisconnected(session); }

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

        /// <summary>
        /// Accept socket disposed flag
        /// </summary>
        public bool IsSocketDisposed { get; private set; }

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
                    Stop();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                IsDisposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~TcpServer()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
