using System;
using System.Collections.Generic;
using System.Drawing; 
using System.Text;
using System.Windows.Forms;

namespace CustomSkin.Windows.Forms
{
    public class Panel :System.Windows.Forms.Panel
    {
        public Panel()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
            this.BackColor = Color.Transparent;
        }
    }
}
