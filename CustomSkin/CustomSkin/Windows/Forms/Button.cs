
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using CustomSkin.Drawing;

namespace CustomSkin.Windows.Forms
{
    public class Button : System.Windows.Forms.Button
    {
        public Button()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            base.BackColor = Color.Transparent;
        }

        #region MouseState
        private MouseState _mouseState = MouseState.Normal;
        /// <summary>
        /// 鼠标状态
        /// </summary>
        internal MouseState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                this.Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.mousePosition = Point.Empty;
            this.Invalidate();
        }

        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseEnter 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.MouseState = MouseState.Enter;
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.MouseState = MouseState.Leave;
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown 事件。
        /// </summary>
        /// <param name="mevent">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            this.MouseState = MouseState.Down;
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseUp 事件。
        /// </summary>
        /// <param name="mevent">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            this.MouseState = MouseState.Up;
        }
        private Point mousePosition = Point.Empty;
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            mousePosition = mevent.Location;
        }
        #endregion

        private bool focusBorder = true;
        [Category("自定义"), Description("是否显示焦点框")]
        public virtual bool FocusBorder
        {
            get { return this.focusBorder; }
            set { this.focusBorder = value; }
        }

        Rectangle rectAll = Rectangle.Empty, rectPaint = Rectangle.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            rectAll = new Rectangle(0, 0, this.Width, this.Height);
            rectPaint = new Rectangle(2, 2, this.rectAll.Width - 4, this.rectAll.Height - 4);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }

        private TextFormatFlags _textAlign = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
        ContentAlignment textAlign = ContentAlignment.MiddleCenter;

        [Category("自定义"), Description("按钮上文字的对齐方式")]
        public new ContentAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                textAlign = value;
                switch (textAlign)
                {
                    case ContentAlignment.BottomCenter:
                        this._textAlign = TextFormatFlags.Bottom |
                                          TextFormatFlags.HorizontalCenter |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomLeft:
                        this._textAlign = TextFormatFlags.Bottom |
                                          TextFormatFlags.Left |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomRight:
                        this._textAlign = TextFormatFlags.Bottom |
                                          TextFormatFlags.Right |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleCenter:
                        this._textAlign = TextFormatFlags.SingleLine |
                                          TextFormatFlags.HorizontalCenter |
                                          TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleLeft:
                        this._textAlign = TextFormatFlags.Left |
                                          TextFormatFlags.VerticalCenter |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleRight:
                        this._textAlign = TextFormatFlags.Right |
                                          TextFormatFlags.VerticalCenter |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopCenter:
                        this._textAlign = TextFormatFlags.Top |
                                          TextFormatFlags.HorizontalCenter |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopLeft:
                        this._textAlign = TextFormatFlags.Top |
                                          TextFormatFlags.Left |
                                          TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopRight:
                        this._textAlign = TextFormatFlags.Top |
                                          TextFormatFlags.Right |
                                          TextFormatFlags.SingleLine;
                        break;
                }
                base.Invalidate(this.rectPaint);
            }
        }

        string
            strFocus = "Resources.Button.Light.png",
            strDown = "Resources.Button.down.png",
            strEnter = "Resources.Button.focus.png",
            strNormal = "Resources.Button.normal.png",
            strUnable = "Resources.Button.gray.png";
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            if (this.Enabled)
            {
                if (this.Focused && this.FocusBorder)
                    g.RendererBackground(this.rectAll, Res.Current.GetImage(strFocus), true);
                switch (this.MouseState)
                {
                    case MouseState.Down:
                        g.RendererBackground(this.rectPaint, Res.Current.GetImage(strDown), true);
                        break;
                    case MouseState.Up:
                        g.RendererBackground(this.rectPaint, Res.Current.GetImage(
                            this.rectPaint.Contains(this.mousePosition) ? strEnter : strNormal), true);
                        break;
                    case MouseState.Enter:
                        g.RendererBackground(this.rectPaint, Res.Current.GetImage(strEnter), true);
                        break;
                    case MouseState.Normal:
                    case MouseState.Leave:
                        g.RendererBackground(this.rectPaint, Res.Current.GetImage(strNormal), true);
                        break;
                }
                //绘制按钮上的文字
                TextRenderer.DrawText(g, this.Text, this.Font, this.rectPaint, this.ForeColor, this._textAlign);
            }
            else
            {
                g.RendererBackground(this.rectPaint, Res.Current.GetImage(strUnable), true);
                TextRenderer.DrawText(g, this.Text, this.Font, this.rectPaint, Color.Gray, this._textAlign);
            }
        }
    }
}
