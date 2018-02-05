using System;
using MessageLib;

namespace CommonService
{
    public class Service : TcpServer<ExtraData>
    {
        Process process;

        public Service()
        {
            process = new Process();
            process.ReceiveMessage += new Action<Message>(process_ReceiveMessage);
            this.OnAccept += new TcpServerEvent.OnAcceptEventHandler(Service_OnAccept);
            this.OnClose += new TcpServerEvent.OnCloseEventHandler(Service_OnClose);
            this.OnReceive += new TcpServerEvent.OnReceiveEventHandler(Service_OnReceive);
        }

        HandleResult Service_OnAccept(TcpServer server, IntPtr connId, IntPtr pClient)
        {
            this.SetExtra(connId, new ExtraData());
            return HandleResult.Ok;
        }

        HandleResult Service_OnClose(TcpServer server, IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            this.RemoveExtra(connId);
            return HandleResult.Ok;
        }

        HandleResult Service_OnReceive(IntPtr connId, byte[] bytes)
        {
            ExtraData msg = null;
            try
            {
                msg = this.GetExtra(connId);
                this.process.RecvData(msg, bytes);
            }
            catch (Exception ex)
            {
                this.SDK_OnError(this, connId, ex);
                msg.Data = null;
                msg.Position = 0;
            }
            return HandleResult.Ok;
        }

        void process_ReceiveMessage(Message message)
        {
            if (this.ConnectionCount > 0)
                foreach (var cId in this.GetAllConnectionIDs())
                {
                    var bytes = this.process.FormatterMessageBytes(message);
                    this.Send(cId, bytes, bytes.Length);
                }
        }

    }
}
