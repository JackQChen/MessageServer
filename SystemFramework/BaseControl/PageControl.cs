
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
namespace SystemFramework.BaseControl
{
    [ToolboxItem(false)]
    public class PageControl : XtraForm
    {
        private Type _pageType;
        private WorkPlat _ownPlat;
        private System.Windows.Forms.ContextMenuStrip Unable;
        private System.ComponentModel.IContainer components;
        private List<object> _list = new List<object>();

        static PageControl()
        {
            typeof(DevExpress.Data.CurrencyDataController).GetProperty("DisableThreadingProblemsDetection"
                 , System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .SetValue(null, true, null);
        }

        public virtual void UpdatePriv(List<string> privList)
        {
        }

        /// <summary>
        /// 需要保存配置的控件
        /// </summary>
        public List<object> LayoutList
        {
            get { return _list; }
            set { _list = value; }
        }

        public WorkPlat OwnPlat
        {
            get { return _ownPlat; }
            set { _ownPlat = value; }
        }

        /// <summary>
        /// 显示页类型
        /// </summary>
        public Type PageType
        {
            get
            {
                return _pageType;
            }
            set
            {
                _pageType = value;
            }
        }

        /// <summary>
        /// 是否为对话框模式
        /// </summary>
        public bool DialogMode { get; set; }

        public PageControl()
        {
            InitializeComponent();
        }

        private void PageControl_Shown(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            this.RefreshLayout();
        }

        public virtual void RefreshLayout()
        {
            FrmLayout.SetLayout(this.LayoutList, this.GetType());
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Unable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // Unable
            // 
            this.Unable.Name = "Unable";
            this.Unable.Size = new System.Drawing.Size(61, 4);
            // 
            // PageControl
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.ContextMenuStrip = this.Unable;
            this.Name = "PageControl";
            this.Shown += new System.EventHandler(this.PageControl_Shown);
            this.ResumeLayout(false);

        }

        public void RemovePage()
        {
            if (this._ownPlat != null)
                this._ownPlat.RemovePage(this._pageType);
        }

    }

    public class PageTextAttribute : Attribute
    {
        private string _text;

        /// <summary>
        /// 页文字
        /// </summary>
        public string PageText
        {
            get
            {
                return _text;
            }
        }

        public PageTextAttribute(string pageText)
        {
            _text = pageText;
        }
    }

    public class PagePrivAttribute : Attribute
    {
        private string _text;

        public string PagePriv
        {
            get { return _text; }
        }

        public PagePrivAttribute(string privText)
        {
            _text = privText;
        }

    }

}
