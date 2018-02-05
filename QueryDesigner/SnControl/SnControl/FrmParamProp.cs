namespace SnControl
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmParamProp : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private System.Windows.Forms.ComboBox cboDataSetList;
        private Container components = null;
        private SnDataSet curDataSet;
        private ChangeType FChangeType;
        private object FControl;
        private string _dataSetName;
        private string _paramName;
        private string _paramType;
        private string _function;
        private GroupBox groupBox1;
        public bool isInserting;
        private Label label2;
        private Label label5;
        private Label label8;
        private Solution solution;
        private ButtonEdit txtParamName;
        private ButtonEdit txtFunction;

        public FrmParamProp()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFunction = new DevExpress.XtraEditors.ButtonEdit();
            this.txtParamName = new DevExpress.XtraEditors.ButtonEdit();
            this.cboDataSetList = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFunction);
            this.groupBox1.Controls.Add(this.txtParamName);
            this.groupBox1.Controls.Add(this.cboDataSetList);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数属性";

            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // txtFunction
            // 
            this.txtFunction.EditValue = "";
            this.txtFunction.Location = new System.Drawing.Point(81, 82);
            this.txtFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.Size = new System.Drawing.Size(200, 21);
            this.txtFunction.TabIndex = 26;
            this.txtFunction.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtFunction_ButtonClick);
            // 
            // txtFieldName
            // 
            this.txtParamName.EditValue = "";
            this.txtParamName.Location = new System.Drawing.Point(81, 52);
            this.txtParamName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParamName.Name = "txtFieldName";
            this.txtParamName.Size = new System.Drawing.Size(200, 21);
            this.txtParamName.TabIndex = 25;
            this.txtParamName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtField_ButtonClick);
            // 
            // cboDataSetList
            // 
            this.cboDataSetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataSetList.Location = new System.Drawing.Point(81, 20);
            this.cboDataSetList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDataSetList.Name = "cboDataSetList";
            this.cboDataSetList.Size = new System.Drawing.Size(200, 20);
            this.cboDataSetList.TabIndex = 23;
            this.cboDataSetList.SelectedIndexChanged += new System.EventHandler(this.cboDataSetList_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(11, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 20);
            this.label8.TabIndex = 24;
            this.label8.Text = "数据集合：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(11, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "参数名称：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(11, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "执行方法：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnOk.Location = new System.Drawing.Point(120, 140);

            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定&O";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;

            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(200, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消&C";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmParamProp
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(290, 180);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmParamProp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询参数属性";
            this.Load += new System.EventHandler(this.frmParamProp_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmParamProp_KeyPress);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((this.FChangeType == ChangeType.PropGrid) && (this.FControl is ICommonAttribute))
            {
                ((ICommonAttribute)this.FControl).DataSetName = this._dataSetName;
                ((ICommonAttribute)this.FControl).ParamName = this._paramName;
                ((ICommonAttribute)this.FControl).ParamType = this._paramType;
                ((ICommonAttribute)this.FControl).Function = this._function;
            }
            base.DialogResult = DialogResult.OK;
        }

        private void frmParamProp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                base.SelectNextControl(base.ActiveControl, true, true, true, false);
            }
        }

        private void frmParamProp_Load(object sender, EventArgs e)
        {
            if (this.FChangeType == ChangeType.PropGrid)
            {
                this.SetValue();
            }
            this.cboDataSetList.Items.Clear();
            IEnumerator enumerator = this.solution.DataSetList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SnDataSet current = (SnDataSet)enumerator.Current;
                this.cboDataSetList.Items.Add(current.DataSetID + "-" + current.DataSetName);
            }
            this.cboDataSetList.Items.Add("所有数据集");
            if (this._dataSetName == null)
            {
                this.cboDataSetList.SelectedIndex = 0;
            }
            else
            {
                this.cboDataSetList.SelectedIndex = this.cboDataSetList.Items.IndexOf(this._dataSetName);
            }
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            if (cboDataSetList.SelectedIndex >= 0 && cboDataSetList.SelectedIndex < this.solution.DataSetList.Count)
                this.curDataSet = (SnDataSet)this.solution.DataSetList[this.cboDataSetList.SelectedIndex];
            else
                this.curDataSet = new SnDataSet();
            this.txtParamName.Text = this._paramName;
            this.txtFunction.Text = this._function;
        }

        private void SetValue()
        {
            if (this.FControl is ICommonAttribute)
            {
                this._dataSetName = ((ICommonAttribute)this.FControl).DataSetName;
                this._paramName = ((ICommonAttribute)this.FControl).ParamName;
                this._paramType = ((ICommonAttribute)this.FControl).ParamType;
                this._function = ((ICommonAttribute)this.FControl).Function;
            }
        }

        private void txtField_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            FrmSelectField fld = new FrmSelectField();
            fld.FieldList = this.curDataSet.FieldsList;
            fld.ParamList = this.curDataSet.ParamList;
            if (fld.ShowDialog() == DialogResult.OK)
            {
                this._paramName = fld.SelectedParam.ParamName;
                this._paramType = fld.SelectedParam.ParamType;
                this.txtParamName.Text = this._paramName;
            }
        }

        private void txtFunction_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            FrmSelectFunction function = new FrmSelectFunction();
            if (function.ShowDialog() == DialogResult.OK)
            {
                this._function = function.Function;
                this.txtFunction.Text = function.Function;
            }
        }

        public ChangeType ChangeFrom
        {
            set
            {
                this.FChangeType = value;
            }
        }

        public object Control
        {
            set
            {
                this.FControl = value;
            }
        }

        public string ParamName
        {
            get
            {
                return this._paramName;
            }
            set
            {
                this._paramName = value;
            }
        }

        public Solution QSolution
        {
            get
            {
                return this.solution;
            }
            set
            {
                this.solution = value;
            }
        }

        private void cboDataSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshInfo();
        }
    }
}
