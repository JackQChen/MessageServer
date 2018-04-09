using System;
using System.Configuration;
using System.IO;
using System.Text;
using MessageLib;

namespace AccessService
{
    public class Service : TcpServer
    {
        TcpAgent agent;
        bool isAccess = false;
        string strKey = "";
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
            isAccess = INIOperation.ReadString("Info", "ServiceName") == this.Name;
            if (isAccess)
            {
                strKey = INIOperation.ReadString("Info", "AccessKey");
                INIOperation.WriteString("IPList", null, null);
            }
            else
            {
                forwardIp = ConfigurationManager.AppSettings[this.Name + "_ForwardIP"];
                forwardPort = ushort.Parse(ConfigurationManager.AppSettings[this.Name + "_ForwardPort"]);
            }
            return HandleResult.Ignore;
        }

        HandleResult Service_OnAccept(TcpServer sender, IntPtr connId, IntPtr pClient)
        {
            string ip = "";
            ushort port = 0;
            this.GetRemoteAddress(connId, ref ip, ref port);
            if (isAccess)
            {
                var key = Guid.NewGuid().ToString();
                var keyData = Encoding.Default.GetBytes(Encrypt.AESEncrypt(key, strKey) + "\r\n");
                this.Send(connId, keyData, keyData.Length);
                this.SetExtra(connId, new ExtraData() { Key = key, IP = ip });
                return HandleResult.Ignore;
            }
            if (!bool.Parse(INIOperation.ReadString("IPList", ip, "false")))
            {
                this.Disconnect(connId);
                this.Log(string.Format("[{0}]连接失败:IP={1}", this.Name, ip));
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
                var extraData = this.GetExtra<ExtraData>(connId);
                extraData.Data += Encoding.Default.GetString(bytes);
                if (extraData.Data.Length > 128)
                {
                    this.Disconnect(connId);
                }
                else if (extraData.Data.IndexOf('\n') > 0)
                {
                    if (extraData.Data.Trim() == extraData.Key)
                    {
                        var data = Encoding.Default.GetBytes("已于"
                            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            + "成功授权！\r\n");
                        this.Send(connId, data, data.Length);
                        INIOperation.WriteString("IPList", extraData.IP, "true");
                        extraData.Access = true;
                    }
                    else
                    {
                        this.Disconnect(connId);
                        this.Log(string.Format("[{0}]验证失败:IP={1}", this.Name, extraData.IP));
                    }
                }
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
                string ip = "";
                ushort port = 0;
                this.GetRemoteAddress(connId, ref ip, ref port);
                var result = false;
                foreach (var cid in this.GetAllConnectionIDs())
                {
                    if (cid == connId)
                        continue;
                    var data = this.GetExtra<ExtraData>(cid);
                    if (data.IP == ip && data.Access)
                    {
                        result = true;
                        break;
                    }
                }
                if (!result)
                    INIOperation.WriteString("IPList", ip, "false");
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

        void Log(string strLog)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.AppendAllText(dir + DateTime.Now.ToString("yyyyMMdd") + ".log",
                     string.Format("{0}\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strLog));
        }
    }
}
