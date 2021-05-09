using MessageLib.SocketBase.Protocol;

namespace CommonService
{

    public class CommonRequest : IRequestInfo<Message>
    {
        public Message Body { get; set; }

        public string Key { get; set; }
    }
}
