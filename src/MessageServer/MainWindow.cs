using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Loader;
using Gtk;
using MessageLib;
using MessageLib.Logging;
using MessageServer.Extension;
using MessageServer.Properties;
using UI = Gtk.Builder.ObjectAttribute;

namespace MessageServer
{
    public class MainWindow : Window
    {
        [UI] private ToolButton tbStart = null, tbStop = null, tbExit = null;
        [UI] private TreeView tvService = null, tvClient = null;
        [UI] private TextView txtLog = null, txtProp = null;
        [UI] private Label labState = null;

        Logger logger;
        Dictionary<string, TcpServer> serviceList;
        SortedList<int, TreeIter> clientList;
        int selectedIndex = -1;
        TcpServer selectedService;
        Action<string, string> actServiceState;
        Action<string, string, string> actLog;

        string strProp;
        Dictionary<string, ServiceInfo> serviceInfo;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
            this.Icon = new Gdk.Pixbuf(Resources.Icon);
            this.tbStart.IconWidget = new Image(new Gdk.Pixbuf(Resources.Run));
            this.tbStart.IconWidget.Visible = true;
            this.tbStop.IconWidget = new Image(new Gdk.Pixbuf(Resources.Stop));
            this.tbStop.IconWidget.Visible = true;
            this.tbExit.IconWidget = new Image(new Gdk.Pixbuf(Resources.Exit));
            this.tbExit.IconWidget.Visible = true;

            this.txtProp.AddTickCallback((w, f) =>
            {
                if (string.IsNullOrEmpty(strProp))
                    return true;
                ((TextView)w).Buffer.Text = strProp;
                return true;
            });
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            Shown += MainWindow_Shown;
            DeleteEvent += MainWindow_DeleteEvent;
            tvService.Selection.Changed += this.tvService_SelectedIndexChanged;

            new System.Timers.Timer(1000) { Enabled = true }.Elapsed += this.timerServerState_Tick;

            this.tbStart.Clicked += tbStart_Clicked;
            this.tbStop.Clicked += tbStop_Clicked;
            this.tbExit.Clicked += tbExit_Clicked;
        }

        #region Services

        ListStore InitialTreeView(TreeView treeView, string[] columns, int[] width)
        {
            ListStore listStore = new ListStore(typeof(string));
            var columnTypes = columns.Select(s => GLib.GType.String).ToArray();
            listStore.ColumnTypes = columnTypes;
            var cellRenderer = new CellRendererText();
            var index = 0;
            foreach (var column in columns)
            {
                var clm = new TreeViewColumn(column, cellRenderer);
                clm.FixedWidth = width[index];
                clm.AddAttribute(cellRenderer, "text", index++);
                treeView.AppendColumn(clm);
            }
            treeView.Model = listStore;
            return listStore;
        }

