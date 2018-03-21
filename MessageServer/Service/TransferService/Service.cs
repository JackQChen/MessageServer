using System;
using System.Configuration;
using MessageLib;

namespace TransferService
{
    public class Service : TcpServer
    {
        TcpAgent agent;
        string forwardIp = "";
        ushort forwardPort = 0;

        public Service()
        {
            this.OnPrepareListen += new TcpServerEvent.OnPrepareListenEventHandler(Service_OnPrepareListen);
            this.OnAccept += new TcpServerEvent.OnAcceptEventHandler(Service_OnAccept);
            this.OnReceive += new TcpServerEvent.OnReceiveEventHandler(Service_OnReceive);
            this.OnClose += new TcpServerEvent.OnCloseEventHandler(Service_OnClose);
            agent = new TcpAgent();
            agent.Start("0.0.0.0", false);
            agent.OnReceive += new TcpAgentEvent.OnReceiveEventHandler(agent_OnReceive);
            agent.OnClose += new TcpAgentEvent.OnCloseEventHandler(agent_OnClose);
        }

        HandleResult Service_OnPrepareListen(TcpServer sender, IntPtr soListen)
        {
            forwardIp = ConfigurationManager.AppSettings[this.Name + "_ForwardIP"];
            forwardPort = ushort.Parse(ConfigurationManager.AppSettings[this.Name + "_ForwardPort"]);
            return HandleResult.Ignore;
        }

        HandleResult Service_OnAccept(TcpServer sender, IntPtr connId, IntPtr pClient)
        {
            var aId = IntPtr.Zero;
            if (agent.Connect(forwardIp, forwardPort, ref aId))
            {
                this.SetExtra(connId, aId);
                agent.SetExtra(aId, connId);
            }
            else
                this.Disconnect(connId);
            return HandleResult.Ignore;
        }

        HandleResult Service_OnReceive(IntPtr connId, byte[] bytes)
        {
            var aId = this.GetExtra<IntPtr>(connId);
            this.agent.Send(aId, bytes, bytes.Length);
            return HandleResult.Ignore;
        }

        HandleResult Service_OnClose(TcpServer sender, IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            this.agent.Disconnect(this.GetExtra<IntPtr>(connId));
            return HandleResult.Ignore;
        }

        HandleResult agent_OnReceive(IntPtr connId, byte[] bytes)
        {
            var cId = this.agent.GetExtra<IntPtr>(connId);
            this.Send(cId, bytes, bytes.Length);
            return HandleResult.Ignore;
        }

        HandleResult agent_OnClose(IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            this.Disconnect(this.agent.GetExtra<IntPtr>(connId));
            return HandleResult.Ignore;
        }
    }
}
