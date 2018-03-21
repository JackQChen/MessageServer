using System;
using System.Configuration;
using System.Text;
using MessageLib;

namespace AccessService
{
    public class Service : TcpServer
    {
        TcpAgent agent;
        bool isAccess = false;
        string strPwd = "";
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
            isAccess = INIOperation.ReadString("Info", "AccessKey") == this.Name;
            if (isAccess)
                strPwd = INIOperation.ReadString("Info", "Password");
            else
            {
                forwardIp = ConfigurationManager.AppSettings[this.Name + "_ForwardIP"];
                forwardPort = ushort.Parse(ConfigurationManager.AppSettings[this.Name + "_ForwardPort"]);
            }
            return HandleResult.Ignore;
        }

        HandleResult Service_OnAccept(TcpServer sender, IntPtr connId, IntPtr pClient)
        {
            if (isAccess)
                return HandleResult.Ignore;
            if (!bool.Parse(INIOperation.ReadString("Info", "Access")))
            {
                this.Disconnect(connId);
                return HandleResult.Ignore;
            }
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
            if (isAccess)
            {
                var strData = this.GetExtra<string>(connId);
                strData += Encoding.Default.GetString(bytes);
                if (strData.Length > 24)
                    this.Disconnect(connId);
                else if (strData.IndexOf('\n') > 0)
                {
                    if (strData.Trim() == strPwd)
                    {
                        INIOperation.WriteString("Info", "Access", "true");
                        var data = Encoding.Default.GetBytes("已于"
                            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            + "成功授权！\r\n");
                        this.Send(connId, data, data.Length);
                    }
                    else
                        this.Disconnect(connId);
                    return HandleResult.Ignore;
                }
                else
                    this.SetExtra(connId, strData);
            }
            else
            {
                var aId = this.GetExtra<IntPtr>(connId);
                this.agent.Send(aId, bytes, bytes.Length);
            }
            return HandleResult.Ignore;
        }

        HandleResult Service_OnClose(TcpServer sender, IntPtr connId, SocketOperation enOperation, int errorCode)
        {
            if (isAccess)
            {
                var result = false;
                foreach (var cId in this.GetAllConnectionIDs())
                {
                    if (cId == connId)
                        continue;
                    var data = this.GetExtra<string>(cId);
                    if (string.IsNullOrEmpty(data))
                        continue;
                    if (data.Trim() == strPwd)
                    {
                        result = true;
                        break;
                    }
                }
                if (!result)
                    INIOperation.WriteString("Info", "Access", "false");
            }
            else
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
