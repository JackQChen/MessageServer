using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public partial class SkinForm : FormBase
    {
        public SkinForm()
        {
            InitializeComponent();
        }

        Rectangle rectSelect = Rectangle.Empty;
        List<Rectangle> listRect;
        List<Image> listImage;
        Bitmap bmp;

        private void SkinForm_Load(object sender, EventArgs e)
        {
            this.listRect = new List<Rectangle>();
            listImage = Res.Current.ImageList;
            int x = 0, y = 0, picWidth = 200, picHeight = 100, space = 5, xCount = 3, yCount = 3;
            bmp = new Bitmap(picWidth * xCount + space * (xCount + 1), picHeight * yCount + space * (yCount + 1));
            Graphics g = Graphics.FromImage(bmp);
            x = space;
            y = space;
            if (listImage.Count == 0)
            {
                string strEmpty = "没有图片信息";
                Size size = TextRenderer.MeasureText(strEmpty, this.Font);
                TextRenderer.DrawText(g, strEmpty, this.Font, new Point(
                    (this.picImage.Width - size.Width) / 2,
                    (this.picImage.Height - size.Height) / 2)
                    , SystemColors.ControlText);
            }
            else
                for (int i = 0; i < listImage.Count; i++)
                {
                    Rectangle rect = new Rectangle(x, y, picWidth, picHeight);
                    this.listRect.Add(rect);
                    g.DrawImage(listImage[i], rect);
                    x += picWidth + space;
                    if (x >= (picWidth + space) * xCount)
                    {
                        x = space;
                        y += picHeight + space;
                    }
                }
            this.picImage.Image = bmp;
            this.unitWidth = this.picImage.Width / 18;
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rectCurrent = this.listRect.Find(match =>
            {
                return e.X > match.X * this.picImage.Width / bmp.Width
                    && e.Y > match.Y * this.picImage.Height / bmp.Height
                    && e.X < (match.X + match.Width) * this.picImage.Width / bmp.Width
                    && e.Y < (match.Y + match.Height) * this.picImage.Height / bmp.Height;
            });
            if (!rectCurrent.Equals(this.rectSelect))
            {
                this.rectSelect = rectCurrent;
                this.picImage.Invalidate();
            }
        }

        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(39, 140, 222), 3),
               this.rectSelect.X * this.picImage.Width / bmp.Width,
               this.rectSelect.Y * this.picImage.Height / bmp.Height,
               this.rectSelect.Width * this.picImage.Width / bmp.Width,
               this.rectSelect.Height * this.picImage.Height / bmp.Height);
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            if (this.listRect.Count == 0)
                return;
            if (this.rectSelect == Rectangle.Empty)
                return;
            Image img = this.listImage[this.listRect.IndexOf(this.rectSelect)];
            if (img != this.BackgroundImage)
            {
                this.BackgroundImage = img;
                this.Owner.BackgroundImage = img;
            }
        }

        int colorX = 0, unitWidth;

        private void picColor_MouseMove(object sender, MouseEventArgs e)
        {
            if (colorX != (e.X / unitWidth) * unitWidth)
            {
                colorX = (e.X / unitWidth) * unitWidth;
                this.picColor.Invalidate();
            }
        }

        private void picColor_Click(object sender, EventArgs e)
        {
            if (colorX == this.picColor.Width - unitWidth)
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    if (cd.ShowDialog(this) == DialogResult.OK)
                    {
                        this.BackgroundImage = null;
                        this.BackColor = cd.Color;
                        this.Owner.BackgroundImage = null;
                        this.Owner.BackColor = this.BackColor;
                    }
                }
            }
            else
            {
                this.BackgroundImage = null;
                Color color = new Bitmap(this.picColor.Image).GetPixel(colorX + unitWidth / 2, unitWidth / 2);
                if (color != this.BackColor)
                {
                    this.BackColor = color;
                    this.Owner.BackgroundImage = null;
                    this.Owner.BackColor = color;
                }
            }
        }

        private void picColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(39, 140, 222), 3), colorX, 1, unitWidth, 30);
        }
    }
}
