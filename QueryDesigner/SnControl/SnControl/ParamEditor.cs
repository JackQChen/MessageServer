namespace SnControl
{
    //using Skynet.Framework.Common;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;
    using System.Windows.Forms;

    public class ParamEditor : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                if (context.Instance.GetType().ToString() == "System.Object[]")
                {
                    MessageBox.Show("非法操作！不能同时设置多个变参控件！","错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                if (SolutionInstance.GetInstance().Solution.DataSetList.Count == 0)
                {
                    MessageBox.Show("请先设置数据集合！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                this.edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (this.edSvc != null)
                {
                    FrmParamProp paramProp = new FrmParamProp();
                    paramProp.QSolution = SolutionInstance.GetInstance().Solution;
                    paramProp.ChangeFrom = ChangeType.PropGrid;
                    this.SetEditorProps(context.Instance, paramProp);
                    this.edSvc.ShowDialog(paramProp);
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        protected virtual void SetEditorProps(object control, FrmParamProp ParamProp)
        {
            ParamProp.Control = control;
        }
    }
}

