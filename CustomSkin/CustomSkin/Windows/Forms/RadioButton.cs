using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.RadioButton))]
    public class RadioButton : System.Windows.Forms.RadioButton
    {
        /// <summary>
        /// 
        /// </summary>
        private MouseState _mouseState = MouseState.Normal;
        /// <summary>
        /// 
        /// </summary>
        public RadioButton()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            this.BackColor = Color.Transparent;
        }
        internal MouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 文本区域
        /// </summary>
        internal Rectangle TextRect
        {
            get { return new Rectangle(17, 0, this.Width - 17, this.Height); }
        }
        /// <summary>
        /// 图片显示区域
        /// </summary>
        internal Rectangle ImageRect
        {
            get { return new Rectangle(0, (this.Height - 17) / 2, 17, 17); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            Color foreColor = base.Enabled ? this.ForeColor : Color.Gray;
            TextFormatFlags flags = TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.Left |
                                    TextFormatFlags.SingleLine;
            TextRenderer.DrawText(g, this.Text, this.Font, this.TextRect, foreColor, flags);

            switch (this.MouseState)
            {
                case MouseState.Leave:
                case MouseState.Normal:
                    if (base.Checked)
                    {
                        Image normal = Res.Current.GetImage("Resources.RadioButton.tick_normal.png");
                        g.DrawImage(normal, this.ImageRect);
                    }
                    else
                    {
                        Image normal = Res.Current.GetImage("Resources.RadioButton.normal.png");
                        g.DrawImage(normal, this.ImageRect);
                    }

                    break;
                case MouseState.Enter:
                case MouseState.Down:
                case MouseState.Up:
                    if (base.Checked)
                    {
                        Image high = Res.Current.GetImage("Resources.RadioButton.tick_highlight.png");
                        g.DrawImage(high, this.ImageRect);
                    }
                    else
                    {
                        Image high = Res.Current.GetImage("Resources.RadioButton.highlight.png");
                        g.DrawImage(high, this.ImageRect);
                    }
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            this.MouseState = MouseState.Enter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            this.MouseState = MouseState.Leave;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            this.MouseState = MouseState.Up;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            this.MouseState = MouseState.Down;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.MouseState = MouseState.Enter;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.MouseState = MouseState.Leave;
        }
    }
}
