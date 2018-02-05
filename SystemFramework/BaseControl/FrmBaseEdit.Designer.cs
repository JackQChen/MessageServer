namespace SystemFramework.BaseControl
{
    partial class FrmBaseEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaseEdit));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.MenuBar = new DevExpress.XtraBars.Bar();
            this.btnSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnUndo = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnLayout = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnHelp = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.StatusBar = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.MenuBar,
            this.StatusBar});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Images = this.imageList1;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnSave,
            this.btnUndo,
            this.btnHelp,
            this.btnClose,
            this.btnLayout,
            this.barStaticItem1});
            this.barManager.MaxItemId = 9;
            this.barManager.StatusBar = this.StatusBar;
            // 
            // MenuBar
            // 
            this.MenuBar.BarName = "菜单";
            this.MenuBar.DockCol = 0;
            this.MenuBar.DockRow = 0;
            this.MenuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.MenuBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUndo, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnLayout, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHelp, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClose, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.MenuBar.Text = "菜单";
            // 
            // btnSave
            // 
            this.btnSave.Caption = "保存(&S)";
            this.btnSave.Id = 0;
            this.btnSave.ImageIndex = 1;
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnUndo
            // 
            this.btnUndo.Caption = "撤销修改(&U)";
            this.btnUndo.Id = 1;
            this.btnUndo.ImageIndex = 7;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUndo_ItemClick);
            // 
            // btnLayout
            // 
            this.btnLayout.Caption = "设置布局(&L)";
            this.btnLayout.Id = 7;
            this.btnLayout.ImageIndex = 52;
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLayout_ItemClick);
            // 
            // btnHelp
            // 
            this.btnHelp.Caption = "帮助(&H)";
            this.btnHelp.Id = 2;
            this.btnHelp.ImageIndex = 27;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHelp_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "关闭(&C)";
            this.btnClose.Id = 3;
            this.btnClose.ImageIndex = 16;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // StatusBar
            // 
            this.StatusBar.BarName = "状态栏";
            this.StatusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.StatusBar.DockCol = 0;
            this.StatusBar.DockRow = 0;
            this.StatusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.StatusBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.StatusBar.OptionsBar.AllowQuickCustomization = false;
            this.StatusBar.OptionsBar.DrawDragBorder = false;
            this.StatusBar.OptionsBar.UseWholeRow = true;
            this.StatusBar.Text = "状态栏";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "就绪";
            this.barStaticItem1.Id = 8;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(689, 62);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 397);
            this.barDockControlBottom.Size = new System.Drawing.Size(689, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 62);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 335);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(689, 62);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 335);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "warning.png");
            this.imageList1.Images.SetKeyName(1, "accept.png");
            this.imageList1.Images.SetKeyName(2, "accept_page.png");
            this.imageList1.Images.SetKeyName(3, "add.png");
            this.imageList1.Images.SetKeyName(4, "add_page.png");
            this.imageList1.Images.SetKeyName(5, "add_to_folder.png");
            this.imageList1.Images.SetKeyName(6, "attachment.png");
            this.imageList1.Images.SetKeyName(7, "back.png");
            this.imageList1.Images.SetKeyName(8, "block.png");
            this.imageList1.Images.SetKeyName(9, "calendar.png");
            this.imageList1.Images.SetKeyName(10, "calendar_empty.png");
            this.imageList1.Images.SetKeyName(11, "chart.png");
            this.imageList1.Images.SetKeyName(12, "chart_pie.png");
            this.imageList1.Images.SetKeyName(13, "clock.png");
            this.imageList1.Images.SetKeyName(14, "comment.png");
            this.imageList1.Images.SetKeyName(15, "comments.png");
            this.imageList1.Images.SetKeyName(16, "delete.png");
            this.imageList1.Images.SetKeyName(17, "delete_folder.png");
            this.imageList1.Images.SetKeyName(18, "delete_page.png");
            this.imageList1.Images.SetKeyName(19, "download.png");
            this.imageList1.Images.SetKeyName(20, "favorite.png");
            this.imageList1.Images.SetKeyName(21, "folder.png");
            this.imageList1.Images.SetKeyName(22, "folder_accept.png");
            this.imageList1.Images.SetKeyName(23, "folder_full.png");
            this.imageList1.Images.SetKeyName(24, "full_page.png");
            this.imageList1.Images.SetKeyName(25, "heart.png");
            this.imageList1.Images.SetKeyName(26, "help.png");
            this.imageList1.Images.SetKeyName(27, "info.png");
            this.imageList1.Images.SetKeyName(28, "lock.png");
            this.imageList1.Images.SetKeyName(29, "mail.png");
            this.imageList1.Images.SetKeyName(30, "mail_lock.png");
            this.imageList1.Images.SetKeyName(31, "mail_receive.png");
            this.imageList1.Images.SetKeyName(32, "mail_search.png");
            this.imageList1.Images.SetKeyName(33, "mail_send.png");
            this.imageList1.Images.SetKeyName(34, "new_page.png");
            this.imageList1.Images.SetKeyName(35, "next.png");
            this.imageList1.Images.SetKeyName(36, "page_process.png");
            this.imageList1.Images.SetKeyName(37, "process.png");
            this.imageList1.Images.SetKeyName(38, "promotion.png");
            this.imageList1.Images.SetKeyName(39, "protection.png");
            this.imageList1.Images.SetKeyName(40, "refresh.png");
            this.imageList1.Images.SetKeyName(41, "rss.png");
            this.imageList1.Images.SetKeyName(42, "search.png");
            this.imageList1.Images.SetKeyName(43, "search_page.png");
            this.imageList1.Images.SetKeyName(44, "tag_blue.png");
            this.imageList1.Images.SetKeyName(45, "tag_green.png");
            this.imageList1.Images.SetKeyName(46, "text_page.png");
            this.imageList1.Images.SetKeyName(47, "unlock.png");
            this.imageList1.Images.SetKeyName(48, "user.png");
            this.imageList1.Images.SetKeyName(49, "users.png");
            this.imageList1.Images.SetKeyName(50, "Edit.png");
            this.imageList1.Images.SetKeyName(51, "Print.png");
            this.imageList1.Images.SetKeyName(52, "Layout.png");
            // 
            // FrmBaseEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 420);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmBaseEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标题";
            this.Load += new System.EventHandler(this.FrmBase_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBaseEdit_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar MenuBar;
        protected DevExpress.XtraBars.BarLargeButtonItem btnSave;
        protected DevExpress.XtraBars.BarLargeButtonItem btnUndo;
        protected DevExpress.XtraBars.BarLargeButtonItem btnHelp;
        protected DevExpress.XtraBars.BarLargeButtonItem btnClose;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.Bar StatusBar;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        protected DevExpress.XtraBars.BarLargeButtonItem btnLayout;
    }
}