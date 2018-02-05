using System;
using System.Windows.Forms;
using Register;

namespace Client
{
    public partial class FrmClient : Form
    {
        public FrmClient()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Register.Validate.Check())
                this.Text = "校验成功";
        }
    }
}
