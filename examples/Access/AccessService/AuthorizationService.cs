using System.Configuration;
using MessageLib;

namespace AccessService
{
    public class AuthorizationService : TcpServer
    {

        public AccessInfo AccessInfo { get { return AccessInfo.instance; } }

        public AuthorizationService()
        {
        }

        protected override TcpSession CreateSession()
        {
            return new AuthorizationSession();
        }

        protected override void OnStarted()
        {
            this.AccessInfo.AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        }

    }
}
