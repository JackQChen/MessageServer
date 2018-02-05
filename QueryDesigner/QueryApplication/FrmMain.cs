using System.Windows.Forms;
using QueryDataAccess;

namespace QueryLauncher
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            QueryDAO.DataType = System.Configuration.ConfigurationManager.AppSettings["dbType"];
            QueryDAO.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[QueryDAO.DataType].ConnectionString;
        }

        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.txtPath.Text = this.openFileDialog1.FileName;
            }
        }

        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            FormQueryResult formQueryResult = new FormQueryResult();
            formQueryResult.FileName = this.txtPath.Text;
            formQueryResult.ShowDialog();
        }
    }
}
