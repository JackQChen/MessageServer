using System;
using System.IO;
using System.Linq;
using System.Text;
using MessageLib;
using MessageLib.Logging;

namespace TelnetService
{
    public class Session : TcpSession
    {
        StringBuilder strBuffer = new StringBuilder();
        string logDir;

        public Session()
        {
        }

        protected override void Initialize()
        {
            logDir = (this.Server as Service).logDir;
        }

        protected override void OnConnected()
        {
            this.SendAsync("Welcome back! Mr. Chan\r\n");
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            strBuffer.Append(Encoding.UTF8.GetString(buffer, offset, size));
            var strFull = strBuffer.Replace("\r", "").ToString();
            var lastIndex = strFull.LastIndexOf('\n');
            if (lastIndex > -1)
            {
                foreach (var strLine in strFull.Substring(0, lastIndex).Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    var strArray = strLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    ProcessRequest(strArray[0], strArray.Skip(1).ToArray());
                }
                strBuffer.Clear();
                strBuffer.Append(strFull.Substring(lastIndex + 1));
            }
        }

        private void ProcessRequest(string key, params string[] body)
        {
            if (key == "exit")
            {
                this.SendAsync("bye!\r\n");
                this.Disconnect();
                return;
            }
            else if (key == "log")
            {
                string path;
                if (body.Length == 0)
                    path = Path.Combine(logDir, LogLevel.Info, LogLevel.Info + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                else
                    path = Path.Combine(logDir, body[0], body[0] + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                if (!File.Exists(path))
                {
                    this.SendAsync("log file does not exist!\r\n");
                    return;
                }
                var str = File.ReadAllText(path, Encoding.UTF8) + "\r\n";
                this.SendAsync(str.Replace("\n", "\r\n"));
                return;
            }
            this.SendAsync("received message:" + key + string.Join("", body.Select(s => " " + s)) + "\r\n");
        }
    }
}
