using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SnControl
{
    [ToolboxBitmap(typeof(PictureBox))]
    public class TitlePanel : Panel
    {
        private class Consts
        {
            public const int DefaultHeight = 26;
            public const string DefaultFontName = "Tahoma";
            public const int DefaultFontSize = 12;
            public const int PosOffset = 4;
        }
        private Container components = null;
        private bool active = false;
        private bool antiAlias = true;
        private bool allowActive = true;
        private string text = "";
        private Color colorActiveText = Color.Black;
        private Color colorInactiveText = Color.White;
        private Color colorActiveLow = Color.FromArgb(255, 165, 78);
        private Color colorActiveHigh = Color.FromArgb(255, 225, 155);
        private Color colorInactiveLow = Color.FromArgb(3, 55, 145);
        private Color colorInactiveHigh = Color.FromArgb(90, 135, 215);
        private SolidBrush brushActiveText;
        private SolidBrush brushInactiveText;
        private LinearGradientBrush brushActive;
        private LinearGradientBrush brushInactive;
        private StringFormat format;
        public string Caption
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                base.Invalidate();
            }
        }
        public override string Text
        {
            get
            {
                return this.Caption;
            }
            set
            {
                this.Caption = value;
            }
        }
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
                base.Invalidate();
            }
        }
        public bool AllowActive
        {
            get
            {
                return this.allowActive;
            }
            set
            {
                this.allowActive = value;
                base.Invalidate();
            }
        }
        public bool AntiAlias
        {
            get
            {
                return this.antiAlias;
            }
            set
            {
                this.antiAlias = value;
                base.Invalidate();
            }
        }
        public Color ActiveTextColor
        {
            get
            {
                return this.colorActiveText;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.Black;
                }
                this.colorActiveText = value;
                this.brushActiveText = new SolidBrush(this.colorActiveText);
                base.Invalidate();
            }
        }
        public Color InactiveTextColor
        {
            get
            {
                return this.colorInactiveText;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.White;
                }
                this.colorInactiveText = value;
                this.brushInactiveText = new SolidBrush(this.colorInactiveText);
                base.Invalidate();
            }
        }
        public Color ActiveGradientLowColor
        {
            get
            {
                return this.colorActiveLow;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(255, 165, 78);
                }
                this.colorActiveLow = value;
                this.CreateGradientBrushes();
                base.Invalidate();
            }
        }
        public Color ActiveGradientHighColor
        {
            get
            {
                return this.colorActiveHigh;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(255, 225, 155);
                }
                this.colorActiveHigh = value;
                this.CreateGradientBrushes();
                base.Invalidate();
            }
        }
        public Color InactiveGradientLowColor
        {
            get
            {
                return this.colorInactiveLow;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(3, 55, 145);
                }
                this.colorInactiveLow = value;
                this.CreateGradientBrushes();
                base.Invalidate();
            }
        }
        public Color InactiveGradientHighColor
        {
            get
            {
                return this.colorInactiveHigh;
            }
            set
            {
                if (value.Equals(Color.Empty))
                {
                    value = Color.FromArgb(90, 135, 215);
                }
                this.colorInactiveHigh = value;
                this.CreateGradientBrushes();
                base.Invalidate();
            }
        }
        private SolidBrush TextBrush
        {
            get
            {
                if (this.active && this.allowActive)
                {
                    return this.brushActiveText;
                }
                return this.brushInactiveText;
            }
        }
        private LinearGradientBrush BackBrush
        {
            get
            {
                if (this.active && this.allowActive)
                {
                    return this.brushActive;
                }
                return this.brushInactive;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawCaption(e.Graphics);
            base.OnPaint(e);
        }
        private void DrawCaption(Graphics g)
        {
            g.FillRectangle(this.BackBrush, this.DisplayRectangle);
            if (this.antiAlias)
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            }
            RectangleF rectangleF = new RectangleF(4f, 0f, (float)(this.DisplayRectangle.Width - 4), (float)this.DisplayRectangle.Height);
            g.DrawString(this.text, this.Font, this.TextBrush, rectangleF, this.format);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.allowActive)
            {
                base.Focus();
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.CreateGradientBrushes();
        }
        private void CreateGradientBrushes()
        {
            if (base.Width > 0 && base.Height > 0)
            {
                if (this.brushActive != null)
                {
                    this.brushActive.Dispose();
                }
                this.brushActive = new LinearGradientBrush(this.DisplayRectangle, this.colorActiveHigh, this.colorActiveLow, 1);
                if (this.brushInactive != null)
                {
                    this.brushInactive.Dispose();
                }
                this.brushInactive = new LinearGradientBrush(this.DisplayRectangle, this.colorInactiveHigh, this.colorInactiveLow, 1);
            }
        }
        public TitlePanel()
        {
            this.InitializeComponent();
            base.SetStyle((ControlStyles)73746, true);
            this.format = new StringFormat();
            this.format.FormatFlags = StringFormatFlags.NoWrap;
            this.format.LineAlignment = StringAlignment.Center;
            this.format.Trimming = StringTrimming.EllipsisCharacter;
            this.Font = new Font("Tahoma", 12f, FontStyle.Bold);
            this.ActiveTextColor = this.colorActiveText;
            this.InactiveTextColor = this.colorInactiveText;
            this.CreateGradientBrushes();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            base.Name = "TitlePanel";
            this.Dock = DockStyle.Top;
            this.Height = 40;
        }
    }
}
