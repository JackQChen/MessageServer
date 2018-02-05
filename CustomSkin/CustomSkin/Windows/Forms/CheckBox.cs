using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public class CheckBox : ControlBase
    {
        public CheckBox()
        {
            this.color = this.ForeColor;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.color = this.Enabled ? this.ForeColor : Color.LightGray;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Checked = !this.Checked;
            if (this.CheckedChanged != null)
                this.CheckedChanged(this, EventArgs.Empty);
        }

        Rectangle rectText = Rectangle.Empty, rectImg = Rectangle.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.rectText = new Rectangle(17, 0, this.Width - 17, this.Height);
            this.rectImg = new Rectangle(0, (this.Height - 17) / 2, 17, 17);
        }

        TextFormatFlags flags = TextFormatFlags.Left |
                                    TextFormatFlags.SingleLine |
                                    TextFormatFlags.VerticalCenter;
        Color color;
        [Category("自定义"), Description("是否选中")]
        public bool Checked { get; set; }
        [Category("自定义"), Description("选中状态发生变化")]
        public event EventHandler CheckedChanged;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.rectText, color, flags);
            if (!this.Enabled)
            {
                g.DrawImage(Res.Current.GetImage("Resources.CheckBox.normal.png"), this.rectImg);
            }
            else
            {
                switch (this.MouseState)
                {
                    case MouseState.Normal:
                    case MouseState.Leave:
                        g.DrawImage(Res.Current.GetImage(
                           this.Checked ?
                           "Resources.CheckBox.tick_normal.png" :
                          "Resources.CheckBox.normal.png"), this.rectImg);
                        break;
                    case MouseState.Down:
                    case MouseState.Up:
                    case MouseState.Enter:
                        g.DrawImage(Res.Current.GetImage(
                            this.Checked ?
                            "Resources.CheckBox.tick_highlight.png" :
                            "Resources.CheckBox.hightlight.png"), this.rectImg);
                        break;
                }
                //if (this.CheckState == CheckState.Indeterminate)
                //{
                //    g.DrawImage(Res.Current.GetImage(
                //        this.MouseState == MouseState.Down || this.MouseState == MouseState.Enter ?
                //        "Resources.CheckBox._tick_normal.png" :
                //        "Resources.CheckBox._tick_highlight.png"
                //        ), this.rectImg);
                //}
            }
        }
    }
}
