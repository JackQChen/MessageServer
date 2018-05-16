using System.Drawing;
using System.Windows.Forms;
using AutoUpdate.Properties;

namespace AutoUpdate
{
    public class ListViewEx : ListView
    {
        SolidBrush bFocus;

        public ListViewEx()
        {
            this.OwnerDraw = true;
            this.FullRowSelect = true;
            this.View = View.Details;
            this.bFocus = new SolidBrush(Color.FromArgb(51, 153, 255));
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        int progressIndex = -1;

        public int ProgressColumIndex
        {
            set
            {
                progressIndex = value;
            }
            get
            {
                return progressIndex;
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == this.progressIndex)
            {
                float per = 0;
                if (float.TryParse(e.Item.SubItems[e.ColumnIndex].Text, out per))
                {
                    if (per >= 100)
                        per = 100;
                    if (e.Item.Focused)
                        e.Graphics.FillRectangle(bFocus, e.Bounds);
                    DrawProgress(e.Graphics, e.Bounds, per / 100);
                }
            }
            else
            {
                if (e.Item.Focused)
                    e.Graphics.FillRectangle(bFocus, e.Bounds);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                e.Graphics.DrawString(e.Item.SubItems[e.ColumnIndex].Text, this.Font, Brushes.Black,
                    new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2), sf);
            }
        }

        private void DrawProgress(Graphics g, Rectangle rect, float percent)
        {
            g.RendererBackground(Resources.Progress_Background, rect, 5);
            if (percent > 0)
            {
                var width = (int)(rect.Width * percent);
                if (width < 8)
                    width = 8;
                var rectProgress = new Rectangle(rect.X, rect.Y, width, rect.Height);
                g.RendererBackground(Resources.Progress_Progress, rectProgress, 5);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            g.DrawString(percent.ToString("p1"), this.Font, Brushes.DimGray, rect, sf);
        }
    }
}
