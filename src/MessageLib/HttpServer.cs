using System;
using System.IO;

namespace MessageLib
{
    /// <summary>
    /// HTTP server is used to create HTTP Web server and communicate with clients using HTTP protocol. It allows to receive GET, POST, PUT, DELETE requests and send HTTP responses.
    /// </summary>
    /// <remarks>Thread-safe.</remarks>
    public class HttpServer : TcpServer
    {

        /// <summary>
        /// Get the static content cache
        /// </summary>
        public FileCache Cache { get; private set; }

        /// <summary>
        /// Initialize HTTP server
        /// </summary>
        public HttpServer()
        {
            Cache = new FileCache();
        }

        /// <summary>
        /// Add static content cache
        /// </summary>
        /// <param name="path">Static content path</param>
        /// <param name="prefix">Cache prefix (default is "/")</param>
        /// <param name="filter">Cache filter (default is "*.*")</param>
        /// <param name="timeout">Refresh cache timeout (default is 1 hour)</param>
        public void AddStaticContent(string path, string prefix = "/", string filter = "*.*", TimeSpan? timeout = null)
        {
            if (timeout == null)
                timeout = new TimeSpan?(TimeSpan.FromHours(1));

            Cache.InsertPath(path, prefix, filter, timeout.Value, Handler);
        }

        private bool Handler(FileCache cache, string key, byte[] value, TimeSpan timespan)
        {
            HttpResponse header = new HttpResponse();
            header.SetBegin(200);
            header.SetContentType(Path.GetExtension(key));
            header.SetHeader("Cache-Control", string.Format("max-age={0}", timespan.Seconds));
            header.SetBody(value);
            return cache.Add(key, header.Cache.Data, timespan);
        }

        /// <summary>
        /// Remove static content cache
        /// </summary>
        /// <param name="path">Static content path</param>
        public void RemoveStaticContent(string path) { Cache.RemovePath(path); }
        /// <summary>
        /// Clear static content cache
        /// </summary>
        public void ClearStaticContent() { Cache.Clear(); }

        protected override TcpSession CreateSession() { return new HttpSession(); }

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
                    Cache.Dispose();
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
