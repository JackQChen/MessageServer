using System;
using System.Windows.Forms;

namespace QueryDesigner
{
    public partial class FormNewProject : Form
    {
        public FormNewProject()
        {
            InitializeComponent();
        }

        public string ProjectID
        {
            get { return txtProject.Text; }
            set { txtProject.Text = value; }
        }

        public string ProjectDepict
        {
            get { return txtDepict.Text; }
            set { txtDepict.Text = value; }
        }

        public string ProjectMemo
        {
            get { return txtMemo.Text; }
            set { txtMemo.Text = value; }
        }

        private void FormNewProject_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SelectNextControl(ActiveControl, true, true, true, false);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProject.Text))
            {
                MessageBox.Show("请输入查询方案！","警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtProject.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtDepict.Text))
            {
                MessageBox.Show("请输入方案描述！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDepict.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