        private void InitializeService()
        {
            var listService = InitialTreeView(this.tvService, new string[] { "Type", "Port", "Name", "State" }, new int[] { 50, 50, 150, 60 });
            var serviceConfig = (ServiceConfig)ConfigurationManager.GetSection("ServiceConfig");
            foreach (Config config in serviceConfig.Configs)
            {
                TcpServer service = null;
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.Type.Split(',')[1] + ".dll"));
                    service = Activator.CreateInstance(System.Type.GetType(config.Type)) as TcpServer;
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
                listService.AppendValues("TCP", config.Port, config.Name, "Ready");
                this.serviceInfo[config.Name] = new ServiceInfo(service);
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
                        this.tvService.Model.Foreach((model, path, iter) =>
                       {
                           var serviceName = model.GetValue(iter, 2).ToString();
                           if (serviceName == service.Key)
                           {
                               model.SetValue(iter, 3, "Running");
                               return true;
                           }
                           else
                               return false;
                       });
                        break;
                    default:
                        this.tvService.Model.Foreach((model, path, iter) =>
                        {
                            var serviceName = model.GetValue(iter, 2).ToString();
                            if (serviceName == service.Key)
                            {
                                model.SetValue(iter, 3, "Stopped");
                                return true;
                            }
                            else
                                return false;
                        });
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
            //this.txtLog.BeginInvoke(actLog, name, level, log);
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
            {
                var iter = clientList[id];
                (this.tvClient.Model as ListStore).Remove(ref iter);
                this.clientList.Remove(id);
            }
            foreach (var id in addList)
            {
                var ip = "";
                try
                {
                    ip = this.selectedService.GetSession(id).Socket.RemoteEndPoint.ToString();
                }
                catch { }
                var iter = (this.tvClient.Model as ListStore).AppendValues(id, ip, "Connected");
                this.clientList.Add(id, iter);
            }
        }

        void DisconnectClients()
        {
            //foreach (int index in this.lvClient.SelectedIndices)
            //{
            //    var session = this.selectedService.GetSession(this.clientList.ElementAt(index).Key);
            //    if (session != null)
            //        session.Disconnect();
            //}
        }

        void Exit()
        {
            //if (MessageBox.Show("确定停止服务并退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //    return;
            //this.FormClosing -= this.FrmMain_FormClosing;
            this.StopService();
            Application.Quit();
        }

        #endregion

        #region Events

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            //actServiceState = (name, state) =>
            //{
            //    this.lvService.Items[name].SubItems["State"].Text = state;
            //};
            //actLog = (name, level, log) =>
            //{
            //    if (txtLog.Lines.Length > 1000)
            //        txtLog.Clear();
            //    this.txtLog.AppendText(string.Format("{0} - [{1}]\r\n[{2}]{3}\r\n",
            //        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), name, level, log));
            //};
            this.logger = new Logger("Server");
            this.serviceList = new Dictionary<string, TcpServer>();
            this.serviceInfo = new Dictionary<string, ServiceInfo>();
            this.clientList = new SortedList<int, TreeIter>();
            this.InitializeService();
            this.InitialTreeView(this.tvClient, new string[] { "ID", "IP", "State" }, new int[] { 60, 150, 80 });
            this.tbStart_Clicked(null, null);
            //this.Close();
        }

        private void MainWindow_DeleteEvent(object sender, DeleteEventArgs e)
        {
            Application.Quit();
        }

        //private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide();
        //    this.notifyIcon.ShowBalloonTip(100);
        //}

        private void tbStart_Clicked(object sender, EventArgs e)
        {
            this.labState.Text = "正在启动...";
            StartService();
            //if (this.lvService.Items.Count > 0)
            //    this.lvService.Items[0].Selected = true;
            this.labState.Text = "服务正在运行...";
            //this.tsStart.Enabled = false;
            //this.tsStop.Enabled = true;
        }

        private void tbStop_Clicked(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定停止服务吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            //    return;
            StopService();
            this.labState.Text = "服务已停止";
            //this.tbStart.Enabled = true;
            //this.tsStop.Enabled = false;
        }

        private void tsAddService_Click(object sender, EventArgs e)
        {

        }

        private void tsRemoveService_Click(object sender, EventArgs e)
        {

        }

        private void tsSetting_Click(object sender, EventArgs e)
        {
        }

        private void tbExit_Clicked(object sender, EventArgs e)
        {
            Exit();
        }

        private void lvService_DoubleClick(object sender, EventArgs e)
        {
            //this.btnServicePanel.PerformClick();
        }

        private void tvService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tvService.Selection.CountSelectedRows() != 1)
                return;
            TreeIter iRow;
            this.tvService.Selection.GetSelected(out iRow);
            var serviceName = this.tvService.Model.GetValue(iRow, 2).ToString();
            var currentIndex = this.serviceList.Keys.ToList().IndexOf(serviceName);
            if (selectedIndex == currentIndex)
                return;
            this.clientList.Clear();
            (this.tvClient.Model as ListStore).Clear();
            this.selectedIndex = currentIndex;
            this.selectedService = this.serviceList[serviceName];
        }

        private void btnServicePanel_Click(object sender, EventArgs e)
        {
            var ui = selectedService as IServiceUI;
            if (ui == null)
                return;
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
            var info = this.serviceInfo[this.selectedService.Name];
            info.Update();
            this.strProp = $@"----------------------------------------
Backlog:                             {info.Backlog}
SendBufferSize:                  {info.SendBufferSize}
ReceiveBufferSize:              {info.ReceiveBufferSize}
----------------------------------------
CurrentConnectionCount:  {info.CurrentConnectionCount}
MaxConnectionCount:       {info.MaxConnectionCount}
----------------------------------------
SendingSpeed:                   {info.SendingSpeedText}
ReceivingSpeed:                 {info.ReceivingSpeedText}
----------------------------------------
TotalSentSize:                     {info.TotalSentSize}
TotalReceivedSize:              {info.TotalReceivedSize}
";
        }

        private void tabServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.tabServer.SelectedTab == this.tabLog)
            //    this.txtLog.ScrollToCaret();
        }

        //private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    this.Show();
        //}

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
