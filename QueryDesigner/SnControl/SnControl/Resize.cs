namespace SnControl
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class Resize
    {
        private Control ctrl;
        private int ctrlHeight;
        private int ctrlLastLeft;
        private int ctrlLastTop;
        private int ctrlLeft;
        private Rectangle ctrlRectangle = new Rectangle();
        private int ctrlTop;
        private int ctrlWidth;
        private int cursorL;
        private int cursorT;
        private Panel frm;
        private bool IsMoving = false;

        public Resize(Control c, Panel frm)
        {
            this.ctrl = c;
            this.frm = frm;
            this.ctrl.MouseDown += new MouseEventHandler(this.MouseDown);
            this.ctrl.MouseMove += new MouseEventHandler(this.MouseMove);
            this.ctrl.MouseUp += new MouseEventHandler(this.MouseUp);
            this.ctrl = c.Parent;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (this.frm != null)
            {
                this.IsMoving = true;
                this.ctrlLeft = this.ctrl.PointToScreen(this.ctrl.Location).X - this.ctrl.Left;
                this.ctrlTop = this.ctrl.PointToScreen(this.ctrl.Location).Y - this.ctrl.Top;
                this.cursorL = Cursor.Position.X;
                this.cursorT = Cursor.Position.Y;
                this.ctrlWidth = this.ctrl.Width;
                this.ctrlHeight = this.ctrl.Height;
                this.ctrlRectangle.Location = new Point(this.ctrlLeft, this.ctrlTop);
                this.ctrlRectangle.Size = new Size(this.ctrlWidth, this.ctrlHeight);
                ControlPaint.DrawReversibleFrame(this.ctrlRectangle, Color.Aqua, FrameStyle.Thick);
                for (int i = 0; i < this.frm.Controls.Count; i++)
                {
                    this.frm.Controls[i].Invalidate();
                }
                this.frm.Invalidate();
                this.frm.FindForm().Invalidate();
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (((this.frm != null) && (e.Button == MouseButtons.Left)) && this.IsMoving)
            {
                if (this.ctrlLastLeft == 0)
                {
                    this.ctrlLastLeft = this.ctrlLeft;
                }
                if (this.ctrlLastTop == 0)
                {
                    this.ctrlLastTop = this.ctrlTop;
                }
                int x = ((Cursor.Position.X - this.cursorL) + this.ctrl.PointToScreen(this.ctrl.Location).X) - this.ctrl.Left;
                int y = ((Cursor.Position.Y - this.cursorT) + this.ctrl.PointToScreen(this.ctrl.Location).Y) - this.ctrl.Top;
                if (x < this.frm.PointToScreen(new Point(0, 0)).X)
                {
                    x = this.frm.PointToScreen(new Point(0, 0)).X;
                }
                if (y < this.frm.PointToScreen(new Point(0, 0)).Y)
                {
                    y = this.frm.PointToScreen(new Point(0, 0)).Y;
                }
                this.ctrlLeft = x;
                this.ctrlTop = y;
                this.ctrlRectangle.Location = new Point(this.ctrlLastLeft, this.ctrlLastTop);
                this.ctrlRectangle.Size = new Size(this.ctrlWidth, this.ctrlHeight);
                ControlPaint.DrawReversibleFrame(this.ctrlRectangle, Color.Aqua, FrameStyle.Thick);
                this.ctrlLastLeft = this.ctrlLeft;
                this.ctrlLastTop = this.ctrlTop;
                this.ctrlRectangle.Location = new Point(this.ctrlLeft, this.ctrlTop);
                this.ctrlRectangle.Size = new Size(this.ctrlWidth, this.ctrlHeight);
                ControlPaint.DrawReversibleFrame(this.ctrlRectangle, Color.Aqua, FrameStyle.Thick);
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if ((this.frm != null) && this.IsMoving)
            {
                this.ctrlRectangle.Location = new Point(this.ctrlLeft, this.ctrlTop);
                this.ctrlRectangle.Size = new Size(this.ctrlWidth, this.ctrlHeight);
                ControlPaint.DrawReversibleFrame(this.ctrlRectangle, Color.Aqua, FrameStyle.Thick);
                this.ctrl.Left = this.ctrl.PointToClient(this.ctrlRectangle.Location).X + this.ctrl.Left;
                this.ctrl.Top = this.ctrl.PointToClient(this.ctrlRectangle.Location).Y + this.ctrl.Top;
                this.IsMoving = false;
                for (int i = 0; i < this.frm.Controls.Count; i++)
                {
                    this.frm.Controls[i].Invalidate();
                }
                this.frm.Invalidate();
                this.frm.FindForm().Invalidate();
            }
        }
    }
}

