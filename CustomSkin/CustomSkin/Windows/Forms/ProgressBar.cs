using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomSkin.Drawing;

namespace CustomSkin.Windows.Forms
{
    public class ProgressBar : ControlBase
    {
        int value = 0, max = 100;

        [Category("自定义"), Description("当前进度")]
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.Invalidate();
            }
        }
        [Category("自定义"), Description("总进度")]
        public int Minimum { get; set; }

        [Category("自定义"), Description("总进度")]
        public int Maximum
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
                this.Invalidate();
            }
        }

        string strNormal = "Resources.ProgressBar.AFStaticProgress_Background.png",
            strValue = "Resources.ProgressBar.AFStaticProgress_Green.png";

        public ProgressBar()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.UpdateStyles();
        }

        Rectangle rectAll = Rectangle.Empty, rectValue = Rectangle.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.rectAll.Size = this.Size;
            this.rectValue.Height = this.Height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.RendererBackground(this.rectAll, 5, Res.Current.GetImage(strNormal));
            rectValue.Width = Convert.ToInt32((double)this.Width / this.Maximum * this.Value);
            if (rectValue.Width > 0)
                g.RendererBackground(this.rectValue, 5, Res.Current.GetImage(strValue));
        }
    }
}
