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
            this.tsAddProtocol = new System.Windows.Forms.ToolStripButton();
            this.tsRemoveProtocol = new System.Windows.Forms.ToolStripButton();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvService = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvClient = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.tsAddProtocol,
            this.tsRemoveProtocol,
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
            // tsAddProtocol
            // 
            this.tsAddProtocol.Image = global::MessageServer.Properties.Resources.Add;
            this.tsAddProtocol.Name = "tsAddProtocol";
            this.tsAddProtocol.Size = new System.Drawing.Size(76, 45);
            this.tsAddProtocol.Text = "添加服务(&A)";
            this.tsAddProtocol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsAddProtocol.Click += new System.EventHandler(this.tsAddProtocol_Click);
            // 
            // tsRemoveProtocol
            // 
            this.tsRemoveProtocol.Image = global::MessageServer.Properties.Resources.Remove;
            this.tsRemoveProtocol.Name = "tsRemoveProtocol";
            this.tsRemoveProtocol.Size = new System.Drawing.Size(76, 45);
            this.tsRemoveProtocol.Text = "移除服务(&R)";
            this.tsRemoveProtocol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsRemoveProtocol.Click += new System.EventHandler(this.tsRemoveProtocol_Click);
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
            this.tabMain.Controls.Add(this.splitContainer1);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(776, 466);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "服务";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(770, 460);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvService);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 460);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务列表";
            // 
            // lvService
            // 
            this.lvService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvService.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
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
            // columnHeader1
            // 
            this.columnHeader1.Text = "  类型";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "端口";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "名称";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 64;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pgService);
            this.splitContainer2.Size = new System.Drawing.Size(486, 460);
            this.splitContainer2.SplitterDistance = 264;
            this.splitContainer2.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvClient);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 460);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "客户端列表";
            // 
            // lvClient
            // 
            this.lvClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvClient.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvClient.FullRowSelect = true;
            this.lvClient.HideSelection = false;
            this.lvClient.Location = new System.Drawing.Point(3, 17);
            this.lvClient.Name = "lvClient";
            this.lvClient.Size = new System.Drawing.Size(258, 392);
            this.lvClient.TabIndex = 3;
            this.lvClient.UseCompatibleStateImageBehavior = false;
            this.lvClient.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ID";
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "IP";
            this.columnHeader6.Width = 140;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "状态";
            this.columnHeader7.Width = 50;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnServicePanel);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 409);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 48);
            this.panel1.TabIndex = 4;
            // 
            // btnServicePanel
            // 
            this.btnServicePanel.Enabled = false;
            this.btnServicePanel.Location = new System.Drawing.Point(36, 13);
            this.btnServicePanel.Name = "btnServicePanel";
            this.btnServicePanel.Size = new System.Drawing.Size(75, 23);
            this.btnServicePanel.TabIndex = 5;
            this.btnServicePanel.Text = "服务面板";
            this.btnServicePanel.UseVisualStyleBackColor = true;
            this.btnServicePanel.Click += new System.EventHandler(this.btnServicePanel_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(137, 13);
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton tsAddProtocol;
        private System.Windows.Forms.ToolStripButton tsRemoveProtocol;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labState;
        private System.Windows.Forms.ListView lvClient;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ListView lvService;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.PropertyGrid pgService;
        private System.Windows.Forms.GroupBox groupBox2;
        private MessageServer.Controls.TabControlEx tabServer;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ColumnHeader columnHeader2;
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
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ImageList imageList;
    }
}

