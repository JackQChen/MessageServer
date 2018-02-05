
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;

namespace SystemFramework.BaseControl
{
    public partial class FrmBaseEdit : XtraForm
    {
        private bool editable = true;
        private List<object> _list = new List<object>();

        /// <summary>
        /// 需要保存配置的控件
        /// </summary>
        public List<object> LayoutList
        {
            get { return _list; }
            set { _list = value; }
        }

        public FrmBaseEdit()
        {
            InitializeComponent();
        }

        private void FrmBase_Load(object sender, EventArgs e)
        {
            FrmLayout.SetLayout(this.LayoutList, this.GetType());
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

        protected virtual void Save()
        {
        }

        protected virtual void Undo()
        {
        }

        protected virtual void FrmBaseEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Save();
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Undo();
        }

    }
}