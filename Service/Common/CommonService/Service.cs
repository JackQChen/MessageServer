using MessageLib.SocketBase;
using MessageLib.SocketBase.Protocol;

namespace CommonService
{
    public class Service : AppServer<CommonSession, CommonRequest>
    {

        public Service() : base(new DefaultReceiveFilterFactory<CommonReceiveFilter, CommonRequest>())
        {
            this.NewRequestReceived += Service_NewRequestReceived;
        }

        private void Service_NewRequestReceived(CommonSession currentSession, CommonRequest requestInfo)
        {
            foreach (var session in this.GetAllSessions())
            {
                var bytes = requestInfo.Body.ToBytes();
                session.Send(bytes, 0, bytes.Length);
            }
        }

    }
}
