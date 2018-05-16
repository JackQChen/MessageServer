using System;
using System.Drawing;
using System.Windows.Forms;
using VirtualControls.UserControls;

namespace VirtualControls
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.vcc1.BackColor = Color.Transparent;
            this.vcc1.Location = Point.Empty;
            this.vcc1.Width = this.Width;
            this.vcc2.BackColor = Color.Transparent;
            this.vcc2.Location = Point.Empty;
            this.vcc2.Width = this.Width;
            this.vcc1.BringToFront();
            for (int i = 0; i < 100; i++)
            {
                var btn = new VirtualButton();
                btn.Rectangle = new Rectangle((i % 10) * 50, (i / 10) * 30, 48, 28);
                btn.Text = string.Format("按钮({0})", i);
                this.vcc1.Controls.Add(btn);
            }
            this.vcc1.Refresh();

            for (int i = 0; i < 100; i++)
            {
                var btn = new VirtualButton();
                btn.Rectangle = new Rectangle((i % 10) * 60, (i / 10) * 40, 58, 38);
                btn.Text = string.Format("Btn({0})", i);
                this.vcc2.Controls.Add(btn);
            }
            this.vcc2.Refresh();

            this.vccControl.BackColor = Color.Transparent;
            var btnChange = new VirtualButton();
            int w = 100, h = 50;
            btnChange.Rectangle = new Rectangle((this.vccControl.Width - w) / 2, (this.vccControl.Height - h) / 2, w, h);
            btnChange.MouseDown += new MouseEventHandler(btnChange_MouseDown);
            btnChange.Text = "切换";

            var chk = new VirtualCheckBox();
            chk.Rectangle = new Rectangle(10, 10, 100, 20);
            chk.Text = "勾选内容";

            this.vccControl.Controls.Add(chk);
            this.vccControl.Controls.Add(btnChange);

            this.vccControl.Refresh();
        }

        bool show = false;

        void btnChange_MouseDown(object sender, MouseEventArgs e)
        {
            show = !show;
            if (show)
                this.vcc2.BringToFront();
            else
                this.vcc1.BringToFront();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pictureBox1.Image = show ? this.vcc2.Image : this.vcc1.Image;
        }
    }
}
