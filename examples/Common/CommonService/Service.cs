using MessageLib;

namespace CommonService
{
    public class Service : TcpServer
    {

        internal Handler handler = new Handler();

        public Service()
        {
        }

        protected override TcpSession CreateSession()
        {
            return new Session();
        }

    }
}
