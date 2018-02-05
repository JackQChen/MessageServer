using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryDesigner
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            labName.Text = string.Format("改版人：{0}", global::QueryDesigner.Properties.Resources.String1);
        }
    }
}
