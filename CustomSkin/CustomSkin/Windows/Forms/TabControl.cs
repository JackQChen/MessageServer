using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class TabControl : System.Windows.Forms.TabControl
    {
        public TabControl()
            : base()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw
                |
                ControlStyles.SupportsTransparentBackColor
                , true);
            this.SetStyle(ControlStyles.Opaque, false);
            base.SizeMode = TabSizeMode.Fixed;
            base.ItemSize = new Size(80, 32);
            this.HeadColor = Color.Black;
            base.UpdateStyles();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.DrawBackground(g);
            this.DrawTabPages(g);
        }

        public override Color BackColor
        {
            get
            {
                return Color.FromArgb(0, 255, 255, 255);//.Transparent;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public Color HeadColor { get; set; }

        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackground(Graphics g)
        {
            int width = this.ClientRectangle.Width;
            int height = this.ClientRectangle.Height - this.DisplayRectangle.Height;
            Color backColor = this.Enabled ? System.Drawing.Color.Transparent : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
            Rectangle bgRect = new Rectangle(2, 2, this.Width - 2, this.ItemSize.Height);
            //this.DrawImage(g, DotNet.Res.Current.GetImage("aio_png_bkg.png"), bgRect);//绘制背景图
        }
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="rect"></param>
        private void DrawImage(Graphics g, Image image, Rectangle rect)
        {
            g.DrawImage(image, new Rectangle(rect.X, rect.Y, 5, rect.Height), 0, 0, 5, image.Height, GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0, image.Width - 10, image.Height, GraphicsUnit.Pixel);
            g.DrawImage(image, new Rectangle(rect.X + rect.Width - 5, rect.Y, 5, rect.Height), image.Width - 5, 0, 5, image.Height, GraphicsUnit.Pixel);
        }
        private void DrawTabPages(Graphics g)
        {
            //绘制TabContainer
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, SystemColors.Window)))
            {
                int x = 2;
                int y = this.ItemSize.Height;
                int width = this.Width - 2;
                int height = this.Height - this.ItemSize.Height;
                g.FillRectangle(brush, x, y, width, height);
                g.DrawRectangle(new Pen(SystemColors.Window), x, y + 2, width - 1, height - 3);
            }
            //绘制TabHeader
            Rectangle tabRect = Rectangle.Empty;
            Point cursorPoint = this.PointToClient(MousePosition);
            for (int i = 0; i < base.TabCount; i++)
            {
                TabPage page = this.TabPages[i];
                if (page.BackColor != BackColor)
                { page.BackColor = BackColor; }
                tabRect = this.GetTabRect(i);
                Color baseColor = Color.Yellow;
                Image baseTabHeaderImage = null;
                if (this.SelectedIndex == i)//是否选中 
                {
                    baseTabHeaderImage = CustomSkin.Res.Current.GetImage("Resources.TabControl.main_tab_check.png");
                    if (this.SelectedIndex == this.TabCount - 1)
                        tabRect.Inflate(2, 0);
                    else
                        tabRect.Inflate(1, 0);
                    this.DrawImage(g, baseTabHeaderImage, tabRect);
                    Font font = new Font(page.Font, FontStyle.Bold);
                    TextRenderer.DrawText(g, page.Text, font, tabRect, this.HeadColor);
                }
                else if (tabRect.Contains(cursorPoint))//鼠标滑动
                {
                    baseTabHeaderImage = CustomSkin.Res.Current.GetImage("Resources.TabControl.main_tab_highlight.png");
                    this.DrawImage(g, baseTabHeaderImage, tabRect);
                    TextRenderer.DrawText(g, page.Text, page.Font, tabRect, this.HeadColor);
                }
                else
                {
                    TextRenderer.DrawText(g, page.Text, page.Font, tabRect, this.HeadColor);
                }

            }

        }
    }
}
