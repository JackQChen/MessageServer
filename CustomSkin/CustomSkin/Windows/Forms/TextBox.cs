using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public class TextBox : System.Windows.Forms.TextBox
    {
        public TextBox()
        {
            this.SetStyle(
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.DoubleBuffer, false);
            WatermarkColor = Color.FromArgb(204, 204, 204);
            base.BorderStyle = BorderStyle.FixedSingle;
            this.UpdateStyles();

        }
        // protected ListBox list = null;
        private MouseState MouseState { get; set; }
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { } }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                return param;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            MouseState = MouseState.Normal;
            base.OnLostFocus(e);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            MouseState = MouseState.Down;
            base.OnGotFocus(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.MouseState = MouseState.Enter;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Normal;
            base.OnMouseLeave(e);
        }

        const int WM_PAINT = 0x000f;
        const int WM_ERASEBKGND = 0x0014;
        const int WM_NCPAINT = 0x0085;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Image hotLine = null;
            switch (MouseState)
            {
                case MouseState.Down:
                case MouseState.Enter:
                    hotLine = Res.Current.GetImage("Resources.TextBox_move.png"); break;
                default: ; break;
            }
            Rectangle IconRect = new Rectangle(0, 0, this.Width, this.Height);
            Graphics g = e.Graphics;
            {
                RendererBackground(g, IconRect, 3, hotLine);
            }
        }

        protected override void WndProc(ref   Message m)
        {
            // Region = new Region(GetRoundRectPath(new RectangleF(0, 0, Width, Height), 0.5F));
            base.WndProc(ref   m);
            if (m.Msg == WM_PAINT || m.Msg == WM_ERASEBKGND || m.Msg == WM_NCPAINT)
            {
                Image hotLine = null;
                switch (MouseState)
                {
                    case MouseState.Down:
                    case MouseState.Enter: hotLine = Res.Current.GetImage("Resources.TextBox.TextBox_move.png"); break;
                    default: ; break;
                }
                Rectangle IconRect = new Rectangle(0, 0, this.Width, this.Height);
                using (Graphics g = this.CreateGraphics())
                {
                    RendererBackground(g, IconRect, 3, hotLine);
                }
            }

        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            MouseState = MouseState.Enter;
            base.OnKeyDown(e);
        }
        public void RendererBackground(Graphics g, Rectangle rect, int cut, Image backgroundImage)
        {
            if (backgroundImage == null)
            {
                RendererBackground(g, rect, cut);
                return;
            }
            //左上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y, cut, cut), 0, 0, cut, cut, GraphicsUnit.Pixel);
            //上边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y, rect.Width - cut * 2, cut), cut, 0, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //右上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y, cut, cut), backgroundImage.Width - cut, 0, cut, cut, GraphicsUnit.Pixel);
            //左边
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + cut, cut, rect.Height - cut * 2), 0, cut, cut, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //左下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + rect.Height - cut, cut, cut), 0, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //右边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + cut, cut, rect.Height - cut * 2), backgroundImage.Width - cut, cut, cut, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //右下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + rect.Height - cut, cut, cut), backgroundImage.Width - cut, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //下边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + rect.Height - cut, rect.Width - cut * 2, cut), cut, backgroundImage.Height - cut, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //平铺中间
            //  g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + cut, rect.Width - cut * 2, rect.Height - cut * 2), cut, cut, backgroundImage.Width - cut * 2, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
            //
            if (this.Text == string.Empty)
            {
                g.DrawString(WatermarkText, this.Font, new SolidBrush(WatermarkColor), 2, 2);
            }


        }
        private string myWatermarkText;
        private Color myWatermarkColor;
        public string WatermarkText { get { return myWatermarkText; } set { myWatermarkText = value; this.Invalidate(); } }
        public Color WatermarkColor { get { return myWatermarkColor; } set { myWatermarkColor = value; this.Invalidate(); } }
        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cut"></param>
        /// <param name="backgroundImage"></param>
        public void RendererBackground(Graphics g, Rectangle rect, int cut)
        {
            g.DrawLine(new Pen(Color.FromArgb(171, 171, 171)), 0, 0, 0, rect.Height);
            g.DrawLine(new Pen(Color.FromArgb(171, 171, 171)), 0, 0, rect.Width, 0);

            g.DrawLine(new Pen(Color.FromArgb(171, 171, 171)), rect.Width - 1, 0, rect.Width - 1, rect.Height);
            g.DrawLine(new Pen(Color.FromArgb(171, 171, 171)), 0, rect.Height - 1, rect.Width, rect.Height - 1);
            if (this.Text == string.Empty)
            {
                g.DrawString(WatermarkText, this.Font, new SolidBrush(WatermarkColor), 2, 2);//SolidBrush b=new SolidBrush(Color.FromArgb(204,204,204))
            }
        }
    }
}
