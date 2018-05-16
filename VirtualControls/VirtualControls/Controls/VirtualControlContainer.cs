using System.Drawing;
using System.Windows.Forms;

namespace VirtualControls.Controls
{
    public class VirtualControlContainer : Control
    {
        internal Graphics graphics;

        public VirtualControlContainer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                       | ControlStyles.OptimizedDoubleBuffer
                       | ControlStyles.SupportsTransparentBackColor
                       , true);
            this.MouseClick += new MouseEventHandler(VirtualControlContainer_MouseClick);
            this.MouseDown += new MouseEventHandler(VirtualControlContainer_MouseDown);
            this.MouseUp += new MouseEventHandler(VirtualControlContainer_MouseUp);
            this.MouseMove += new MouseEventHandler(VirtualControlContainer_MouseMove);
            this.Controls = new Controls(this);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public new Controls Controls { get; set; }

        public Image Image { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image != null)
                e.Graphics.DrawImage(this.Image, Point.Empty);
        }

        public new void Refresh()
        {
            this.Image = new Bitmap(this.Width, this.Height);
            this.graphics = Graphics.FromImage(this.Image);
            foreach (var ctl in this.Controls)
            {
                ctl.Draw(graphics);
            }
            this.Invalidate();
        }

        void VirtualControlContainer_MouseClick(object sender, MouseEventArgs e)
        {
            var ctl = this.Controls.Find(m => m.Rectangle.Contains(e.Location));
            if (ctl != null)
                ctl.OnMouseClick(e);
        }

        void VirtualControlContainer_MouseDown(object sender, MouseEventArgs e)
        {
            var ctl = this.Controls.Find(m => m.Rectangle.Contains(e.Location));
            if (ctl != null)
                ctl.OnMouseDown(e);
        }

        void VirtualControlContainer_MouseUp(object sender, MouseEventArgs e)
        {
            var ctl = this.Controls.Find(m => m.Rectangle.Contains(e.Location));
            if (ctl != null)
                ctl.OnMouseUp(e);
        }

        VirtualControl ctlIn = null;
        void VirtualControlContainer_MouseMove(object sender, MouseEventArgs e)
        {
            var ctl = this.Controls.Find(m => m.Rectangle.Contains(e.Location));
            if (ctl != ctlIn)
            {
                if (ctlIn != null)
                    ctlIn.OnMouseLeave(e);
                ctlIn = ctl;
                if (ctlIn != null)
                    ctlIn.OnMouseEnter(e);
            }
        }
    }
}
