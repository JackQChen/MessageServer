using System.Drawing;
using System.Windows.Forms;

namespace VirtualControls.Controls
{
    public class VirtualControl
    {
        public Rectangle Rectangle { get; set; }
        public object Tag { get; set; }
        public string Name { get; set; }
        public Font Font { get; set; }
        public VirtualControlContainer Parent { get; set; }
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;

        static StringFormat sf;
        public static StringFormat StringFormat
        {
            get
            {
                if (sf == null)
                {
                    sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.FormatFlags = StringFormatFlags.LineLimit;
                }
                return sf;
            }
        }

        public VirtualControl()
        {
            this.Name = "";
            this.Font = new Font("宋体", 9);
        }
        public void OnMouseClick(MouseEventArgs e)
        {
            if (this.MouseClick != null)
                this.MouseClick(this, e);
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            if (this.MouseDown != null)
                this.MouseDown(this, e);
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
                this.MouseUp(this, e);
        }
        public void OnMouseEnter(MouseEventArgs e)
        {
            if (this.MouseEnter != null)
                this.MouseEnter(this, e);
        }
        public void OnMouseLeave(MouseEventArgs e)
        {
            if (this.MouseLeave != null)
                this.MouseLeave(this, e);
        }

        public virtual void Draw(Graphics g)
        {
        }

        public void Refresh()
        {
            this.Draw(this.Parent.graphics);
            this.Parent.Invalidate(this.Rectangle);
        }
    }
}
