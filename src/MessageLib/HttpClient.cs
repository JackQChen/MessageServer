﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MessageLib
{
    /// <summary>
    /// HTTP client is used to communicate with HTTP Web server. It allows to send GET, POST, PUT, DELETE requests and receive HTTP result.
    /// </summary>
    /// <remarks>Thread-safe.</remarks>
    public class HttpClient : TcpClient
    {

        /// <summary>
        /// Get the HTTP request
        /// </summary>
        public HttpRequest Request { get; protected set; }

        /// <summary>
        /// Get the HTTP response
        /// </summary>
        protected HttpResponse Response { get; set; }

        /// <summary>
        /// Initialize HTTP client
        /// </summary>
        public HttpClient()
        {
            Request = new HttpRequest();
            Response = new HttpResponse();
        }

        #region Send request / Send request body

        /// <summary>
        /// Send the current HTTP request (synchronous)
        /// </summary>
        /// <returns>Size of sent data</returns>
        public int SendRequest() { return SendRequest(Request); }
        /// <summary>
        /// Send the HTTP request (synchronous)
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Size of sent data</returns>
        public int SendRequest(HttpRequest request) { return Send(request.Cache.Data, request.Cache.Offset, request.Cache.Size); }

        /// <summary>
        /// Send the HTTP request body (synchronous)
        /// </summary>
        /// <param name="body">HTTP request body</param>
        /// <returns>Size of sent data</returns>
        public int SendRequestBody(string body) { return Send(body); }
        /// <summary>
        /// Send the HTTP request body (synchronous)
        /// </summary>
        /// <param name="buffer">HTTP request body buffer</param>
        /// <returns>Size of sent data</returns>
        public int SendRequestBody(byte[] buffer) { return Send(buffer); }
        /// <summary>
        /// Send the HTTP request body (synchronous)
        /// </summary>
        /// <param name="buffer">HTTP request body buffer</param>
        /// <param name="offset">HTTP request body buffer offset</param>
        /// <param name="size">HTTP request body size</param>
        /// <returns>Size of sent data</returns>
        public int SendRequestBody(byte[] buffer, int offset, int size) { return Send(buffer, offset, size); }

        /// <summary>
        /// Send the current HTTP request (asynchronous)
        /// </summary>
        /// <returns>'true' if the current HTTP request was successfully sent, 'false' if the session is not connected</returns>
        public bool SendRequestAsync() { return SendRequestAsync(Request); }
        /// <summary>
        /// Send the HTTP request (asynchronous)
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>'true' if the current HTTP request was successfully sent, 'false' if the session is not connected</returns>
        public bool SendRequestAsync(HttpRequest request) { return SendAsync(request.Cache.Data, request.Cache.Offset, request.Cache.Size); }

        /// <summary>
        /// Send the HTTP request body (asynchronous)
        /// </summary>
        /// <param name="body">HTTP request body</param>
        /// <returns>'true' if the HTTP request body was successfully sent, 'false' if the session is not connected</returns>
        public bool SendRequestBodyAsync(string body) { return SendAsync(body); }
        /// <summary>
        /// Send the HTTP request body (asynchronous)
        /// </summary>
        /// <param name="buffer">HTTP request body buffer</param>
        /// <returns>'true' if the HTTP request body was successfully sent, 'false' if the session is not connected</returns>
        public bool SendRequestBodyAsync(byte[] buffer) { return SendAsync(buffer); }
        /// <summary>
        /// Send the HTTP request body (asynchronous)
        /// </summary>
        /// <param name="buffer">HTTP request body buffer</param>
        /// <param name="offset">HTTP request body buffer offset</param>
        /// <param name="size">HTTP request body size</param>
        /// <returns>'true' if the HTTP request body was successfully sent, 'false' if the session is not connected</returns>
        public bool SendRequestBodyAsync(byte[] buffer, int offset, int size) { return SendAsync(buffer, offset, size); }

        #endregion

        #region Session handlers

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            // Receive HTTP response header
            if (Response.IsPendingHeader())
            {
                if (Response.ReceiveHeader(buffer, offset, size))
                    OnReceivedResponseHeader(Response);

                size = 0;
            }

            // Check for HTTP response error
            if (Response.IsErrorSet)
            {
                OnReceivedResponseError(Response, "Invalid HTTP response!");
                Response.Clear();
                Disconnect();
                return;
            }

            // Receive HTTP response body
            if (Response.ReceiveBody(buffer, offset, size))
            {
                OnReceivedResponse(Response);
                Response.Clear();
                return;
            }

            // Check for HTTP response error
            if (Response.IsErrorSet)
            {
                OnReceivedResponseError(Response, "Invalid HTTP response!");
                Response.Clear();
                Disconnect();
                return;
            }
        }

        protected override void OnDisconnected()
        {
            // Receive HTTP response body
            if (Response.IsPendingBody())
            {
                OnReceivedResponse(Response);
                Response.Clear();
                return;
            }
        }

        /// <summary>
        /// Handle HTTP response header received notification
        /// </summary>
        /// <remarks>Notification is called when HTTP response header was received from the server.</remarks>
        /// <param name="response">HTTP request</param>
        protected virtual void OnReceivedResponseHeader(HttpResponse response) { }

        /// <summary>
        /// Handle HTTP response received notification
        /// </summary>
        /// <remarks>Notification is called when HTTP response was received from the server.</remarks>
        /// <param name="response">HTTP response</param>
        protected virtual void OnReceivedResponse(HttpResponse response) { }

        /// <summary>
        /// Handle HTTP response error notification
        /// </summary>
        /// <remarks>Notification is called when HTTP response error was received from the server.</remarks>
        /// <param name="response">HTTP response</param>
        /// <param name="error">HTTP response error</param>
        protected virtual void OnReceivedResponseError(HttpResponse response, string error) { }

        #endregion
    }

    /// <summary>
    /// HTTP extended client make requests to HTTP Web server with returning Task as a synchronization primitive.
    /// </summary>
    /// <remarks>Thread-safe.</remarks>
    public class HttpClientEx : HttpClient
    {

        #region Send request

        /// <summary>
        /// Send current HTTP request
        /// </summary>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendRequest(TimeSpan? timeout = null) { return SendRequest(Request, timeout); }
        /// <summary>
        /// Send HTTP request
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="timeout">HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendRequest(HttpRequest request, TimeSpan? timeout = null)
        {
            if (timeout == null)
                timeout = new TimeSpan?(TimeSpan.FromMinutes(1));

            _tcs = new TaskCompletionSource<HttpResponse>();
            Request = request;

            // Check if the HTTP request is valid
            if (Request.IsEmpty || Request.IsErrorSet)
            {
                SetPromiseError("Invalid HTTP request!");
                return _tcs.Task;
            }

            if (!IsConnected)
            {
                // Connect to the Web server
                if (!ConnectAsync())
                {
                    SetPromiseError("Connection failed!");
                    return _tcs.Task;
                }
            }
            else
            {
                // Send prepared HTTP request
                if (!SendRequestAsync())
                {
                    SetPromiseError("Failed to send HTTP request!");
                    return _tcs.Task;
                }
            }

            // Create a new timeout timer
            if (_timer == null)
                _timer = new Timer(TimeoutHandler, null, Timeout.Infinite, Timeout.Infinite);

            // Start the timeout timer
            _timer.Change((int)timeout.Value.TotalMilliseconds, Timeout.Infinite);

            return _tcs.Task;
        }

        private void TimeoutHandler(object state)
        {
            // Disconnect on timeout
            OnReceivedResponseError(Response, "Timeout!");
            Response.Clear();
            DisconnectAsync();
        }

        /// <summary>
        /// Send HEAD request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendHeadRequest(string url, TimeSpan? timeout = null) { return SendRequest(Request.MakeHeadRequest(url), timeout); }
        /// <summary>
        /// Send GET request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendGetRequest(string url, TimeSpan? timeout = null) { return SendRequest(Request.MakeGetRequest(url), timeout); }
        /// <summary>
        /// Send POST request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="content">Content</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendPostRequest(string url, string content, TimeSpan? timeout = null) { return SendRequest(Request.MakePostRequest(url, content), timeout); }
        /// <summary>
        /// Send PUT request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="content">Content</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendPutRequest(string url, string content, TimeSpan? timeout = null) { return SendRequest(Request.MakePutRequest(url, content), timeout); }
        /// <summary>
        /// Send DELETE request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendDeleteRequest(string url, TimeSpan? timeout = null) { return SendRequest(Request.MakeDeleteRequest(url), timeout); }
        /// <summary>
        /// Send OPTIONS request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendOptionsRequest(string url, TimeSpan? timeout = null) { return SendRequest(Request.MakeOptionsRequest(url), timeout); }
        /// <summary>
        /// Send TRACE request
        /// </summary>
        /// <param name="url">URL to request</param>
        /// <param name="timeout">Current HTTP request timeout (default is 1 minute)</param>
        /// <returns>HTTP request Task</returns>
        public Task<HttpResponse> SendTraceRequest(string url, TimeSpan? timeout = null) { return SendRequest(Request.MakeTraceRequest(url), timeout); }

        #endregion

        #region Session handlers

        protected override void OnConnected()
        {
            // Send prepared HTTP request on connect
            if (!Request.IsEmpty && !Request.IsErrorSet)
                if (!SendRequestAsync())
                    SetPromiseError("Failed to send HTTP request!");
        }

        protected override void OnDisconnected()
        {
            // Cancel timeout check timer
            if (this._timer != null)
                this._timer.Change(Timeout.Infinite, Timeout.Infinite);

            base.OnDisconnected();
        }

        protected override void OnReceivedResponse(HttpResponse response)
        {
            // Cancel timeout check timer
            if (this._timer != null)
                this._timer.Change(Timeout.Infinite, Timeout.Infinite);

            SetPromiseValue(response);
        }

        protected override void OnReceivedResponseError(HttpResponse response, string error)
        {
            // Cancel timeout check timer
            if (this._timer != null)
                this._timer.Change(Timeout.Infinite, Timeout.Infinite);

            SetPromiseError(error);
        }

        #endregion

        private TaskCompletionSource<HttpResponse> _tcs = new TaskCompletionSource<HttpResponse>();
        private Timer _timer;

        private void SetPromiseValue(HttpResponse response)
        {
            Response = new HttpResponse();
            _tcs.SetResult(response);
            Request.Clear();
        }

        private void SetPromiseError(string error)
        {
            _tcs.SetException(new Exception(error));
            Request.Clear();
        }

        #region IDisposable implementation

        // Disposed flag.
        private bool _disposed;

        protected override void Dispose(bool disposingManagedResources)
        {
            if (!_disposed)
            {
                if (disposingManagedResources)
                {
                    // Dispose managed resources here...
                    if (this._timer != null)
                        this._timer.Dispose();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }

            // Call Dispose in the base class.
            base.Dispose(disposingManagedResources);
        }

        // The derived class does not have a Finalize method
        // or a Dispose method without parameters because it inherits
        // them from the base class.

        #endregion
    }
}
