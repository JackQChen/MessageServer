namespace QueryLauncher
{
    partial class FormQueryResult
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
            this.components = new System.ComponentModel.Container();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.toolQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPrint = new System.Windows.Forms.ToolStripButton();
            this.toolView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolField = new System.Windows.Forms.ToolStripButton();
            this.toolCard = new System.Windows.Forms.ToolStripButton();
            this.toolExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolFirt = new System.Windows.Forms.ToolStripButton();
            this.toolPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolNext = new System.Windows.Forms.ToolStripButton();
            this.toolLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolLayout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolExit = new System.Windows.Forms.ToolStripButton();
            this.printingSystem1 = new DevExpress.XtraPrinting.PrintingSystem(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.mainTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printingSystem1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTool
            // 
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolQuery,
            this.toolStripSeparator1,
            this.toolPrint,
            this.toolView,
            this.toolStripSeparator2,
            this.toolField,
            this.toolCard,
            this.toolExport,
            this.toolStripSeparator3,
            this.toolFirt,
            this.toolPrevious,
            this.toolNext,
            this.toolLast,
            this.toolStripSeparator4,
            this.toolLayout,
            this.toolStripSeparator5,
            this.toolExit});
            this.mainTool.Location = new System.Drawing.Point(0, 0);
            this.mainTool.Name = "mainTool";
            this.mainTool.Size = new System.Drawing.Size(792, 25);
            this.mainTool.TabIndex = 0;
            this.mainTool.Text = "toolStrip1";
            // 
            // toolQuery
            //  
            this.toolQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolQuery.Name = "toolQuery";
            this.toolQuery.Size = new System.Drawing.Size(49, 22);
            this.toolQuery.Text = "查询";
            this.toolQuery.Click += new System.EventHandler(this.toolQuery_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolPrint
            //  
            this.toolPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPrint.Name = "toolPrint";
            this.toolPrint.Size = new System.Drawing.Size(49, 22);
            this.toolPrint.Text = "打印";
            this.toolPrint.Click += new System.EventHandler(this.toolPrint_Click);
            // 
            // toolView
            //  
            this.toolView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolView.Name = "toolView";
            this.toolView.Size = new System.Drawing.Size(49, 22);
            this.toolView.Text = "预览";
            this.toolView.Click += new System.EventHandler(this.toolView_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolField
            //  
            this.toolField.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolField.Name = "toolField";
            this.toolField.Size = new System.Drawing.Size(49, 22);
            this.toolField.Text = "字段";
            this.toolField.Click += new System.EventHandler(this.toolField_Click);
            // 
            // toolCard
            //  
            this.toolCard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolCard.Name = "toolCard";
            this.toolCard.Size = new System.Drawing.Size(49, 22);
            this.toolCard.Text = "卡片";
            this.toolCard.Click += new System.EventHandler(this.toolCard_Click);
            // 
            // toolExport
            //  
            this.toolExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolExport.Name = "toolExport";
            this.toolExport.Size = new System.Drawing.Size(49, 22);
            this.toolExport.Text = "导出";
            this.toolExport.Click += new System.EventHandler(this.toolExport_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolFirt
            //  
            this.toolFirt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFirt.Name = "toolFirt";
            this.toolFirt.Size = new System.Drawing.Size(49, 22);
            this.toolFirt.Text = "首项";
            this.toolFirt.Click += new System.EventHandler(this.toolFirt_Click);
            // 
            // toolPrevious
            //  
            this.toolPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPrevious.Name = "toolPrevious";
            this.toolPrevious.Size = new System.Drawing.Size(49, 22);
            this.toolPrevious.Text = "前项";
            this.toolPrevious.Click += new System.EventHandler(this.toolPrevious_Click);
            // 
            // toolNext
            //  
            this.toolNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNext.Name = "toolNext";
            this.toolNext.Size = new System.Drawing.Size(49, 22);
            this.toolNext.Text = "后项";
            this.toolNext.Click += new System.EventHandler(this.toolNext_Click);
            // 
            // toolLast
            //  
            this.toolLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLast.Name = "toolLast";
            this.toolLast.Size = new System.Drawing.Size(49, 22);
            this.toolLast.Text = "末项";
            this.toolLast.Click += new System.EventHandler(this.toolLast_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolLayout
            //  
            this.toolLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLayout.Name = "toolLayout";
            this.toolLayout.Size = new System.Drawing.Size(73, 22);
            this.toolLayout.Text = "保存布局";
            this.toolLayout.Click += new System.EventHandler(this.toolLayout_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolExit
            //  
            this.toolExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolExit.Name = "toolExit";
            this.toolExit.Size = new System.Drawing.Size(49, 22);
            this.toolExit.Text = "退出";
            this.toolExit.Click += new System.EventHandler(this.toolExit_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 25);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(792, 541);
            this.mainPanel.TabIndex = 1;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "Excel文件|*.xls";
            // 
            // FormQueryResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.mainTool);
            this.Name = "FormQueryResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询结果";
            this.Load += new System.EventHandler(this.FormQueryResult_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormQueryResult_KeyPress_1);
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printingSystem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainTool;
        private System.Windows.Forms.ToolStripButton toolQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolPrint;
        private System.Windows.Forms.ToolStripButton toolView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolField;
        private System.Windows.Forms.ToolStripButton toolCard;
        private System.Windows.Forms.ToolStripButton toolExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolFirt;
        private System.Windows.Forms.ToolStripButton toolPrevious;
        private System.Windows.Forms.ToolStripButton toolNext;
        private System.Windows.Forms.ToolStripButton toolLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolLayout;
        private DevExpress.XtraPrinting.PrintingSystem printingSystem1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}