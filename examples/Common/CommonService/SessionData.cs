using System;

namespace CommonService
{
    public class SessionData : IDisposable
    {

        private const int headLength = 4;
        public long Position { get; set; }
        public byte[] Head { get; set; }
        public byte[] Data { get; set; }

        public bool UseRawData { get; set; }
        public long RawDataLength { get; set; }

        public SessionData()
        {
            this.Head = new byte[headLength];
        }

        public void Dispose()
        {
            this.Head = null;
            this.Data = null;
        }
    }
}
