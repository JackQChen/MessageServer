using System;
using System.Drawing;
using System.Windows.Forms;
using CustomSkin;
using CustomSkin.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace WindowsFormsApplication1
{
    public partial class Form1 : FormBase
    {
        public Form1()
        {
            InitializeComponent();
            this.contextMenuStrip1.Renderer = new ToolStripRenderers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.TitleColor == Color.Black)
                this.TitleColor = Color.White;
            else
                this.TitleColor = Color.Black;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.BackgroundImage = null;
                this.BackColor = this.colorDialog1.Color;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.TitleStyle = this.checkBox3.Checked;
        }

        int imgIndex = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Res.Current.ImageList[imgIndex];
            imgIndex++;
            if (imgIndex == Res.Current.ImageList.Count)
                imgIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //foreach (string str in Directory.GetFiles(Application.StartupPath + "\\Image", "*.*", SearchOption.AllDirectories))
            //    Res.Current.ImageList.Add(Image.FromFile(str));
            Res.Current.ImageList.AddRange(new Image[] { 
            Resources._1,
            Resources._2,
            Resources._3,
            Resources._4,
            Resources._5,
            Resources._6,
            Resources._7,
            Resources._8
            });
            unitWidth = this.pictureBox1.Width / 17;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            this.BackColor = new Bitmap(this.pictureBox1.Image).GetPixel(clickX + unitWidth / 2, unitWidth / 2);
        }

        int colorX = 0, unitWidth = 0;
        int clickX = 0;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.RoyalBlue, 5), colorX + 1, 1, unitWidth, 37);
            e.Graphics.DrawImage(Resources.uncheck, colorX, 0);
            e.Graphics.DrawImage(Resources.check, clickX, 0);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (colorX != (e.X / unitWidth) * unitWidth)
            {
                colorX = (e.X / unitWidth) * unitWidth;
                this.pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            clickX = colorX;
            this.pictureBox1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (progressBar2.Value == 100)
                progressBar2.Value = 0;
            progressBar2.Value += 5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = ActiveControl.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Text = CustomSkin.Windows.Forms.MessageBox.Show("123", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information).ToString();
        }
    }
}
