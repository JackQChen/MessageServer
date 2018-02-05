using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using MessageLib;

namespace MessageServer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        ListExtra<TcpServer> serverList = new ListExtra<TcpServer>();
        ListExtra<ListExtra<string>> clientList = new ListExtra<ListExtra<string>>();
        int lastSelectedIndex = -1;
        Action<string, string> actServiceState, actLog;

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            actServiceState = (name, state) =>
            {
                this.lvService.Items[name].SubItems[4].Text = state;
            };
            actLog = (name, log) =>
            {
                this.txtLog.AppendText(log);
                this.tabControl1.SelectedTab = this.tabLog;
            };
            this.InitService();
            this.tsStart.PerformClick();
            this.Close();
        }

        private void tsStart_Click(object sender, System.EventArgs e)
        {
            this.labState.Text = "正在启动...";
            this.statusStrip1.Update();
            StartService();
            if (this.lvService.Items.Count > 0)
                this.lvService.Items[0].Selected = true;
            this.labState.Text = "服务正在运行...";
            this.statusStrip1.Update();
            this.tsStart.Enabled = false;
            this.tsStop.Enabled = true;
        }

        private void tsStop_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("确定停止服务吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StopService();
                this.labState.Text = "服务已停止";
                this.statusStrip1.Update();
                this.tsStart.Enabled = true;
                this.tsStop.Enabled = false;
            }
        }

        private void InitService()
        {
            var config = (ServiceConfig)ConfigurationManager.GetSection("ServiceConfig");
            foreach (Config cfg in config.Configs)
            {
                TcpServer service = null;
                try
                {
                    service = Activator.CreateInstance(Type.GetType(cfg.Type)) as TcpServer;
                    service.Name = cfg.Name;
                    service.Port = ushort.Parse(cfg.Port);
                    this.serverList.Set(cfg.Name, service);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message
                    + "\r\n" + (ex.InnerException == null ? "" : ex.InnerException.Message), "错误 - "
                    + cfg.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
                ListViewItem serviceItem = this.lvService.Items.Add(cfg.Name, "", -1);
                serviceItem.SubItems.Add("TCP");
                serviceItem.SubItems.Add(cfg.Port);
                serviceItem.SubItems.Add(cfg.Name);
                serviceItem.SubItems.Add("准备就绪");
                serviceItem.Tag = new ServiceInfo(service);
                this.clientList.Set(cfg.Name, new ListExtra<string>());
                service.OnError += (srv, connId, ex) =>
                {
                    if (this.tabControl1.IsHandleCreated)
                        this.tabControl1.Invoke(actLog,
                            srv.Name,
                            string.Format("\r\n{0} - {1}\r\n{2}:{3}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            srv.Name,
                            this.clientList.Get(srv.Name).Get(connId.ToString()),
                            ex.Message)
                        );
                    return HandleResult.Ok;
                };
                service.OnPrepareListen += (srv, so) =>
                {
                    if (this.lvService.IsHandleCreated)
                        this.lvService.Invoke(actServiceState, srv.Name, "正在运行");
                    return HandleResult.Ok;
                };
                service.OnAccept += (srv, connId, pClient) =>
                {
                    string ip = "";
                    ushort port = 0;
                    srv.GetRemoteAddress(connId, ref ip, ref port);
                    var cList = this.clientList.Get(srv.Name);
                    cList.Set(connId.ToString(), ip + ":" + port);
                    return HandleResult.Ok;
                };
                service.OnClose += (srv, connId, enOperation, errorCode) =>
                {
                    var cList = this.clientList.Get(srv.Name);
                    cList.Remove(connId.ToString());
                    return HandleResult.Ok;
                };
                service.OnShutdown += srv =>
                {
                    if (this.lvService.IsHandleCreated)
                        this.lvService.Invoke(actServiceState, srv.Name, "已停止");
                    return HandleResult.Ok;
                };
            }
        }

        private void StartService()
        {
            foreach (var key in this.serverList.dict.Keys)
            {
                var server = this.serverList.Get(key);
                if (!server.Start())
                {
                    MessageBox.Show(string.Format("服务启动失败:{0}({1})", server.ErrorMessage, server.ErrorCode), "错误 - " + key, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
            }
        }

        private void StopService()
        {
            foreach (var service in this.serverList.dict.Values)
                service.Stop();
        }

        private void tsAddProtocol_Click(object sender, EventArgs e)
        {

        }

        private void tsRemoveProtocol_Click(object sender, EventArgs e)
        {

        }

        private void tsSetting_Click(object sender, System.EventArgs e)
        {
            //new FrmSetting().ShowDialog();
            var frm = Activator.CreateInstance(Type.GetType("SystemConfig.FrmMain,SystemConfig")) as Form;
            frm.ShowDialog(this);
        }

        void Exit()
        {
            if (MessageBox.Show("确定停止服务并退出吗？", "提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.FormClosing -= this.FrmMain_FormClosing;
                this.StopService();
                Application.Exit();
            }
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void lvService_DoubleClick(object sender, EventArgs e)
        {
            this.btnServicePanel.PerformClick();
        }

        private void lvService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvService.SelectedItems.Count != 1)
                return;
            var curIndex = this.lvService.SelectedItems[0].Index;
            if (lastSelectedIndex != curIndex)
            {
                var srvName = this.lvService.Items[curIndex].Name;
                var cList = this.clientList.Get(srvName);
                cList.isChanged = true;
                this.lvService.Items[curIndex].SubItems[0].Text = "●";
                if (this.lastSelectedIndex != -1)
                    this.lvService.Items[lastSelectedIndex].SubItems[0].Text = "";
                lastSelectedIndex = curIndex;
                this.btnServicePanel.Enabled = this.serverList.Get(srvName) as IServiceUI != null;
            }
        }

        void UpdateClientList()
        {
            if (this.lastSelectedIndex == -1)
                return;
            var cList = this.clientList.Get(this.lvService.Items[this.lastSelectedIndex].Name);
            if (!cList.isChanged)
                return;
            this.lvClient.Items.Clear();
            foreach (var key in cList.dict.Keys)
            {
                ListViewItem item = this.lvClient.Items.Add(key, key, -1);
                item.SubItems.Add(cList.Get(key));
                item.SubItems.Add("连接");
            }
            cList.isChanged = false;
        }

        private void btnServicePanel_Click(object sender, EventArgs e)
        {
            if (this.lastSelectedIndex == -1)
                return;
            var srvName = this.lvService.Items[lastSelectedIndex].Name;
            var service = this.serverList.Get(srvName);
            var ui = service as IServiceUI;
            if (ui == null)
                return;
            using (var frm = Activator.CreateInstance(ui.UIType, new object[] { service }) as Form)
            {
                if (frm != null)
                    frm.ShowDialog(this);
            }
        }

        private void btnDisConn_Click(object sender, EventArgs e)
        {
            if (this.lastSelectedIndex == -1)
                return;
            var srvName = this.lvService.Items[lastSelectedIndex].Name;
            foreach (ListViewItem item in this.lvClient.SelectedItems)
                this.serverList.Get(srvName).Disconnect(new IntPtr(Convert.ToInt32(item.Name)));
        }

        private void timerClient_Tick(object sender, EventArgs e)
        {
            UpdateClientList();
        }

        private void timerServerStatus_Tick(object sender, EventArgs e)
        {
            if (this.lastSelectedIndex == -1)
                return;
            var si = this.lvService.Items[lastSelectedIndex].Tag as ServiceInfo;
            si.接收速率 = FormatFileSize(si.totalRecv - si.lastRecv) + "/s";
            si.发送速率 = FormatFileSize(si.totalSend - si.lastSend) + "/s";
            si.lastRecv = si.totalRecv;
            si.lastSend = si.totalSend;
            this.pgService.SelectedObject = si;
        }

        internal class ServiceInfo
        {
            private TcpServer server;
            internal long lastRecv, lastSend;
            internal long totalRecv, totalSend;

            public ServiceInfo(TcpServer srv)
            {
                this.server = srv;
            }

            [Category("服务状态")]
            public string 当前连接数 { get { return this.server.ConnectionCount.ToString(); } set { } }
            [Category("服务状态")]
            public string 累计发送
            {
                get
                {
                    this.totalSend = this.server.TotalSendCount;
                    return FormatFileSize(this.server.TotalSendCount);
                }
                set { }
            }
            [Category("服务状态")]
            public string 累计接收
            {
                get
                {
                    this.totalRecv = this.server.TotalRecvCount;
                    return FormatFileSize(this.server.TotalRecvCount);
                }
                set { }
            }
            [Category("服务状态")]
            public string 发送速率 { get; set; }
            [Category("服务状态")]
            public string 接收速率 { get; set; }

            [Category("缓存池状态")]
            public string 最大连接数 { get { return this.server.MaxConnectionCount.ToString(); } set { } }
            [Category("缓存池状态")]
            public string 最大并发量 { get { return this.server.AcceptSocketCount.ToString(); } set { } }
            [Category("缓存池状态")]
            public string 工作线程数 { get { return this.server.WorkerThreadCount.ToString(); } set { } }

        }

        /// <summary>
        /// 转换文件大小格式
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns></returns>
        internal static string FormatFileSize(long fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            else
                return string.Format("{0} Bytes", fileSize);
        }

        private void txtLog_DoubleClick(object sender, EventArgs e)
        {
            this.txtLog.Clear();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.notifyIcon.ShowBalloonTip(100);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

    }
}
