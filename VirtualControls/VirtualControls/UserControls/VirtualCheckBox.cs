using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VirtualControls.Controls;
using VirtualControls.Properties;

namespace VirtualControls.UserControls
{
    public class VirtualCheckBox : VirtualControl
    {
        Rectangle rectChk, rectText;
        bool isEnter = false, isChecked = false;
        public string Text { get; set; }
        Dictionary<string, GraphicsState> dicState = new Dictionary<string, GraphicsState>();

        public bool Checked { get { return this.isChecked; } }

        public VirtualCheckBox()
        {
            this.MouseEnter += new MouseEventHandler(VirtualButton_MouseEnter);
            this.MouseLeave += new MouseEventHandler(VirtualButton_MouseLeave);
            this.MouseDown += new MouseEventHandler(VirtualButton_MouseDown);
        }

        void VirtualButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.isChecked = !this.isChecked;
            this.Refresh();
        }

        void VirtualButton_MouseEnter(object sender, MouseEventArgs e)
        {
            isEnter = true;
            this.Refresh();
        }

        void VirtualButton_MouseLeave(object sender, MouseEventArgs e)
        {
            isEnter = false;
            this.Refresh();
        }

        public override void Draw(Graphics g)
        {
            if (rectChk == Rectangle.Empty)
            {
                rectChk = new Rectangle(this.Rectangle.X, this.Rectangle.Y, this.Rectangle.Height, this.Rectangle.Height);
                rectText = new Rectangle(this.Rectangle.X + this.rectChk.Width, this.Rectangle.Y, this.Rectangle.Width - this.rectChk.Width, this.Rectangle.Height);
            }
            if (isChecked)
            {
                g.RendererBackground(isEnter ? Resources.chk_4 : Resources.chk_3, rectChk, 5);
            }
            else
            {
                g.RendererBackground(isEnter ? Resources.chk_1 : Resources.chk_2, rectChk, 5);
            }
            var sf = StringFormat.Clone() as StringFormat;
            sf.Alignment = StringAlignment.Near;
            g.DrawString(this.Text, this.Font, Brushes.Black, rectText, sf);
        }
    }
}
