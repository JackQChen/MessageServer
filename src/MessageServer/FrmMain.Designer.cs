using MessageServer.Controls;

namespace MessageServer
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsStart = new System.Windows.Forms.ToolStripButton();
            this.tsStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsAddService = new System.Windows.Forms.ToolStripButton();
            this.tsRemoveService = new System.Windows.Forms.ToolStripButton();
            this.tsSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labState = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerServerState = new System.Windows.Forms.Timer(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tabServer = new MessageServer.Controls.TabControlEx();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.scService = new System.Windows.Forms.SplitContainer();
            this.gbService = new System.Windows.Forms.GroupBox();
            this.lvService = new System.Windows.Forms.ListView();
            this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scClient = new System.Windows.Forms.SplitContainer();
            this.gbClient = new System.Windows.Forms.GroupBox();
            this.lvClient = new System.Windows.Forms.ListView();
            this.clmID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmClientState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.plServicePanel = new System.Windows.Forms.Panel();
            this.btnServicePanel = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.pgService = new System.Windows.Forms.PropertyGrid();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tabPerformance = new System.Windows.Forms.TabPage();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.tabServer.SuspendLayout();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scService)).BeginInit();
            this.scService.Panel1.SuspendLayout();
            this.scService.Panel2.SuspendLayout();
            this.scService.SuspendLayout();
            this.gbService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scClient)).BeginInit();
            this.scClient.Panel1.SuspendLayout();
            this.scClient.Panel2.SuspendLayout();
            this.scClient.SuspendLayout();
            this.gbClient.SuspendLayout();
            this.plServicePanel.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStart,
            this.tsStop,
            this.toolStripSeparator1,
            this.tsAddService,
            this.tsRemoveService,
            this.tsSetting,
            this.toolStripSeparator2,
            this.tsExit});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 48);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tsStart
            // 
            this.tsStart.Image = global::MessageServer.Properties.Resources.Run;
            this.tsStart.Name = "tsStart";
            this.tsStart.Size = new System.Drawing.Size(60, 45);
            this.tsStart.Text = "启动服务";
            this.tsStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsStart.Click += new System.EventHandler(this.tsStart_Click);
            // 
            // tsStop
            // 
            this.tsStop.Image = global::MessageServer.Properties.Resources.Stop;
            this.tsStop.Name = "tsStop";
            this.tsStop.Size = new System.Drawing.Size(60, 45);
            this.tsStop.Text = "停止服务";
            this.tsStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsStop.Click += new System.EventHandler(this.tsStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 48);
            // 
            // tsAddService
            // 
            this.tsAddService.Image = global::MessageServer.Properties.Resources.Add;
            this.tsAddService.Name = "tsAddService";
            this.tsAddService.Size = new System.Drawing.Size(76, 45);
            this.tsAddService.Text = "添加服务(&A)";
            this.tsAddService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsAddService.Click += new System.EventHandler(this.tsAddService_Click);
            // 
            // tsRemoveService
            // 
            this.tsRemoveService.Image = global::MessageServer.Properties.Resources.Remove;
            this.tsRemoveService.Name = "tsRemoveService";
            this.tsRemoveService.Size = new System.Drawing.Size(76, 45);
            this.tsRemoveService.Text = "移除服务(&R)";
            this.tsRemoveService.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsRemoveService.Click += new System.EventHandler(this.tsRemoveService_Click);
            // 
            // tsSetting
            // 
            this.tsSetting.Image = global::MessageServer.Properties.Resources.Setting;
            this.tsSetting.Name = "tsSetting";
            this.tsSetting.Size = new System.Drawing.Size(51, 45);
            this.tsSetting.Text = "设置(&S)";
            this.tsSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsSetting.Click += new System.EventHandler(this.tsSetting_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 48);
            // 
            // tsExit
            // 
            this.tsExit.Image = global::MessageServer.Properties.Resources.Exit;
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(52, 45);
            this.tsExit.Text = "退出(&X)";
            this.tsExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labState});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // labState
            // 
            this.labState.Name = "labState";
            this.labState.Size = new System.Drawing.Size(32, 17);
            this.labState.Text = "就绪";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "欢迎使用消息服务器!";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "欢迎使用消息服务器!";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMenuItem,
            this.splitMenuItem,
            this.exitMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(137, 54);
            // 
            // showMenuItem
            // 
            this.showMenuItem.Name = "showMenuItem";
            this.showMenuItem.Size = new System.Drawing.Size(136, 22);
            this.showMenuItem.Text = "显示主界面";
            this.showMenuItem.Click += new System.EventHandler(this.showMenuItem_Click);
            // 
            // splitMenuItem
            // 
            this.splitMenuItem.Name = "splitMenuItem";
            this.splitMenuItem.Size = new System.Drawing.Size(133, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(136, 22);
            this.exitMenuItem.Text = "退出";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // timerServerState
            // 
            this.timerServerState.Enabled = true;
            this.timerServerState.Interval = 1000;
            this.timerServerState.Tick += new System.EventHandler(this.timerServerState_Tick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "selected.png");
            // 
            // tabServer
            // 
            this.tabServer.Controls.Add(this.tabMain);
            this.tabServer.Controls.Add(this.tabLog);
            this.tabServer.Controls.Add(this.tabPerformance);
            this.tabServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabServer.Location = new System.Drawing.Point(0, 48);
            this.tabServer.Name = "tabServer";
            this.tabServer.SelectedIndex = 0;
            this.tabServer.Size = new System.Drawing.Size(784, 492);
            this.tabServer.TabIndex = 6;
            this.tabServer.SelectedIndexChanged += new System.EventHandler(this.tabServer_SelectedIndexChanged);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.scService);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(776, 466);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "服务";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // scService
            // 
            this.scService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scService.Location = new System.Drawing.Point(3, 3);
            this.scService.Name = "scService";
            // 
            // scService.Panel1
            // 
            this.scService.Panel1.Controls.Add(this.gbService);
            // 
            // scService.Panel2
            // 
            this.scService.Panel2.Controls.Add(this.scClient);
            this.scService.Size = new System.Drawing.Size(770, 460);
            this.scService.SplitterDistance = 280;
            this.scService.TabIndex = 6;
            // 
            // gbService
            // 
            this.gbService.Controls.Add(this.lvService);
            this.gbService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbService.Location = new System.Drawing.Point(0, 0);
            this.gbService.Name = "gbService";
            this.gbService.Size = new System.Drawing.Size(280, 460);
            this.gbService.TabIndex = 4;
            this.gbService.TabStop = false;
            this.gbService.Text = "服务列表";
            // 
            // lvService
            // 
            this.lvService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvService.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmType,
            this.clmPort,
            this.clmName,
            this.clmState});
            this.lvService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvService.FullRowSelect = true;
            this.lvService.HideSelection = false;
            this.lvService.Location = new System.Drawing.Point(3, 17);
            this.lvService.MultiSelect = false;
            this.lvService.Name = "lvService";
            this.lvService.Size = new System.Drawing.Size(274, 440);
            this.lvService.SmallImageList = this.imageList;
            this.lvService.TabIndex = 0;
            this.lvService.UseCompatibleStateImageBehavior = false;
            this.lvService.View = System.Windows.Forms.View.Details;
            this.lvService.SelectedIndexChanged += new System.EventHandler(this.lvService_SelectedIndexChanged);
            this.lvService.DoubleClick += new System.EventHandler(this.lvService_DoubleClick);
            // 
            // clmType
            // 
            this.clmType.Text = "  类型";
            // 
            // clmPort
            // 
            this.clmPort.Text = "端口";
            this.clmPort.Width = 50;
            // 
            // clmName
            // 
            this.clmName.Text = "名称";
            this.clmName.Width = 100;
            // 
            // clmState
            // 
            this.clmState.Text = "状态";
            this.clmState.Width = 64;
            // 
            // scClient
            // 
            this.scClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scClient.Location = new System.Drawing.Point(0, 0);
            this.scClient.Name = "scClient";
            // 
            // scClient.Panel1
            // 
            this.scClient.Panel1.Controls.Add(this.gbClient);
            // 
            // scClient.Panel2
            // 
            this.scClient.Panel2.Controls.Add(this.pgService);
            this.scClient.Size = new System.Drawing.Size(486, 460);
            this.scClient.SplitterDistance = 264;
            this.scClient.TabIndex = 7;
            // 
            // gbClient
            // 
            this.gbClient.Controls.Add(this.lvClient);
            this.gbClient.Controls.Add(this.plServicePanel);
            this.gbClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbClient.Location = new System.Drawing.Point(0, 0);
            this.gbClient.Name = "gbClient";
            this.gbClient.Size = new System.Drawing.Size(264, 460);
            this.gbClient.TabIndex = 0;
            this.gbClient.TabStop = false;
            this.gbClient.Text = "客户端列表";
            // 
            // lvClient
            // 
            this.lvClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvClient.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmID,
            this.clmIP,
            this.clmClientState});
            this.lvClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvClient.FullRowSelect = true;
            this.lvClient.HideSelection = false;
            this.lvClient.Location = new System.Drawing.Point(3, 17);
            this.lvClient.Name = "lvClient";
            this.lvClient.Size = new System.Drawing.Size(258, 392);
            this.lvClient.TabIndex = 3;
            this.lvClient.UseCompatibleStateImageBehavior = false;
            this.lvClient.View = System.Windows.Forms.View.Details;
            this.lvClient.VirtualMode = true;
            this.lvClient.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvClient_RetrieveVirtualItem);
            // 
            // clmID
            // 
            this.clmID.Text = "ID";
            this.clmID.Width = 50;
            // 
            // clmIP
            // 
            this.clmIP.Text = "IP";
            this.clmIP.Width = 140;
            // 
            // clmClientState
            // 
            this.clmClientState.Text = "状态";
            this.clmClientState.Width = 50;
            // 
            // plServicePanel
            // 
            this.plServicePanel.Controls.Add(this.btnServicePanel);
            this.plServicePanel.Controls.Add(this.btnDisconnect);
            this.plServicePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plServicePanel.Location = new System.Drawing.Point(3, 409);
            this.plServicePanel.Name = "plServicePanel";
            this.plServicePanel.Size = new System.Drawing.Size(258, 48);
            this.plServicePanel.TabIndex = 4;
            // 
            // btnServicePanel
            // 
            this.btnServicePanel.Enabled = false;
            this.btnServicePanel.Location = new System.Drawing.Point(41, 13);
            this.btnServicePanel.Name = "btnServicePanel";
            this.btnServicePanel.Size = new System.Drawing.Size(75, 23);
            this.btnServicePanel.TabIndex = 5;
            this.btnServicePanel.Text = "服务面板";
            this.btnServicePanel.UseVisualStyleBackColor = true;
            this.btnServicePanel.Click += new System.EventHandler(this.btnServicePanel_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(142, 13);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "断开连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // pgService
            // 
            this.pgService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgService.LineColor = System.Drawing.SystemColors.Control;
            this.pgService.Location = new System.Drawing.Point(0, 0);
            this.pgService.Name = "pgService";
            this.pgService.Size = new System.Drawing.Size(218, 460);
            this.pgService.TabIndex = 5;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(776, 466);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "日志";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(770, 460);
            this.txtLog.TabIndex = 0;
            // 
            // tabPerformance
            // 
            this.tabPerformance.Location = new System.Drawing.Point(4, 22);
            this.tabPerformance.Name = "tabPerformance";
            this.tabPerformance.Size = new System.Drawing.Size(776, 466);
            this.tabPerformance.TabIndex = 2;
            this.tabPerformance.Text = "性能";
            this.tabPerformance.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabServer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "消息服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.tabServer.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.scService.Panel1.ResumeLayout(false);
            this.scService.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scService)).EndInit();
            this.scService.ResumeLayout(false);
            this.gbService.ResumeLayout(false);
            this.scClient.Panel1.ResumeLayout(false);
            this.scClient.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scClient)).EndInit();
            this.scClient.ResumeLayout(false);
            this.gbClient.ResumeLayout(false);
            this.plServicePanel.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsStart;
        private System.Windows.Forms.ToolStripButton tsStop;
        private System.Windows.Forms.ToolStripButton tsSetting;
        private System.Windows.Forms.ToolStripButton tsAddService;
        private System.Windows.Forms.ToolStripButton tsRemoveService;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labState;
        private System.Windows.Forms.ListView lvClient;
        private System.Windows.Forms.ColumnHeader clmID;
        private System.Windows.Forms.ColumnHeader clmIP;
        private System.Windows.Forms.ColumnHeader clmClientState;
        private System.Windows.Forms.ListView lvService;
        private System.Windows.Forms.GroupBox gbService;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmState;
        private System.Windows.Forms.PropertyGrid pgService;
        private System.Windows.Forms.GroupBox gbClient;
        private MessageServer.Controls.TabControlEx tabServer;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.SplitContainer scService;
        private System.Windows.Forms.Panel plServicePanel;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ColumnHeader clmPort;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnServicePanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsExit;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMenuItem;
        private System.Windows.Forms.ToolStripSeparator splitMenuItem;
        private System.Windows.Forms.Timer timerServerState;
        private System.Windows.Forms.SplitContainer scClient;
        private System.Windows.Forms.ImageList imageList;
    }
}

