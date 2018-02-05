namespace QueryDesigner
{
    partial class FormNewStyle
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
            this.button2 = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dateValue2 = new System.Windows.Forms.DateTimePicker();
            this.comValue2 = new System.Windows.Forms.ComboBox();
            this.txtValue2 = new System.Windows.Forms.TextBox();
            this.comType2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dateValue1 = new System.Windows.Forms.DateTimePicker();
            this.comValue1 = new System.Windows.Forms.ComboBox();
            this.txtValue1 = new System.Windows.Forms.TextBox();
            this.comType1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comCondition = new System.Windows.Forms.ComboBox();
            this.checkApplyToRow = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFore = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(332, 296);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "取  消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(111, 296);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确  定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dateValue2);
            this.groupBox4.Controls.Add(this.comValue2);
            this.groupBox4.Controls.Add(this.txtValue2);
            this.groupBox4.Controls.Add(this.comType2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(277, 170);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 100);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "值二";
            // 
            // dateValue2
            // 
            this.dateValue2.Location = new System.Drawing.Point(78, 64);
            this.dateValue2.Name = "dateValue2";
            this.dateValue2.Size = new System.Drawing.Size(121, 21);
            this.dateValue2.TabIndex = 3;
            // 
            // comValue2
            // 
            this.comValue2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comValue2.FormattingEnabled = true;
            this.comValue2.Items.AddRange(new object[] {
            "true",
            "false"});
            this.comValue2.Location = new System.Drawing.Point(78, 64);
            this.comValue2.Name = "comValue2";
            this.comValue2.Size = new System.Drawing.Size(121, 20);
            this.comValue2.TabIndex = 4;
            // 
            // txtValue2
            // 
            this.txtValue2.Location = new System.Drawing.Point(78, 64);
            this.txtValue2.Name = "txtValue2";
            this.txtValue2.Size = new System.Drawing.Size(121, 21);
            this.txtValue2.TabIndex = 5;
            // 
            // comType2
            // 
            this.comType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comType2.FormattingEnabled = true;
            this.comType2.Items.AddRange(new object[] {
            "System.String",
            "System.Int32",
            "System.Decimal",
            "System.Boolean",
            "System.DateTime",
            "<Null>"});
            this.comType2.Location = new System.Drawing.Point(78, 29);
            this.comType2.Name = "comType2";
            this.comType2.Size = new System.Drawing.Size(121, 20);
            this.comType2.TabIndex = 1;
            this.comType2.SelectedIndexChanged += new System.EventHandler(this.comType2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "值：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "类型：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dateValue1);
            this.groupBox3.Controls.Add(this.comValue1);
            this.groupBox3.Controls.Add(this.txtValue1);
            this.groupBox3.Controls.Add(this.comType1);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 170);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "值一";
            // 
            // dateValue1
            // 
            this.dateValue1.Location = new System.Drawing.Point(79, 65);
            this.dateValue1.Name = "dateValue1";
            this.dateValue1.Size = new System.Drawing.Size(121, 21);
            this.dateValue1.TabIndex = 3;
            // 
            // comValue1
            // 
            this.comValue1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comValue1.FormattingEnabled = true;
            this.comValue1.Items.AddRange(new object[] {
            "true",
            "false"});
            this.comValue1.Location = new System.Drawing.Point(79, 65);
            this.comValue1.Name = "comValue1";
            this.comValue1.Size = new System.Drawing.Size(121, 20);
            this.comValue1.TabIndex = 4;
            // 
            // txtValue1
            // 
            this.txtValue1.Location = new System.Drawing.Point(79, 64);
            this.txtValue1.Name = "txtValue1";
            this.txtValue1.Size = new System.Drawing.Size(121, 21);
            this.txtValue1.TabIndex = 5;
            // 
            // comType1
            // 
            this.comType1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comType1.FormattingEnabled = true;
            this.comType1.Items.AddRange(new object[] {
            "System.String",
            "System.Int32",
            "System.Decimal",
            "System.Boolean",
            "System.DateTime",
            "<Null>"});
            this.comType1.Location = new System.Drawing.Point(79, 29);
            this.comType1.Name = "comType1";
            this.comType1.Size = new System.Drawing.Size(121, 20);
            this.comType1.TabIndex = 1;
            this.comType1.SelectedIndexChanged += new System.EventHandler(this.comType1_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "值：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "类型：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comCondition);
            this.groupBox2.Controls.Add(this.checkApplyToRow);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(495, 70);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息";
            // 
            // comCondition
            // 
            this.comCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comCondition.FormattingEnabled = true;
            this.comCondition.Items.AddRange(new object[] {
            "等于",
            "不等于",
            "在值一与值二之间",
            "不在值一与值二之间",
            "大于",
            "小于",
            "大于等于",
            "小于等于"});
            this.comCondition.Location = new System.Drawing.Point(195, 31);
            this.comCondition.Name = "comCondition";
            this.comCondition.Size = new System.Drawing.Size(269, 20);
            this.comCondition.TabIndex = 2;
            // 
            // checkApplyToRow
            // 
            this.checkApplyToRow.AutoSize = true;
            this.checkApplyToRow.Location = new System.Drawing.Point(33, 34);
            this.checkApplyToRow.Name = "checkApplyToRow";
            this.checkApplyToRow.Size = new System.Drawing.Size(72, 16);
            this.checkApplyToRow.TabIndex = 0;
            this.checkApplyToRow.Text = "应用于行";
            this.checkApplyToRow.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "条件：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnFore);
            this.groupBox1.Controls.Add(this.btnBack);
            this.groupBox1.Location = new System.Drawing.Point(12, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "颜色";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(263, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "前景色：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "背景色：";
            // 
            // btnFore
            // 
            this.btnFore.BackColor = System.Drawing.Color.Black;
            this.btnFore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFore.Location = new System.Drawing.Point(322, 23);
            this.btnFore.Name = "btnFore";
            this.btnFore.Size = new System.Drawing.Size(142, 23);
            this.btnFore.TabIndex = 3;
            this.btnFore.UseVisualStyleBackColor = false;
            this.btnFore.Click += new System.EventHandler(this.button_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Location = new System.Drawing.Point(90, 22);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(142, 23);
            this.btnBack.TabIndex = 1;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.button_Click);
            // 
            // FormNewStyle
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(519, 338);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNewStyle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增样式";
            this.Load += new System.EventHandler(this.FormNewStyle_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DateTimePicker dateValue2;
        private System.Windows.Forms.ComboBox comValue2;
        private System.Windows.Forms.TextBox txtValue2;
        private System.Windows.Forms.ComboBox comType2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dateValue1;
        private System.Windows.Forms.ComboBox comValue1;
        private System.Windows.Forms.TextBox txtValue1;
        private System.Windows.Forms.ComboBox comType1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comCondition;
        private System.Windows.Forms.CheckBox checkApplyToRow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFore;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       