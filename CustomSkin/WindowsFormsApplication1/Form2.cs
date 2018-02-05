using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CustomSkin.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form2 : FormBase
    {
        public Form2()
        {
            this.InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.menuStrip1.Renderer = new CustomSkin.Windows.Forms.ToolStripRenderers(true);
            this.statusStrip1.Renderer = new CustomSkin.Windows.Forms.ToolStripRenderers();
            //this.statusStrip1.SizeGripBounds
        }

        class myStatusStrip : StatusStrip
        {
            public myStatusStrip()
            {
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 132 && this.SizingGrip)
                {
                    Rectangle sizeGripBounds = this.SizeGripBounds;
                    int param = m.LParam.ToInt32();
                    Point point = new Point(param & 0xFFFF, param >> 16);
                    if (sizeGripBounds.Contains(this.PointToClient(point)))
                    {
                        m.Result = (IntPtr)17;
                        return;
                    }
                }
                else
                    base.WndProc(ref m);
            }
        }
    }
}
