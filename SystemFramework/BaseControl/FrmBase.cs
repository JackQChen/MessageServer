
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid;

namespace SystemFramework.BaseControl
{
    public partial class FrmBase : PageControl
    {
        private bool editable = true;

        public FrmBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public virtual bool Editable
        {
            get
            {
                return editable;
            }
            set
            {
                if (!value)
                {
                    this.MenuBar.Visible = false;
                    this.Height -= 60;
                    editable = false;
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.OwnPlat != null)
                this.barManager.Form = this.OwnPlat.ParentForm;
        }

        private void btnLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmLayout frm = new FrmLayout();
            frm.ControlType = this.GetType();
            frm.LayoutList = this.LayoutList;
            frm.ShowDialog(this);
        }

        public virtual void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string filePath = Application.StartupPath + "\\用户手册.pdf";
            if (File.Exists(filePath))
                Process.Start(filePath);
            else
                MessageBoxEx.Show("帮助文档不存在，请检查文件", "提示", MessageBoxIcon.Information);
        }

        protected virtual void Add()
        {
        }

        protected virtual void Edit()
        {
        }

        protected virtual void Delete()
        {
        }

        protected virtual void Query()
        {
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barManager.Dispose();
            this.barManager = null;
            this.Close();
            this.RemovePage();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Add();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Edit();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Delete();
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Query();
        }

    }
}