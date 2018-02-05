using System;
using System.Windows.Forms;

namespace QueryDesigner
{
    public partial class FormSetAliasName : Form
    {
        public FormSetAliasName()
        {
            InitializeComponent();
        }

        public string AliasName
        {
            get
            {
                return txtAliasName.Text;
            }
            set
            {
                txtAliasName.Text = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAliasName.Text))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
