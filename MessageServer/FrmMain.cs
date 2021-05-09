using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MessageLib.Common;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Config;
using MessageLib.SocketEngine;
using MessageServer.Controls;
using MessageServer.Extension;
using MessageServer.Logging;

namespace MessageServer
{
    public partial class FrmMain : Form
    {

        DefaultBootstrap bootstrap;
        int selectedIndex = -1;
        IAppServer<IAppSession> selectedService;
        Action<string, string> actServiceState;
        Action<string, string, string> actLog;
        List<string> clientList;
        ViewerHost viewerHost;

        public FrmMain()
        {
            InitializeComponent();
        }

        #region Services

        private void InitService()
        {
            var configs = (ServiceConfig)ConfigurationManager.GetSection("ServiceConfig");
            var servers = configs.Configs.Cast<Config>()
                .Select(config => new ServerConfig
                {
                    Name = config.Name,
                    ServerType = config.Type,
                    Ip = "Any",
                    Port = config.Port,
                    MaxRequestLength = int.MaxValue,
                    MaxConnectionNumber = 10000,
                    DisableSessionSnapshot = true
                });
            var typeProvider = new TypeProvider();
            typeProvider.ElementInformation.Properties["name"].Value = "ServerLogFactory";
            typeProvider.ElementInformation.Properties["type"].Value = typeof(ServerLogFactory).AssemblyQualifiedName;
            bootstrap = new DefaultBootstrap(new ConfigurationSource
            {
                Servers = servers,
                LogFactory = typeProvider.Name,
                LogFactories = new TypeProvider[] { typeProvider },
                PerformanceDataCollectInterval = 1
            });
            bootstrap.Initialize();
            foreach (var config in servers)
            {
                ListViewItem serviceItem = this.lvService.Items.Add(config.Name, "TCP", -1);
                serviceItem.SubItems.Add(config.Port.ToString());
                serviceItem.SubItems.Add(config.Name);
                serviceItem.SubItems.Add("准备就绪").Name = "State";
                serviceItem.Tag = new ServiceInfo(bootstrap.PerfMonitor, config.Name);
            }
        }

        private void StartService()
        {
            this.bootstrap.Start();
            this.UpdateServiceState();
        }

        private void StopService()
        {
            this.bootstrap.Stop();
            this.UpdateServiceState();
        }

        private void UpdateServiceState()
        {
            foreach (var service in this.bootstrap.AppServers)
            {
                switch (service.State)
                {
                    case ServerState.Running:
                        this.lvService.Invoke(actServiceState, service.Name, "正在运行");
                        break;
                    default:
                        this.lvService.Invoke(actServiceState, service.Name, "已停止");
                        break;
                }
            }
        }

        #endregion

        #region Functions

        internal void Log(string name, string level, string log)
        {
            this.txtLog.BeginInvoke(actLog, name, level, log);
        }

        void InitViewerHost()
        {
            var position = this.tabServer.Location;
            position.Offset(this.tabPerformance.Bounds.Location);
            this.viewerHost = new ViewerHost();
            this.viewerHost.Visible = false;
            this.viewerHost.Location = position;
            this.viewerHost.Size = this.tabPerformance.Size;
            this.viewerHost.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            this.Controls.Add(this.viewerHost);
            this.viewerHost.BringToFront();
            this.viewerHost.AppFileName = AppDomain.CurrentDomain.BaseDirectory + "Viewer\\FlowViewer.exe";
            this.viewerHost.Start(Process.GetCurrentProcess().Id.ToString());
        }

        void UpdateClients()
        {
            var sessionList = this.selectedService.GetAllSessions().Select(s =>
            new
            {
                SessionID = s.SessionID.ToString(),
                IP = s.RemoteEndPoint.ToString()
            }).ToDictionary(k => k.SessionID, v => v.IP);
            var removeList = clientList.Except(sessionList.Keys).ToArray();
            foreach (var key in removeList)
            {
                this.lvClient.Items.RemoveByKey(key);
                this.clientList.Remove(key);
            }
            var addList = sessionList.Keys.Except(clientList);
            this.lvClient.Items.AddRange(addList.Select(key =>
            new ListViewItem(new string[] { key, sessionList[key], "连接" })
            {
                Name = key
            }).ToArray());
            this.clientList.AddRange(addList);
        }

