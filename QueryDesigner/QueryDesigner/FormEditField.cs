using System;
using System.Windows.Forms;
using SnControl;
using System.Collections.Generic;

namespace QueryDesigner
{
    public partial class FormEditField : Form
    {
        public FormEditField()
        {
            InitializeComponent();
        }

        public string FieldName
        {
            set
            {
                txtFieldName.Text = value;
            }
        }

        public string FieldType
        {
            get
            {
                return this.txtFieldType.Text;
            }
            set
            {
                txtFieldType.Text = value;
            }
        }

        public string FieldChineseName
        {
            get
            {
                return txtChineseName.Text;
            }
            set
            {
                txtChineseName.Text = value;
            }
        }

        public string CalcType
        {
            get
            {
                return cboCalcType.Text;
            }
            set
            {
                cboCalcType.Text = value;
            }
        }

        public List<StyleFormat> StyleFormat
        {
            get;
            set;
        }

        public string Converge
        {
            get
            {
                return comConverge.Text;
            }
            set
            {
                comConverge.Text = value;
            }
        }

        public string DisplayWidth
        {
            get
            {
                return displayWidth.Text;
            }
            set
            {
                displayWidth.Text = value;
            }
        }

        public int DecimalDigits
        {
            get
            {
                return Convert.ToInt32(txtDigits.Text);
            }
            set
            {
                txtDigits.Text = value.ToString();
            }
        }

        public bool FieldVisible
        {
            get
            {
                return this.chkVisible.Checked;
            }
            set
            {
                this.chkVisible.Checked = value;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtChineseName.Text))
            {
                MessageBox.Show("中文名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(cboCalcType.Text))
            {
                MessageBox.Show("统计类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> temp = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                temp.Add(i.ToString());
            }

            if (txtFieldType.Text == "DECIMAL" && (string.IsNullOrEmpty(txtDigits.Text) || !temp.Contains(txtDigits.Text)))
            {
                MessageBox.Show("小数位数格式不对（正确格式：0-6）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnStyle_Click(object sender, EventArgs e)
        {
            if (StyleFormat.Count <= 0)
            {
                MessageBox.Show("此列还没有样式，不能编辑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormStyle form = new FormStyle();

            StyleFormat[] s = new StyleFormat[StyleFormat.Count];
            StyleFormat.CopyTo(s);
            form.StyleFormat = new System.Collections.Generic.List<StyleFormat>();

            foreach (StyleFormat st in s)
            {
                form.StyleFormat.Add(st);
            }

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                StyleFormat = form.StyleFormat;
            }
        }

        private void FormEditField_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (string.IsNullOrEmpty(cboCalcType.Text))
            {
                cboCalcType.SelectedIndex = 0;
            }

            if (string.IsNullOrEmpty(comConverge.Text))
            {
                comConverge.SelectedIndex = 0;
            }
        }

        private void btnAddStyle_Click(object sender, EventArgs e)
        {
            FormNewStyle form = new FormNewStyle();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                StyleFormat.Add(form.StyleFormat);
            }
        }
    }
}
