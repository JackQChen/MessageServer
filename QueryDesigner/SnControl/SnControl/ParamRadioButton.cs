using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace SnControl
{
    public class ParamRadioButton : RadioButton, ICommonAttribute
    {
        public ParamRadioButton()
        {
        }

        public string BindingData { get; set; }

        #region ICommonAttribute 成员

        [Description("数据集合"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string DataSetName { get; set; }

        [Description("执行函数"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string Function { get; set; }

        private string _paramName = "";

        [Description("参数名称"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string ParamName
        {
            get
            {
                if (this.DesignMode)
                    return _paramName;
                return this.Checked ? _paramName : "";
            }
            set { _paramName = value; }
        }

        [Description("参数类型"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        public string ParamType { get; set; }

        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.BindingData))
                    return "";
                else
                    return this.BindingData;
            }
        }

        #endregion
    }
}