        void DisconnectClients()
        {
            foreach (ListViewItem item in this.lvClient.SelectedItems)
            {
                var session = this.selectedService.GetSessionByID(Convert.ToInt32(item.Name));
                if (session != null)
                    session.Close();
            }
        }

        void Exit()
        {
            if (MessageBox.Show("确定停止服务并退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            this.FormClosing -= this.FrmMain_FormClosing;
            this.StopService();
            Application.Exit();
        }

        #endregion

        #region Events

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            actServiceState = (name, state) =>
            {
                this.lvService.Items[name].SubItems["State"].Text = state;
            };
            actLog = (name, level, log) =>
            {
                if (txtLog.Lines.Length > 1000)
                    txtLog.Clear();
                this.txtLog.AppendText(string.Format("{0} - [{1}]\r\n{2}:{3}\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), name, level, log));
            };
            this.InitViewerHost();
            this.tabServer.SelectedTab = this.tabLog;
            this.tabServer.SelectedTab = this.tabMain;
            this.clientList = new List<string>();
            this.lvClient.ListViewItemSorter = new ListViewItemComparer();
            this.InitService();
            this.tsStart.PerformClick();
            this.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.notifyIcon.ShowBalloonTip(100);
        }

        private void tsStart_Click(object sender, EventArgs e)
        {
            this.labState.Text = "正在启动...";
            this.statusStrip.Update();
            StartService();
            if (this.lvService.Items.Count > 0)
                this.lvService.Items[0].Selected = true;
            this.labState.Text = "服务正在运行...";
            this.statusStrip.Update();
            this.tsStart.Enabled = false;
            this.tsStop.Enabled = true;
        }

        private void tsStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定停止服务吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            StopService();
            this.labState.Text = "服务已停止";
            this.statusStrip.Update();
            this.tsStart.Enabled = true;
            this.tsStop.Enabled = false;
        }

        private void tsAddProtocol_Click(object sender, EventArgs e)
        {

        }

        private void tsRemoveProtocol_Click(object sender, EventArgs e)
        {

        }

        private void tsSetting_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSetting())
            {
                frm.ShowDialog(this);
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
            var currentIndex = this.lvService.SelectedItems[0].Index;
            if (selectedIndex == currentIndex)
                return;
            this.lvClient.Items.Clear();
            this.clientList.Clear();
            this.lvService.Items[currentIndex].ImageIndex = 0;
            if (selectedIndex != -1)
                this.lvService.Items[selectedIndex].ImageIndex = -1;
            this.selectedIndex = currentIndex;
            this.pgService.SelectedObject = this.lvService.Items[selectedIndex].Tag;
            this.selectedService = bootstrap.AppServers.FirstOrDefault(p => p.Name == lvService.Items[selectedIndex].Name) as IAppServer<IAppSession>;
            this.UpdateClients();
            this.btnServicePanel.Enabled = this.selectedService as IServiceUI != null;
        }

        private void btnServicePanel_Click(object sender, EventArgs e)
        {
            var ui = selectedService as IServiceUI;
            if (ui == null)
                return;
            using (var frm = Activator.CreateInstance(ui.UIType, new object[] { ui }) as Form)
            {
                if (frm != null)
                    frm.ShowDialog(this);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.DisconnectClients();
        }

        private void timerServerState_Tick(object sender, EventArgs e)
        {
            if (this.selectedIndex == -1)
                return;
            UpdateClients();
            this.pgService.Refresh();
            //this.viewerHost.SendMessageAsync(DateTime.Now.Ticks + "," + si.connCount + "," + recvRate + "," + sendRate);
        }

        private void tabServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.viewerHost.Visible = this.tabServer.SelectedTab == this.tabPerformance;
            if (this.tabServer.SelectedTab == this.tabLog)
                this.txtLog.ScrollToCaret();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        #endregion

    }
}
