namespace SnControl
{
    using QueryDataAccess;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmSelectFunction : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private Container components = null;
        private QueryDAO dao;
        private DataTable dtMethod;
        private string function;
        private GroupBox groupBox1;
        private ListView listView1;
        private ColumnHeader 返回值;
        private ColumnHeader 方法名称;
        private ColumnHeader 描述;

        public FrmSelectFunction()
        {
            this.InitializeComponent();
            this.dao = new QueryDAO();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.GetValue();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSelectFunction_Load(object sender, EventArgs e)
        {
            this.GetData();
        }

        private void GetData()
        {
            this.dtMethod = this.dao.GetMethodList();
            foreach (DataRow row in this.dtMethod.Rows)
            {
                ListViewItem item = this.listView1.Items.Add(row["MID"].ToString());
                item.SubItems.Add(row["COMMENTARY"].ToString());
                item.SubItems.Add(row["VALUETYPE"].ToString());
            }
            this.listView1.TopItem.Selected = true;
        }

        private void GetValue()
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                this.function = this.listView1.SelectedItems[0].Text;
                base.DialogResult = DialogResult.OK;
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.listView1 = new ListView();
            this.描述 = new ColumnHeader();
            this.返回值 = new ColumnHeader();
            this.方法名称 = new ColumnHeader();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new Point(4, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x16c, 0xf8);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOk.FlatStyle = FlatStyle.System;
            this.btnOk.ImageAlign = ContentAlignment.BottomLeft;
            this.btnOk.Location = new Point(160, 0x100);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x58, 0x17);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定&O";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatStyle = FlatStyle.System;
            this.btnCancel.ImageAlign = ContentAlignment.BottomLeft;
            this.btnCancel.Location = new Point(0x100, 0x100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x58, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消&C";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.listView1.Columns.AddRange(new ColumnHeader[] { this.方法名称, this.描述, this.返回值 });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new Point(3, 0x11);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x166, 0xe4);
            this.listView1.TabIndex = 0;
            this.listView1.View = View.Details;
            this.listView1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
            this.描述.Text = "描述";
            this.描述.Width = 0x89;
            this.返回值.Text = "返回值";
            this.方法名称.Text = "方法名称";
            this.方法名称.Width = 0x85;
            base.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(370, 0x11d);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmSelectFunction";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "选择方法";
            base.Load += new EventHandler(this.frmSelectFunction_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.GetValue();
        }

        public string Function
        {
            get
            {
                return this.function;
            }
            set
            {
                this.function = value;
            }
        }
    }
}

