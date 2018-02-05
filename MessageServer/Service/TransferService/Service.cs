using System;
using System.Configuration;
using MessageLib;

namespace TransferService
{
    public class Service : TcpServer
    {
        TcpAgent agent;
        string remoteIp = "";
        ushort remotePort = 0;

        public Service()
        {
            remoteIp = ConfigurationManager.AppSettings["RemoteIP"];
            remotePort = ushort.Parse(ConfigurationManager.AppSettings["RemotePort"]);
            this.OnAccept += new TcpServerEvent.OnAcceptEventHandler(Service_OnAccept);
            this.OnReceive += new TcpServerEvent.OnReceiveEventHandler(Service_OnReceive);
            this.OnClose += new TcpServerEvent.OnCloseEventHandler(Service_OnClose);
            agent = new TcpAgent();
            agent.Start("0.0.0.0", false);
            agent.OnReceive += new TcpAgentEvent.OnReceiveEventHandler(agent_OnReceive);
            agent.OnClose += new TcpAgentEvent.OnCloseEventHandler(agent_OnClose);
        }

        HandleResult Service_OnAccept(TcpServer sender, System.IntPtr connId, System.IntPtr pClient)
        {
            var aId = IntPtr.Zero;
            if (agent.Connect(remoteIp, remotePort, ref aId))
            {
                this.SetExtra(connId, aId);
                agent.SetExtra(aId, connId);
            }
            else
                this.Disconnect(connId);
            return HandleResult.Ok;
        }

        HandleResult Service_OnReceive(System.IntPtr connId, byte[] bytes)
        {
            var aId = this.GetExtra<IntPtr>(connId);
            this.agent.Send(aId, bytes, bytes.Length);
            return HandleResult.Ok;
        }

        HandleResult Service_OnClose(TcpServer sender, IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            this.agent.Disconnect(this.GetExtra<IntPtr>(connId));
            return HandleResult.Ok;
        }

        HandleResult agent_OnReceive(System.IntPtr connId, byte[] bytes)
        {
            var cId = this.agent.GetExtra<IntPtr>(connId);
            this.Send(cId, bytes, bytes.Length);
            return HandleResult.Ok;
        }

        HandleResult agent_OnClose(IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            this.Disconnect(this.agent.GetExtra<IntPtr>(connId));
            return HandleResult.Ok;
        }
    }
}
