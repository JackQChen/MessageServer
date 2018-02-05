//using System.Drawing;
//using System.Windows.Forms;
//using CustomSkin.Drawing;

//namespace CustomSkin.Windows.Forms
//{
//    public class ComboBox : System.Windows.Forms.ComboBox
//    {
//        public ComboBox()
//        {
//            //this.SetStyle(
//            //    ControlStyles.AllPaintingInWmPaint |
//            //    ControlStyles.OptimizedDoubleBuffer |
//            //    ControlStyles.ResizeRedraw |
//            //    ControlStyles.Selectable |
//            //    ControlStyles.DoubleBuffer |
//            //    ControlStyles.SupportsTransparentBackColor |
//            //    ControlStyles.UserPaint, true); 
//            //this.UpdateStyles();
//            this.FlatStyle = FlatStyle.Flat;
//        }

//        protected override void OnPaint(PaintEventArgs e)
//        {
//            Rectangle IconRect = new Rectangle(0, 0, this.Width, this.Height);
//            e.Graphics.RendererBackground(IconRect, 3, Res.Current.GetImage("Resources.TextBox.TextBox_move.png"));

//            e.Graphics.FillRectangle(Brushes.Black, 0, 0, 100, 100);
//            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, IconRect, this.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
//        }

//        protected override void OnPaintBackground(PaintEventArgs pevent)
//        {
//            //base.OnPaintBackground(pevent);
//            pevent.Graphics.FillRectangle(Brushes.Black, 0, 0, 100, 100);

//        } 
//    }
//}
