using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualControls.Controls;
using System.Drawing;
using System.Windows.Forms;
using VirtualControls.Properties;

namespace VirtualControls.UserControls
{
    public class VirtualButton : VirtualControl
    {
        public string Text { get; set; }
        bool isEnter = false, isMouseDown = false;

        public VirtualButton()
        {
            this.MouseEnter += new MouseEventHandler(VirtualButton_MouseEnter);
            this.MouseLeave += new MouseEventHandler(VirtualButton_MouseLeave);
            this.MouseDown += new MouseEventHandler(VirtualButton_MouseDown);
            this.MouseUp += new MouseEventHandler(VirtualButton_MouseUp);
        }

        void VirtualButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.isMouseDown = true;
            this.Refresh();
        }

        void VirtualButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.isMouseDown = false;
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
            isMouseDown = false;
            this.Refresh();
        }

        public override void Draw(Graphics g)
        {
            //ControlPaint.DrawButton(g, rect, isMouseDown ? ButtonState.Checked : ButtonState.Normal);
            //g.FillRectangle(SystemBrushes.Control, this.Rectangle);
            var rect = this.Rectangle;
            if (isEnter)
            {
                if (isMouseDown)
                {
                    rect.Offset(1, 1);
                    g.RendererBackground(Resources.btn_3, rect, 5);
                }
                else
                {
                    g.RendererBackground(Resources.btn_1, rect, 5);
                }
            }
            else
                g.RendererBackground(Resources.btn_2, rect, 5);
            g.DrawString(this.Text, this.Font, Brushes.Black, rect, StringFormat);
        }
    }
}
