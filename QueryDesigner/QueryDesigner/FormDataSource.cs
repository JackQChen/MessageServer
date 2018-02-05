using System.Windows.Forms;
using QueryDataAccess;

namespace QueryDesigner
{
    public partial class FormDataSource : Form
    {
        public FormDataSource()
        {
            InitializeComponent();
        }

        public string DataSetID
        {
            get
            {
                return txtDataSetID.Text;
            }
            set
            {
                txtDataSetID.Text = value;
            }
        }

        public string DataSetChineseName
        {
            get
            {
                return txtDataSetName.Text;
            }
            set
            {
                txtDataSetName.Text = value;
            }
        }

        public int UseType
        {
            get
            {
                return cboUserType.SelectedIndex;
            }
            set
            {
                cboUserType.SelectedIndex = value;
            }
        }

        public string ReportPath
        {
            get
            {
                return txtReportPath.Text;
            }
            set
            {
                txtReportPath.Text = value;
            }
        }

        private void cboUserType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboUserType.Text == "存储过程")
            {
                this.label2.Text = "存储过程名称";
                this.txtDataSetName.Text = string.Empty;
            }
            else
            {
                this.label2.Text = "中文名称";
            }
        }

        private void btnConfirm_Click(object sender, System.EventArgs e)
        {
            if (txtDataSetName.Text == string.Empty)
            {
                MessageBox.Show("中文名称不能为空");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
