namespace SnControl
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;

    public partial class ParamTextBox : TextBox, ICommonAttribute
    {
        private Container components;

        public ParamTextBox()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ParamTextBox(IContainer container)
        {
            this.components = null;
            container.Add(this);
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }


        #region ICommonAttribute 成员

        [Description("数据集合"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string DataSetName { get; set; }

        [Description("执行函数"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string Function { get; set; }

        [Description("参数名称"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string ParamName { get; set; }

        [Description("参数类型"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string ParamType { get; set; }

        public string Value { get { return this.Text; } }

        #endregion
    }
}

