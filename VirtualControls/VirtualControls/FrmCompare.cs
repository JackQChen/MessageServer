using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualControls
{
    public partial class FrmCompare : Form
    {
        public FrmCompare()
        {
            InitializeComponent();
        }

        private void FrmCompare_Load(object sender, EventArgs e)
        {
            this.vcc1.BackColor = Color.Transparent;
            this.vcc1.Location = Point.Empty;
            this.vcc1.Width = this.Width;
            this.vcc2.BackColor = Color.Transparent;
            this.vcc2.Location = Point.Empty;
            this.vcc2.Width = this.Width;

            for (int i = 0; i < 100; i++)
            {
                var btn = new Button();
                btn.Bounds = new Rectangle((i % 10) * 50, (i / 10) * 30, 48, 28);
                btn.Text = string.Format("按钮({0})", i);
                this.vcc1.Controls.Add(btn);
            }
            //this.vcc1.Refresh();

            for (int i = 0; i < 100; i++)
            {
                var btn = new Button();
                btn.Bounds = new Rectangle((i % 10) * 60, (i / 10) * 40, 58, 38);
                btn.Text = string.Format("Btn({0})", i);
                this.vcc2.Controls.Add(btn);
            }
            this.vcc1.BringToFront();

        }

        bool show = false;

        private void button1_Click(object sender, EventArgs e)
        {
            show = !show;
            if (show)
                this.vcc2.BringToFront();
            else
                this.vcc1.BringToFront();
        }
    }
}
