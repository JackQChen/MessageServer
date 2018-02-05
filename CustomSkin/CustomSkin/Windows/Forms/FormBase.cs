using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using CustomSkin.Drawing;
using CustomSkin.Windows.Win32;

namespace CustomSkin.Windows.Forms
{
    /// <summary>
    /// 表示组成应用程序的用户界面的窗口或对话框。
    /// </summary>
    public class FormBase : Form
    {
        public delegate Image SysButtonBorderImage(SysButtons button, MouseState buttonMouseState, FormWindowState windowState);
        private static SysButtonBorderImage mySysButtonBorderImage;
        public static SysButtonBorderImage CreateSysButtonBorderImage
        {
            get
            {
                if (mySysButtonBorderImage == null)
                {
                    mySysButtonBorderImage = CreateSysButtonBorderImage = (b, s, ws) =>
                    {
                        string resName = "Resources.Form.{0}_{1}.png";
                        string name1 = string.Empty;
                        string name2 = string.Empty;
                        switch (b)
                        {
                            case SysButtons.Close:
                                name1 = "btn_close";
                                break;
                            case SysButtons.Maximize:
                                name1 = ws == FormWindowState.Maximized ? "btn_restore" : "btn_max";
                                break;
                            case SysButtons.Minimize:
                                name1 = "btn_mini";
                                break;
                            case SysButtons.Skin:
                                name1 = "btn_Skin";
                                break;
                        }
                        switch (s)
                        {
                            case MouseState.Normal:
                                name2 = "normal";
                                break;
                            case MouseState.Enter:
                            case MouseState.Up:
                                name2 = "highlight";
                                break;
                            case MouseState.Down:
                                name2 = "down";
                                break;
                        }
                        return Res.Current.GetImage(string.Format(resName, name1, name2));
                    };
                }
                return mySysButtonBorderImage;
            }
            set
            {
                mySysButtonBorderImage = value;
            }
        }
        private static Image myWindowsBorderImage;
        private static Image myMessageBorderImage;
        public static Image WindowsBorderImage
        {
            get
            {
                if (myWindowsBorderImage == null)
                {
                    myWindowsBorderImage = Res.Current.GetImage("Resources.Form.WindowsBorderImage.png");
                }
                return myWindowsBorderImage;
            }
            set
            {
                myWindowsBorderImage = value;
            }
        }
        public static Image MessageBorderImage
        {
            get
            {
                if (myMessageBorderImage == null)
                {
                    myMessageBorderImage = Res.Current.GetImage("Resources.Form.MessageBorderImage.png");
                }
                return myMessageBorderImage;
            }
            set
            {
                myMessageBorderImage = value;
            }
        }
        public FormBase()
        {
            IsResize = true;
            this.SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.ResizeRedraw |
              ControlStyles.Selectable |
              ControlStyles.ContainerControl |
              ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(3, 31, 3, 3);
            if (Application.OpenForms.Count > 0 && !this.Equals(Application.OpenForms[0]))
            {
                var form = Application.OpenForms[0];
                if (form.BackgroundImage == null)
                    this.BackColor = form.BackColor;
                else
                {
                    this.BackgroundImage = form.BackgroundImage;
                    this.BackgroundImageLayout = form.BackgroundImageLayout;
                }
                this.Icon = form.Icon;
                if (form as FormBase != null)
                    this.TitleColor = ((FormBase)form).TitleColor;
            }
            this.UpdateStyles();
        }
        private Color myTitleColor;
        [Category("自定义"), Description("标题颜色")]
        public Color TitleColor { get { return myTitleColor; } set { myTitleColor = value; this.Invalidate(); } }
        /// <summary>
        /// 是否允许改变窗口大小
        /// </summary>
        [Category("自定义"), DefaultValue(true), Description("是否允许改变窗口大小")]
        public bool IsResize { get; set; }
        /// <summary>
        /// 标题文本显示区域
        /// </summary>
        protected virtual Rectangle TextRect
        {
            get
            {
                int width = this.TitleRect.Width - this.IconRect.Width - 15;
                int height = this.TitleRect.Height - 10;
                height = Math.Min(height, FontHeight + 10);
                Rectangle textRect = new Rectangle(8, 5, width, height);
                if (this.ShowIcon)
                    textRect.X = this.IconRect.Width + 10;
                return textRect;
            }
        }
        /// <summary>
        /// 图标显示区域
        /// </summary>
        protected virtual Rectangle IconRect
        {
            get { return new Rectangle(6, 5, 21, 21); }
        }
        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        protected virtual Rectangle CloseRect
        {
            get { return new Rectangle(this.Width - 39, -1, 39, 20); }
        }
        /// <summary>
        /// 最大化按钮区域
        /// </summary>
        protected virtual Rectangle MaxRect
        {

            get
            {
                if (MaximizeBox)
                {
                    return new Rectangle(this.Width - this.CloseRect.Width - 28, -1, 28, 20);
                }
                return Rectangle.Empty;
            }
        }
        /// <summary>
        /// 最小化按钮区域
        /// </summary>
        protected virtual Rectangle MiniRect
        {
            get
            {
                if (MinimizeBox)
                {
                    int x = this.Width - this.CloseRect.Width - 28;
                    if (MaximizeBox)
                    {
                        x = x - MaxRect.Width;
                    }
                    return new Rectangle(x, -1, 28, 20);
                }
                return Rectangle.Empty;
            }
        }
        /// <summary>
        /// 皮肤按钮区域
        /// </summary>
        protected virtual Rectangle SkinRect
        {
            get
            {
                if (SkinButtonVisible)
                {
                    int x = this.Width - this.CloseRect.Width - 28;
                    if (MaximizeBox)
                    {
                        x = x - MaxRect.Width;
                    }
                    if (MinimizeBox)
                    {
                        x = x - MiniRect.Width;
                    }
                    return new Rectangle(x, -1, 28, 20);
                }
                return Rectangle.Empty;
            }
        }
        /// <summary>
        /// 是否显示皮肤按钮
        /// </summary>
        [Category("自定义"), DefaultValue(false), Description("是否显示皮肤按钮")]
        public bool SkinButtonVisible
        {
            get
            {
                return mySkinBox;
            }
            set
            {
                mySkinBox = value;
                base.Invalidate(this.SkinRect);
            }
        }
        /// <summary>
        /// 标题栏区域
        /// </summary>
        [Browsable(false)]
        public Rectangle TitleRect
        {
            get
            {
                return new Rectangle(3, 3, this.Width, Padding.Top);
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                base.Invalidate(this.TitleRect);
            }
        }
        private bool mySkinBox;
        private bool myTitleBorder;

