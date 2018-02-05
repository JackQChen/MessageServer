using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DotNet.Windows.Forms
{
    public class InputTextBox :TextBox
    {
        [DllImport("gdi32.dll")]
        static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);
        [DllImport("user32.dll")]
        static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
       protected ListBox list = new ListBox();
       public InputTextBox()
       {
           SetClassLong(list.Handle, -26, GetClassLong(list.Handle, -26) | 0x20000);
           SetClassLong(list.Handle, -26, GetClassLong(list.Handle, -26) | 0x20000);
       }
       protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
       {
           
           if (list.Items.Count > 0)
           {
               if ((Keys)e.KeyValue == Keys.Down)
               {
                   e.IsInputKey = false;
                   this.SelectionStart = this.Text.Length;
                   if (list.SelectedItem == null)
                   {
                       list.SelectedIndex = 0;
                   }
                   else
                   {
                       if (list.Items.Count == list.SelectedIndex + 1)
                       {
                           list.SelectedItem = null;
                       }
                       else { list.SelectedIndex += 1; }
                   }
                   return;
               }
               else if ((Keys)e.KeyValue == Keys.Up)
               {
                   e.IsInputKey = false;
                   this.SelectionStart = this.Text.Length;
                   if (list.SelectedItem == null)
                   {
                       list.SelectedIndex = list.Items.Count - 1;
                   }
                   else
                   {
                       if (list.SelectedIndex == 0)
                       {
                           list.SelectedItem = null;
                       }
                       else { list.SelectedIndex -= 1; }
                   }
                   return;
               }
               else if ((Keys)e.KeyValue == Keys.Enter)
               {

                   if (list.SelectedItem != null)
                   {
                       isLostFocus = true;
                       this.Text = list.SelectedItem.ToString();
                       isLostFocus = false;
                       list.Items.Clear();
                       this.Parent.Controls.Remove(list);
                       var ss = this.Parent.GetNextControl(this, false);
                       this.Parent.GetNextControl(this, false).Focus();
                   }
               }
           }
           base.OnPreviewKeyDown(e);
       }
       bool isLostFocus = false;
       protected override void OnGotFocus(EventArgs e)
       {
           isLostFocus = false;
           base.OnGotFocus(e);
       }
       protected override void OnLostFocus(EventArgs e)
       {
           isLostFocus = true;
           if (list.SelectedItem != null)
           {
               this.Text = list.SelectedItem.ToString();
               list.Items.Clear();
               this.Parent.Controls.Remove(list);
           }
           if (list.Items.Count > 0)
           {
               this.Text = list.Items[0].ToString();
               list.Items.Clear();
               this.Parent.Controls.Remove(list);
           }
           this.Text = this.Text.ToUpper();
           if (Text.Length > 0)
           {
               new System.Threading.Thread(() => { Exec("update dbo.Dictionary set count=count+1 where name='" + Text[0] + "'"); }).Start();
           }
           isLostFocus = false;
           base.OnLostFocus(e);
       }
       private char lastChar = char.MinValue;
       protected override void OnTextChanged(EventArgs e)
       {
           if (this.DesignMode)
           {
               return;
           }
           base.OnTextChanged(e);
           if (Text.Length == 0 || isLostFocus)
           {
               lastChar = char.MinValue;
               list.Items.Clear();
               list.Visible = list.Items.Count > 0;
               return;
           }
           new System.Threading.Thread(() =>
           {
               list.Invoke(new Action(() =>
               {
                   list.Font = this.Font;

                   if (this.Text.Length > 0)
                   {
                       if (lastChar == Text[0])
                       {
                           if (list.Items.Count > 0)
                           {
                               for (int index = 0; index < list.Items.Count; index++)
                               {
                                   var value = Text.Remove(0, 1).ToUpper();
                                   if (value.Length > 0 && !value.Contains('-'))
                                   {
                                       value = value.Insert(1, "-");
                                   }
                                   list.Items[index] = list.Items[index].ToString().Substring(0, 1) + value;
                               }
                           }
                       }
                       else
                       {
                           lastChar = Text[0];
                           var table = GetData(lastChar.ToString());
                           list.Items.Clear();
                           foreach (System.Data.DataRow row in table.Rows)
                           {
                               var value = Text.Remove(0, 1).ToUpper();
                               if (value.Length > 0 && !value.Contains('-'))
                               {
                                   value = value.Insert(1, "-");
                               }
                               list.Items.Add(row["Name"].ToString() + value);
                           }
                       }
                   }

                   list.Width = this.Width;
                   list.Visible = list.Items.Count > 0;
                   if (list.Visible)
                   {
                       list.Top = this.Top + this.Height;
                       list.Left = this.Left;
                       list.FormattingEnabled = true;
                       if (!this.Parent.Controls.Contains(list))
                       {
                           this.Parent.Controls.Add(list);
                           this.Parent.Controls.SetChildIndex(list, 0);
                       }
                       list.Height = Convert.ToInt32(System.Math.Min(list.Items.Count, 8) * (list.Font.Size + 5)) + 10;
                   }
               }));

           }).Start();
       }
       protected System.Data.DataTable GetData(string value)
       {
           string str = "select name from dbo.Dictionary where flag=10 and (code like '" + value + "%' or name='" + value + "') order by count desc,len(code)";
           using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(@"Server=.;Database=EM_DB;Uid=sa;Pwd=qwe123!@#"))
           {
               using (System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand(str, conn))
               {
                   using (System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(comm))
                   {
                       System.Data.DataTable table = new System.Data.DataTable();
                       conn.Open();
                       ad.Fill(table);
                       conn.Close();
                       return table;
                   }
               }
           }
       }
       protected void Exec(string value)
       {
           string str = value;
           using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(@"Server=.;Database=EM_DB;Uid=sa;Pwd=123"))
           {
               using (System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand(str, conn))
               {
                   conn.Open();
                   comm.ExecuteNonQuery();
                   conn.Close();
               }
           }
       }
    }
}
