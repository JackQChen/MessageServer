using System;
using MessageLib.Facility.Protocol;

namespace CommonService
{
    public class CommonReceiveFilter : FixedHeaderReceiveFilter<CommonRequest>
    {
        public CommonReceiveFilter() : base(4)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return BitConverter.ToInt32(header, offset);
        }

        protected override CommonRequest ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new CommonRequest
            {
                Body = bodyBuffer.ToObject<Message>(offset, length)
            };
        }
    }
}
