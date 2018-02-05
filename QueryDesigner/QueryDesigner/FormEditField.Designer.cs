namespace QueryDesigner
{
    partial class FormEditField
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFieldName = new System.Windows.Forms.TextBox();
            this.txtChineseName = new System.Windows.Forms.TextBox();
            this.cboCalcType = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStyle = new System.Windows.Forms.Button();
            this.btnAddStyle = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comConverge = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.displayWidth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDigits = new System.Windows.Forms.TextBox();
            this.txtFieldType = new System.Windows.Forms.ComboBox();
            this.chkVisible = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "统计类型";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "字段类型";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "中文名称";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "字段名称";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFieldName
            // 
            this.txtFieldName.Location = new System.Drawing.Point(77, 10);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(109, 21);
            this.txtFieldName.TabIndex = 1;
            // 
            // txtChineseName
            // 
            this.txtChineseName.Location = new System.Drawing.Point(251, 10);
            this.txtChineseName.Name = "txtChineseName";
            this.txtChineseName.Size = new System.Drawing.Size(109, 21);
            this.txtChineseName.TabIndex = 3;
            // 
            // cboCalcType
            // 
            this.cboCalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCalcType.FormattingEnabled = true;
            this.cboCalcType.Items.AddRange(new object[] {
            "None",
            "Sum",
            "Count",
            "Average",
            "Max",
            "Min"});
            this.cboCalcType.Location = new System.Drawing.Point(251, 37);
            this.cboCalcType.Name = "cboCalcType";
            this.cboCalcType.Size = new System.Drawing.Size(109, 20);
            this.cboCalcType.TabIndex = 7;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(197, 116);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 16;
            this.btnConfirm.Text = "确  定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取  消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnStyle
            // 
            this.btnStyle.Location = new System.Drawing.Point(109, 116);
            this.btnStyle.Name = "btnStyle";
            this.btnStyle.Size = new System.Drawing.Size(75, 23);
            this.btnStyle.TabIndex = 15;
            this.btnStyle.Text = "编辑样式";
            this.btnStyle.UseVisualStyleBackColor = true;
            this.btnStyle.Click += new System.EventHandler(this.btnStyle_Click);
            // 
            // btnAddStyle
            // 
            this.btnAddStyle.Location = new System.Drawing.Point(20, 116);
            this.btnAddStyle.Name = "btnAddStyle";
            this.btnAddStyle.Size = new System.Drawing.Size(75, 23);
            this.btnAddStyle.TabIndex = 14;
            this.btnAddStyle.Text = "添加样式";
            this.btnAddStyle.UseVisualStyleBackColor = true;
            this.btnAddStyle.Click += new System.EventHandler(this.btnAddStyle_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "聚合类型";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comConverge
            // 
            this.comConverge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comConverge.FormattingEnabled = true;
            this.comConverge.Items.AddRange(new object[] {
            "None",
            "Sum",
            "Count"});
            this.comConverge.Location = new System.Drawing.Point(77, 64);
            this.comConverge.Name = "comConverge";
            this.comConverge.Size = new System.Drawing.Size(109, 20);
            this.comConverge.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(192, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "显示宽度";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // displayWidth
            // 
            this.displayWidth.Location = new System.Drawing.Point(251, 63);
            this.displayWidth.Name = "displayWidth";
            this.displayWidth.Size = new System.Drawing.Size(109, 21);
            this.displayWidth.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "小数位数";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDigits
            // 
            this.txtDigits.Location = new System.Drawing.Point(77, 90);
            this.txtDigits.Name = "txtDigits";
            this.txtDigits.Size = new System.Drawing.Size(107, 21);
            this.txtDigits.TabIndex = 13;
            // 
            // txtFieldType
            // 
            this.txtFieldType.FormattingEnabled = true;
            this.txtFieldType.Items.AddRange(new object[] {
            "TEXT",
            "INT",
            "DECIMAL",
            "DATETIME"});
            this.txtFieldType.Location = new System.Drawing.Point(77, 37);
            this.txtFieldType.Name = "txtFieldType";
            this.txtFieldType.Size = new System.Drawing.Size(109, 20);
            this.txtFieldType.TabIndex = 18;
            // 
            // chkVisible
            // 
            this.chkVisible.AutoSize = true;
            this.chkVisible.Location = new System.Drawing.Point(288, 92);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(72, 16);
            this.chkVisible.TabIndex = 19;
            this.chkVisible.Text = "显示该列";
            this.chkVisible.UseVisualStyleBackColor = true;
            // 
            // FormEditField
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(378, 151);
            this.Controls.Add(this.chkVisible);
            this.Controls.Add(this.txtFieldType);
            this.Controls.Add(this.txtDigits);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.displayWidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comConverge);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnAddStyle);
            this.Controls.Add(this.btnStyle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.cboCalcType);
            this.Controls.Add(this.txtChineseName);
            this.Controls.Add(this.txtFieldName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditField";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "字段属性";
            this.Load += new System.EventHandler(this.FormEditField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFieldName;
        private System.Windows.Forms.TextBox txtChineseName;
        private System.Windows.Forms.ComboBox cboCalcType;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStyle;
        private System.Windows.Forms.Button btnAddStyle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comConverge;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox displayWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDigits;
        private System.Windows.Forms.ComboBox txtFieldType;
        private System.Windows.Forms.CheckBox chkVisible;
    }
}