using System;
using System.Text;
using MessageLib;

namespace AccessService
{
    public class AuthorizationSession : TcpSession
    {

        AuthorizationService service;

        public string VerificationKey { get; set; }
        public string ReceivedText { get; set; }
        public string ClientIP { get; set; }

        public AuthorizationSession()
        {
        }

        protected override void Initialize()
        {
            this.service = this.Server as AuthorizationService;
        }

        protected override void OnConnected()
        {
            this.ClientIP = this.Socket.RemoteEndPoint.ToString();
            this.VerificationKey = Guid.NewGuid().ToString();
            var encryptKey = Encrypt.AESEncrypt(this.VerificationKey, this.service.AccessInfo.AccessKey) + "\r\n";
            this.SendAsync(Encoding.UTF8.GetBytes(encryptKey));
        }

        protected override void OnReceived(byte[] buffer, int offset, int size)
        {
            this.ReceivedText += Encoding.UTF8.GetString(buffer, offset, size);
            if (this.ReceivedText.Length > 128)
                this.Disconnect();
            else if (this.ReceivedText.IndexOf('\n') > 0)
            {
                if (this.ReceivedText.Trim() == this.VerificationKey)
                {
                    var data = Encoding.UTF8.GetBytes("授权成功！\r\n");
                    this.SendAsync(data);
                    this.service.AccessInfo.IPList.Add(this.ClientIP);
                }
                else
                {
                    this.Disconnect();
                    this.Logger.WarnFormat("[{0}]验证失败:IP={1}", this.Server.Name, this.ClientIP);
                }
            }
        }

        protected override void OnDisconnected()
        {
            this.service.AccessInfo.IPList.Remove(this.ClientIP);
        }

    }
}
