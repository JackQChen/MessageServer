﻿using System.Windows.Forms;
namespace MessageServer.Controls
{
    public class TabControlEx : TabControl
    {

        public TabControlEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                , true);
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }
}