using System;
using System.IO;
using System.Text;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Logging;
using MessageLib.SocketBase.Protocol;

namespace TelnetService
{
    public class Service : AppServer
    {
        string logDir;

        public Service()
        {
            this.NewSessionConnected += Service_NewSessionConnected;
            this.NewRequestReceived += Service_NewRequestReceived;
            this.logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }

        private void Service_NewSessionConnected(AppSession session)
        {
            session.Send("Welcome back! Mr. Chan");
        }

        private void Service_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            if (requestInfo.Key == "exit")
            {
                session.Send("bye!");
                session.Close();
                return;
            }
            else if (requestInfo.Key == "log")
            {
                string path;
                if (string.IsNullOrEmpty(requestInfo.Body))
                {
                    path = Path.Combine(logDir, LogLevel.Info, LogLevel.Info + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                }
                else
                {
                    path = Path.Combine(logDir, requestInfo.Body, requestInfo.Body + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                }
                if (!File.Exists(path))
                {
                    session.Send("log file does not exist!\r\n");
                    return;
                }
                var str = File.ReadAllText(path, Encoding.UTF8);
                session.Send(str.Replace("\n", "\r\n"));
                return;
            }
            session.Send("received message:" + requestInfo.Key + " " + requestInfo.Body);
        }
    }
}
