
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SnControl;

public class FrmSelectField : Form
{
    private Button btnCancel;
    private Button btnOk;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Container components = null;
    private FieldsCollections fieldList;
    private GroupBox groupBox1;
    private ListView lstFields;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private GroupBox groupBox2;
    private ListView lvPararm;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private FieldItem selectedField;

    public FrmSelectField()
    {
        this.InitializeComponent();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        base.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (this.tabControl1.SelectedIndex == 0)
        {
            if (this.lvPararm.SelectedItems.Count == 1)
            {
                this.SelectedParam = this.ParamList[this.lvPararm.SelectedItems[0].Text];
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
        }
        else
        {
            if (this.lstFields.SelectedItems.Count == 1)
            {
                this.selectedField = this.fieldList[this.lstFields.SelectedItems[0].SubItems[1].Text + "." + this.lstFields.SelectedItems[0].SubItems[2].Text];
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void frmSelectFld_Load(object sender, EventArgs e)
    {
        foreach (FieldItem item in this.fieldList)
        {
            ListViewItem item2 = this.lstFields.Items.Add(item.FieldName);
            item2.SubItems.Add(item.FieldChineseName);
        }
        foreach (SQLParamItem item in this.ParamList)
        {
            ListViewItem item2 = this.lvPararm.Items.Add(item.ParamName);
            item2.SubItems.Add(item.ParamType);
        }
    }

    private void InitializeComponent()
    {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstFields = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvPararm = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstFields);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 258);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "双击选择字段";
            // 
            // lstFields
            // 
            this.lstFields.AllowColumnReorder = true;
            this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.lstFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFields.FullRowSelect = true;
            this.lstFields.GridLines = true;
            this.lstFields.Location = new System.Drawing.Point(3, 17);
            this.lstFields.MultiSelect = false;
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(398, 238);
            this.lstFields.TabIndex = 2;
            this.lstFields.UseCompatibleStateImageBehavior = false;
            this.lstFields.View = System.Windows.Forms.View.Details;
            this.lstFields.DoubleClick += new System.EventHandler(this.lstFields_DoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "字段名称";
            this.columnHeader2.Width = 95;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "中文名称";
            this.columnHeader3.Width = 105;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnOk.Location = new System.Drawing.Point(224, 303);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定&O";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnCancel.Location = new System.Drawing.Point(320, 303);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消&C";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(418, 290);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(410, 264);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "选择参数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvPararm);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(404, 258);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "双击选择参数";
            // 
            // lvPararm
            // 
            this.lvPararm.AllowColumnReorder = true;
            this.lvPararm.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.lvPararm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPararm.FullRowSelect = true;
            this.lvPararm.GridLines = true;
            this.lvPararm.Location = new System.Drawing.Point(3, 17);
            this.lvPararm.MultiSelect = false;
            this.lvPararm.Name = "lvPararm";
            this.lvPararm.Size = new System.Drawing.Size(398, 238);
            this.lvPararm.TabIndex = 2;
            this.lvPararm.UseCompatibleStateImageBehavior = false;
            this.lvPararm.View = System.Windows.Forms.View.Details;
            this.lvPararm.DoubleClick += new System.EventHandler(this.lstFields_DoubleClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "参数名";
            this.columnHeader5.Width = 140;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "参数类型";
            this.columnHeader6.Width = 140;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(410, 264);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "选择字段";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // FrmSelectField
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(418, 340);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectField";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "引用位置";
            this.Load += new System.EventHandler(this.frmSelectFld_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private void lstFields_DoubleClick(object sender, EventArgs e)
    {
        this.btnOk_Click(sender, e);
    }

    public FieldsCollections FieldList
    {
        set
        {
            this.fieldList = value;
        }
    }

    public FieldItem SelectedField
    {
        get
        {
            return this.selectedField;
        }
        set
        {
            this.selectedField = value;
        }
    }

    public SQLParamCollections ParamList { get; set; }

    public SQLParamItem SelectedParam { get; set; }

}