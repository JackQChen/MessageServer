using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MessageLib;
using MessageLib.Logging;
using MessageServer.Controls;
using MessageServer.Extension;

namespace MessageServer
{
    public partial class FrmMain : Form
    {

        Logger logger;
        Dictionary<string, TcpServer> serviceList;
        SortedList<int, ListViewItem> clientList;
        int selectedIndex = -1;
        TcpServer selectedService;
        Action<string, string> actServiceState;
        Action<string, string, string> actLog;
        ViewerHost viewerHost;

        public FrmMain()
        {
            InitializeComponent();
        }

        #region Services

        private void InitializeService()
        {
            var serviceConfig = (ServiceConfig)ConfigurationManager.GetSection("ServiceConfig");
            foreach (Config config in serviceConfig.Configs)
            {
                TcpServer service = null;
                try
                {
                    service = Activator.CreateInstance(Type.GetType(config.Type)) as TcpServer;
                    service.Name = config.Name;
                    service.Endpoint.Port = config.Port;
                    service.OptionBacklog = config.Backlog;
                    service.OptionMaxConnectionCount = config.MaxConnectionCount;
                    service.Initialize();
                    this.serviceList[config.Name] = service;
                }
                catch (Exception ex)
                {
                    Log(config.Name, LogLevel.Error, ex.Message + (ex.InnerException == null ? string.Empty : "\r\n" + ex.InnerException.Message));
                    continue;
                }
                ListViewItem serviceItem = this.lvService.Items.Add(config.Name, "TCP", -1);
                serviceItem.SubItems.Add(config.Port.ToString());
                serviceItem.SubItems.Add(config.Name);
                serviceItem.SubItems.Add("准备就绪").Name = "State";
                serviceItem.Tag = new ServiceInfo(service);
            }
        }

        private void StartService()
        {
            foreach (var service in this.serviceList)
            {
                try
                {
                    service.Value.Start();
                }
                catch (Exception ex)
                {
                    Log(service.Key, LogLevel.Error, string.Format("服务启动失败:{0}", ex.Message));
                    service.Value.Stop();
                }
            }
            this.UpdateServiceState();
        }

        private void StopService()
        {
            foreach (var service in this.serviceList.Values)
                service.Stop();
            this.UpdateServiceState();
        }

        private void UpdateServiceState()
        {
            foreach (var service in this.serviceList)
            {
                switch (service.Value.IsStarted)
                {
                    case true:
                        this.lvService.Invoke(actServiceState, service.Key, "正在运行");
                        break;
                    default:
                        this.lvService.Invoke(actServiceState, service.Key, "已停止");
                        break;
                }
            }
        }

        #endregion

        #region Functions

        void Log(string name, string level, string log)
        {
            if (level == LogLevel.Error)
                this.logger.ErrorFormat("[{0}]{1}", name, log);
            else
                this.logger.InfoFormat("[{0}]{1}", name, log);
            this.txtLog.BeginInvoke(actLog, name, level, log);
        }

        void InitializeViewerHost()
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

        void UpdateClientList()
        {
            var sessionIds = this.selectedService.GetSessionIds();
            var clientIds = this.clientList.Keys;
            var removeList = clientIds.Except(sessionIds).ToArray();
            var addList = sessionIds.Except(clientIds).ToArray();
            if (removeList.Length == 0 && addList.Length == 0)
                return;
            foreach (var id in removeList)
                this.clientList.Remove(id);
            foreach (var id in addList)
            {
                var ip = "";
                try
                {
                    ip = this.selectedService.GetSession(id).Socket.RemoteEndPoint.ToString();
                }
                catch { }
                var item = new ListViewItem(new string[] { id.ToString(), ip, "已连接" }) { Name = id.ToString() };
                this.clientList.Add(id, item);
            }
            this.lvClient.VirtualListSize = sessionIds.Count();
        }

        void DisconnectClients()
        {
            foreach (int index in this.lvClient.SelectedIndices)
            {
                var session = this.selectedService.GetSession(this.clientList.ElementAt(index).Key);
                if (session != null)
                    session.Disconnect();
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
                this.txtLog.AppendText(string.Format("{0} - [{1}]\r\n[{2}]{3}\r\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), name, level, log));
            };
            this.InitializeViewerHost();
            this.tabServer.SelectedTab = this.tabLog;
            this.tabServer.SelectedTab = this.tabMain;
            this.logger = new Logger("Server");
            this.serviceList = new Dictionary<string, TcpServer>();
            this.clientList = new SortedList<int, ListViewItem>();
            this.InitializeService();
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

        private void tsAddService_Click(object sender, EventArgs e)
        {

        }

        private void tsRemoveService_Click(object sender, EventArgs e)
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
            this.clientList.Clear();
            this.lvClient.VirtualListSize = 0;
            this.lvService.Items[currentIndex].ImageIndex = 0;
            if (selectedIndex != -1)
                this.lvService.Items[selectedIndex].ImageIndex = -1;
            this.selectedIndex = currentIndex;
            this.pgService.SelectedObject = this.lvService.Items[selectedIndex].Tag;
            this.selectedService = this.serviceList[lvService.Items[selectedIndex].Name];
            this.btnServicePanel.Enabled = this.selectedService as IServiceUI != null;
        }

        private void lvClient_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = clientList.ElementAt(e.ItemIndex).Value;
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
            this.UpdateClientList();
            this.pgService.Refresh();
            var info = this.pgService.SelectedObject as ServiceInfo;
            info.Update();
            if (this.viewerHost.IsStarted)
            {
                this.viewerHost.UpdateAsync(new ViewerHost.UpdateInfo
                {
                    CollectedTime = info.CollectedTime,
                    ConnectionCount = info.CurrentConnectionCount,
                    SendingSpeed = info.SendingSpeed,
                    ReceivingSpeed = info.ReceivingSpeed
                });
            }
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
