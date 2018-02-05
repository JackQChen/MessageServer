using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemFramework.BaseControl;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace SystemFramework.BaseControl
{
    public class FrmPage : PageControl
    {
        #region Designer

        private BarManager barManager;
        private System.ComponentModel.IContainer components;
        private Bar bar1;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        protected BarLargeButtonItem btnSave;
        protected BarLargeButtonItem btnUndo;
        protected BarLargeButtonItem btnClose;
        private System.Windows.Forms.ImageList imageList1;
        protected BarLargeButtonItem btnDelete;
        private BarLargeButtonItem btnLayout;
        private BarLargeButtonItem btnHelp;
        protected BarLargeButtonItem btnRefresh;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPage));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnUndo = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnRefresh = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnLayout = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnHelp = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // bar
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Images = this.imageList1;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnSave,
            this.btnUndo,
            this.btnClose,
            this.btnDelete,
            this.btnRefresh,
            this.btnLayout,
            this.btnHelp});
            this.barManager.MainMenu = this.bar1;
            this.barManager.MaxItemId = 7;
            // 
            // bar1
            // 
            this.bar1.BarName = "菜单栏";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUndo, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRefresh, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnLayout, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnHelp, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClose, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "菜单栏";
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
            this.btnUndo.Caption = "撤销(&U)";
            this.btnUndo.Id = 1;
            this.btnUndo.ImageIndex = 7;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUndo_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "删除选中行(&D)";
            this.btnDelete.Id = 3;
            this.btnDelete.ImageIndex = 18;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Caption = "刷新(&R)";
            this.btnRefresh.Id = 4;
            this.btnRefresh.ImageIndex = 40;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // btnLayout
            // 
            this.btnLayout.Caption = "设置布局(&L)";
            this.btnLayout.Id = 5;
            this.btnLayout.ImageIndex = 52;
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLayout_ItemClick);
            // 
            // btnHelp
            // 
            this.btnHelp.Caption = "帮助(&H)";
            this.btnHelp.Id = 6;
            this.btnHelp.ImageIndex = 27;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHelp_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "关闭(&C)";
            this.btnClose.Id = 2;
            this.btnClose.ImageIndex = 16;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Name = "barDockControlTop";
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(703, 55);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.Name = "barDockControlBottom";
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 411);
            this.barDockControlBottom.Size = new System.Drawing.Size(703, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.Name = "barDockControlLeft";
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 55);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 356);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.Name = "barDockControlRight";
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(703, 55);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 356);
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
            // FrmPage
            // 
            this.ClientSize = new System.Drawing.Size(703, 411);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmPage";
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public FrmPage()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.OwnPlat != null)
                this.barManager.Form = this.OwnPlat.ParentForm;
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Save();
        }

        private void btnUndo_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Undo();
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Delete();
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Query();
        }

        private void btnLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmLayout frm = new FrmLayout();
            frm.ControlType = this.GetType();
            frm.LayoutList = this.LayoutList;
            frm.ShowDialog(this);
        }

        private void btnHelp_ItemClick(object sender, ItemClickEventArgs e)
        {
            string filePath = Application.StartupPath + "\\用户手册.pdf";
            if (File.Exists(filePath))
                Process.Start(filePath);
            else
                MessageBoxEx.Show("帮助文档不存在，请检查文件", "提示", MessageBoxIcon.Information);
        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.barManager.Dispose();
            this.barManager = null;
            this.Close();
            this.RemovePage();
        }


        protected virtual void Save()
        {
        }

        protected virtual void Undo()
        {
        }

        protected virtual void Delete()
        {
        }

        protected virtual void Query()
        {
        }
    }
}