        [Category("自定义"), DefaultValue(false), Description("标题框样式")]
        public bool TitleStyle
        {
            get
            {
                return myTitleBorder;
            }
            set
            {
                myTitleBorder = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 皮肤按钮当前的鼠标状态
        /// </summary>
        private MouseState mySkinState;
        /// <summary>
        /// 关闭按钮当前的鼠标状态
        /// </summary>
        private MouseState myCloseState;
        /// <summary>
        /// 最大化按钮当前的鼠标状态
        /// </summary>
        private MouseState myMaxState;
        /// <summary>
        /// 最小化按钮当前的鼠标状态
        /// </summary>
        private MouseState myMinState;
        /// <summary>
        /// 皮肤按钮当前的鼠标状态
        /// </summary>
        protected MouseState SkinState
        {
            get { return mySkinState; }
            set
            {
                mySkinState = value;
                base.Invalidate(this.SkinRect);
            }
        }
        /// <summary>
        /// 关闭按钮当前的鼠标状态
        /// </summary>
        protected MouseState CloseState
        {
            get { return myCloseState; }
            set
            {
                myCloseState = value;
                base.Invalidate(this.CloseRect);
            }
        }
        /// <summary>
        /// 最大化按钮当前的鼠标状态
        /// </summary>
        protected MouseState MaxState
        {
            get { return myMaxState; }
            set
            {
                myMaxState = value;
                base.Invalidate(this.MaxRect);
            }
        }
        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Handle.SetWindowRgn(0, 0, this.Width + 1, this.Height + 1, 4, 4);
            this.Invalidate();
        }
        /// <summary>
        /// 最小化按钮当前的鼠标状态
        /// </summary>
        protected MouseState MinState
        {
            get { return myMinState; }
            set
            {
                myMinState = value;
                base.Invalidate(this.MiniRect);
            }
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (base.BackColor != value)
                {
                    base.BackgroundImage = null;
                }
                base.BackColor = value;
            }
        }
        /// <summary>
        /// 封装创建控件时所需的信息。
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                if (this.Opacity < 1)
                {
                    CreateParams param = base.CreateParams;
                    param.ExStyle = 0x00080000;
                    return param;
                }
                return base.CreateParams;
            }
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.FormClosing 事件。
        /// </summary>
        /// <param name="e">一个包含事件数据的 System.Windows.Forms.FormClosingEventArgs。</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Handle.AnimateWindow(250, AnimateWindowFlags.AW_SLIDE | AnimateWindowFlags.AW_HIDE | AnimateWindowFlags.AW_BLEND | AnimateWindowFlags.AW_CENTER);
            base.OnFormClosing(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.IsHandleCreated)
            {
                this.Handle.SetWindowRgn(0, 0, this.Width + 1, this.Height + 1, 4, 4);
                this.Invalidate();
            }
        }
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            base.Invalidate();
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseDown。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point point = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                if (this.CloseRect.Contains(point))
                {
                    this.CloseState = MouseState.Down;
                }
                else if (this.MiniRect.Contains(point))
                {
                    this.MinState = MouseState.Down;
                }
                else if (this.MaxRect.Contains(point))
                {
                    this.MaxState = MouseState.Down;
                }
                else if (this.SkinRect.Contains(point))
                {
                    this.SkinState = MouseState.Down;
                }
                else
                {
                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            User32.ReleaseCapture();
                            Handle.SendMessage(274, 61440 + 9, 0);
                        }
                    }
                }
            }
        }
        private string currArea = "", lastArea = "";
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseMove。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
                return;
            Point point = e.Location;
            if (this.CloseRect.Contains(point)
                && this.CloseState != MouseState.Enter)
            {
                this.CloseState = MouseState.Enter;
                currArea = "CloseState";
            }
            else if (this.MiniRect.Contains(point)
                && this.MinState != MouseState.Enter)
            {
                this.MinState = MouseState.Enter;
                currArea = "MinState";
            }
            else if (this.MaxRect.Contains(point)
                && this.MaxState != MouseState.Enter)
            {
                this.MaxState = MouseState.Enter;
                currArea = "MaxState";
            }
            else if (this.SkinRect.Contains(point)
                && this.SkinState != MouseState.Enter)
            {
                this.SkinState = MouseState.Enter;
                currArea = "SkinState";
            }
            if (this.lastArea != "" && this.lastArea != this.currArea)
            {
                this.GetType().GetProperty(this.lastArea,
                    System.Reflection.BindingFlags.GetProperty
                    | System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance)
                    .SetValue(this, MouseState.Normal, null);
                this.lastArea = this.currArea;
            }
            if (this.lastArea != this.currArea)
                this.lastArea = this.currArea;
        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseLeave。
        /// </summary>
        /// <param name="e">包含事件数据的 System.EventArgs。</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.CloseState = MouseState.Normal;
            this.MaxState = MouseState.Normal;
            this.MinState = MouseState.Normal;
            this.SkinState = MouseState.Normal;

        }
        /// <summary>
        /// 引发 System.Windows.Forms.Form.MouseUp。
        /// </summary>
        /// <param name="e">包含事件数据的 System.Windows.Forms.MouseEventArgs。</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
                return;
            Point point = e.Location;
            if (this.CloseRect.Contains(point))
            {
                this.CloseState = MouseState.Enter;
                this.Close();
            }
            else
            {
                this.CloseState = MouseState.Normal;
            }
            if (this.MiniRect.Contains(point))
            {
                this.MinState = MouseState.Enter;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.MinState = MouseState.Normal;
            }
            if (this.MaxRect.Contains(point))
            {
                this.MaxState = MouseState.Enter;
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            else
            {
                this.MaxState = MouseState.Normal;
            }
            if (this.SkinRect.Contains(point))
            {
                this.SkinState = MouseState.Enter;
                using (SkinForm skinForm = new SkinForm())
                {
                    skinForm.Icon = this.Icon;
                    skinForm.BackgroundImage = this.BackgroundImage;
                    skinForm.BackColor = this.BackColor;
                    skinForm.TitleColor = this.TitleColor;
                    skinForm.ShowDialog(this);
                    this.OnSkinChanged();
                }
            }
            else
            {
                this.SkinState = MouseState.Normal;
            }
        }

        protected virtual void OnSkinChanged()
        {

        }

        protected virtual void OnMainChangeBack(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (form != null)
            {
                this.BackgroundImage = form.BackgroundImage;
                this.BackColor = form.BackColor;
                this.BackgroundImageLayout = form.BackgroundImageLayout;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawBorder(Graphics g)
        {
            var titleHeight = Padding.Top;

            if (TitleStyle)
            {
                var fromBorderImg = WindowsBorderImage;

                g.DrawImage(WindowsBorderImage, 0, 0, 5, titleHeight, 0, 0, 5, 31);
                g.DrawImage(WindowsBorderImage, 5, 0, this.Width - 10, 5, 5, 0, WindowsBorderImage.Width - 10, 5);//上中间
                g.DrawImage(WindowsBorderImage, 5, 5, this.Width - 10, titleHeight - 5, 5, 5, WindowsBorderImage.Width - 10, 26);//上中间
                g.DrawImage(WindowsBorderImage, this.Width - 5, 0, 5, titleHeight, WindowsBorderImage.Width - 5, 0, 5, 31);

                g.DrawImage(fromBorderImg, 0, titleHeight, 2, 25, 0, 31, 2, 25);
                g.DrawImage(fromBorderImg, 2, titleHeight, this.Width - 4, 25, 2, 31, fromBorderImg.Width - 4, 25);
                g.DrawImage(fromBorderImg, this.Width - 2, titleHeight, 2, 25, fromBorderImg.Width - 2, 31, 2, 25);

                g.DrawImage(fromBorderImg, 0, this.Height - Padding.Bottom, 5, Padding.Bottom, 0, fromBorderImg.Height - Padding.Bottom, 5, Padding.Bottom);
                g.DrawImage(fromBorderImg, 5, this.Height - Padding.Bottom, this.Width - 10, Padding.Bottom, 5, fromBorderImg.Height - Padding.Bottom, fromBorderImg.Width - 10, Padding.Bottom);
                g.DrawImage(fromBorderImg, this.Width - 5, this.Height - Padding.Bottom, 5, Padding.Bottom, fromBorderImg.Width - 5, fromBorderImg.Height - Padding.Bottom, 5, Padding.Bottom);

                titleHeight += 25;
                var bottom = Height - (titleHeight + Padding.Bottom);
                g.DrawImage(fromBorderImg, 0, titleHeight, 5, bottom, 0, fromBorderImg.Height - 230, 5, 130);
                g.DrawImage(fromBorderImg, 5, titleHeight, this.Width - 10, bottom, 5, fromBorderImg.Height - 230, fromBorderImg.Width - 10, 130);
                g.DrawImage(fromBorderImg, this.Width - 5, titleHeight, 5, bottom, fromBorderImg.Width - 5, fromBorderImg.Height - 230, 5, 130);

            }
            else
            {
                var _backImage = MessageBorderImage;
                //  // 左上角
                //  g.DrawImage(_backImage, 0, 0, 5, 5, 5, 5, 10, 10);

                // // g.DrawImage(_backImage, 0, 5, 5, titleHeight / 2 - 5, 5, 10,10, 5);
                //  // 左边
                // // g.DrawImage(_backImage, 0, 5, 5, Height - 10, 5, 10, 10, _backImage.Height - 20);
                //  // 左下角
                //  g.DrawImage(_backImage, 0, Height - 5, 5, 5, 5, _backImage.Height - 20, 5, 5);

                //  // 右上角
                //  g.DrawImage(_backImage, Width - 5, 0, 5, 5, _backImage.Width - 10, 5, 5, 5);
                //  // 右边
                ////  g.DrawImage(_backImage, Width - 5, 5, 5, Height - 10, _backImage.Width - 10, 10, 5, _backImage.Height - 20);
                //  // 右下角
                //  g.DrawImage(_backImage, Width - 5, Height - 5, 5, 5, _backImage.Width - 10, _backImage.Height - 10, 5, 5);

                //  // 上边
                //  g.DrawImage(_backImage, 5, 0, Width - 10, 5, 10, 5, _backImage.Width - 20, 5);
                //  // 下边
                //  g.DrawImage(_backImage, 5, Height, Width - 10, 5, 10, _backImage.Height - 10, _backImage.Width - 20, 5);


                // g.DrawImage(_backImage, 5, 5,                Width - 10, Height - 10, 10, 10, _backImage.Width - 20, _backImage.Height - 20);
                //// 填充
                //g.DrawImage(_backImage, 5, 5, Width - 10, titleHeight/2 - 5, 10, 0, _backImage.Width - 20, 5);

                //g.DrawImage(_backImage, 5, titleHeight / 2 - 5, Width - 10, Height - 10, 10, 5, _backImage.Width - 20, _backImage.Height - 20);

                g.DrawImage(_backImage, 0, 0, Width, titleHeight - (titleHeight / 10) - 5, 10, 0, _backImage.Width, 5);

                g.DrawImage(_backImage, 0, titleHeight - (titleHeight / 10) - 5, Width, Height, 10, 5, _backImage.Width - 20, _backImage.Height - 25);
            }

        }
        protected virtual void DrawSysButton(Graphics g)
        {
            var image = CreateSysButtonBorderImage(SysButtons.Close, CloseState, this.WindowState);
            g.DrawImage(image, CloseRect);

            if (MaximizeBox)
                g.DrawImage(CreateSysButtonBorderImage(SysButtons.Maximize, MaxState, this.WindowState), MaxRect);
            if (MinimizeBox)
                g.DrawImage(CreateSysButtonBorderImage(SysButtons.Minimize, MinState, this.WindowState), MiniRect);
            if (SkinButtonVisible)
                g.DrawImage(CreateSysButtonBorderImage(SysButtons.Skin, SkinState, this.WindowState), SkinRect);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            if (ShowIcon)
            {
                using (Bitmap iconImage = this.Icon.ToBitmap())
                {
                    g.DrawImage(iconImage, this.IconRect);
                }
            }
            if (!string.IsNullOrEmpty(this.Text))
            {
                // TextRenderer.DrawText(g, this.Text, new Font(this.Font.FontFamily, Font.Size + 2, Font.Style | FontStyle.Bold, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont), this.TextRect, Color.White, TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, this.Text, new Font(this.Font.FontFamily, Font.Size + 1, Font.Style | FontStyle.Bold, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont), this.TextRect, TitleColor, TextFormatFlags.VerticalCenter);
            }

            DrawBorder(g);
            var _borderImage = Res.Current.GetImage("Resources.Form.fringe_bkg.png");
            g.DrawImage(_borderImage, new Rectangle(0, 0, 10, 10), new Rectangle(5, 5, 10, 10), GraphicsUnit.Pixel);//左上角
            g.DrawImage(_borderImage, new Rectangle(0, -5, 10, this.Height + 10), new Rectangle(5, 5, 10, _borderImage.Height - 10), GraphicsUnit.Pixel);//左边框
            g.DrawImage(_borderImage, new Rectangle(-5, this.Height - 10, 10, 10), new Rectangle(0, _borderImage.Height - 15, 10, 10), GraphicsUnit.Pixel);//左下角
            g.DrawImage(_borderImage, new Rectangle(this.Width - 9, -5, 10, 10), new Rectangle(20, 0, 10, 10), GraphicsUnit.Pixel);//右上角
            g.DrawImage(_borderImage, new Rectangle(this.Width - 9, -5, 10, this.Height + 10), new Rectangle(20, 5, 10, _borderImage.Height - 10), GraphicsUnit.Pixel);//右边框
            g.DrawImage(_borderImage, new Rectangle(this.Width - 9, this.Height - 10, 10, 10), new Rectangle(20, _borderImage.Height - 15, 10, 10), GraphicsUnit.Pixel);//右下角
            g.DrawImage(_borderImage, new Rectangle(5, -5, this.Width - 10, 18), new Rectangle(12, 0, 6, 18), GraphicsUnit.Pixel);
            g.DrawImage(_borderImage, new Rectangle(5, this.Height - 6, this.Width - 10, 18), new Rectangle(12, 0, 6, 18), GraphicsUnit.Pixel);
            DrawSysButton(g);
        }
        /// <summary>
        /// 拖动窗口大小。
        /// </summary>
        /// <param name="m"></param>
        protected virtual void DragFormSize(ref Message m)
        {
            bool isR = false;
            int param = m.LParam.ToInt32();
            Point point = new Point(param & 0xFFFF, param >> 16);
            point = PointToClient(point);
            if (this.WindowState != FormWindowState.Maximized)
            {
                if (IsResize)
                {
                    if (point.X <= 3)
                    {
                        if (point.Y <= 3)
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTTOPLEFT;
                            isR = true;
                        }
                        else if (point.Y > Height - 3)
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOMLEFT;
                            isR = true;
                        }
                        else
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTLEFT;
                            isR = true;
                        }
                    }
                    else if (point.X >= Width - 3)
                    {
                        if (point.Y <= 3)
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTTOPRIGHT;
                            isR = true;
                        }
                        else if (point.Y >= Height - 3)
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOMRIGHT;
                            isR = true;
                        }
                        else
                        {
                            m.Result = (IntPtr)WM_NCHITTEST.HTRIGHT;
                            isR = true;
                        }
                    }
                    else if (point.Y <= 3)
                    {
                        m.Result = (IntPtr)WM_NCHITTEST.HTTOP;
                        isR = true;
                    }
                    else if (point.Y >= Height - 3)
                    {
                        m.Result = (IntPtr)WM_NCHITTEST.HTBOTTOM;
                        isR = true;
                    }
                }
            }
            if (!isR)
            {
                if (TitleRect.Contains(point))
                {
                    if (!this.MaximizeBox
                        || CloseRect.Contains(point)
                        || MaxRect.Contains(point)
                        || MiniRect.Contains(point)
                        || SkinRect.Contains(point))
                    {
                        return;
                    }
                    m.Result = (IntPtr)2;
                }
            }
        }

        /// <summary>
        /// 处理 Windows 消息。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case (int)WindowsMessage.WM_NCPAINT:
                    case (int)WindowsMessage.WM_NCCALCSIZE:
                        break;
                    case (int)WindowsMessage.WM_NCACTIVATE:
                        if (m.WParam == (IntPtr)0 || m.WParam == (IntPtr)2097152)
                        {
                            m.Result = (IntPtr)1;
                        }
                        break;
                    case (int)WindowsMessage.WM_NCHITTEST:
                        base.WndProc(ref m);
                        this.DragFormSize(ref m);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
