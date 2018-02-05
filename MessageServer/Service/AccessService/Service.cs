using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MessageLib;

namespace AccessService
{
    public class Service : TcpServer
    {
        string strPwd = "";
        public Service()
        {
            this.OnAccept += Service_OnAccept;
            this.OnReceive += Service_OnReceive;
        }

        private HandleResult Service_OnAccept(TcpServer sender, IntPtr connId, IntPtr pClient)
        {
            strPwd = INIOperation.ReadString("Info", "Password");
            var data = Encoding.Default.GetBytes("欢迎连接\r\n请输入密码:");
            this.Send(connId, data, data.Length);
            this.SetExtra(connId, "");
            return HandleResult.Ok;
        }

        private HandleResult Service_OnReceive(IntPtr connId, byte[] bytes)
        {
            byte[] data = null;
            var strData = this.GetExtra<string>(connId);
            strData += Encoding.Default.GetString(bytes);
            if (strData.Length > 24)
            {
                data = Encoding.Default.GetBytes("密码长度错误！");
                this.Send(connId, data, data.Length);
                Thread.Sleep(100);
                this.Disconnect(connId);
            }
            else if (strData.IndexOf('\n') > 0)
            {
                if (strData.Trim() != strPwd)
                {
                    data = Encoding.Default.GetBytes("密码错误！");
                    this.Send(connId, data, data.Length);
                    Thread.Sleep(100);
                    this.Disconnect(connId);
                    return HandleResult.Ok;
                }
                foreach (var cId in this.GetAllConnectionIDs())
                {
                    if (cId == connId)
                        continue;
                    this.Disconnect(cId);
                }
                string ip = "";
                ushort port = 0;
                this.GetRemoteAddress(connId, ref ip, ref port);
                INIOperation.WriteString("Info", "IP", ip);
                data = Encoding.Default.GetBytes("设置成功！");
                this.Send(connId, data, data.Length);
                Thread.Sleep(100);
                this.Disconnect(connId);
            }
            else
                this.SetExtra(connId, strData);
            return HandleResult.Ok;
        }
    }
}
