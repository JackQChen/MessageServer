namespace FlowViewer
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.wbPerformance = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbPerformance
            // 
            this.wbPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbPerformance.Location = new System.Drawing.Point(0, 0);
            this.wbPerformance.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbPerformance.Name = "wbPerformance";
            this.wbPerformance.ScriptErrorsSuppressed = true;
            this.wbPerformance.Size = new System.Drawing.Size(284, 262);
            this.wbPerformance.TabIndex = 0;
            this.wbPerformance.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbPerformance_DocumentCompleted);
            this.wbPerformance.SizeChanged += new System.EventHandler(this.wbPerformance_SizeChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.wbPerformance);
            this.Name = "FrmMain";
            this.Text = "流量查看器";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbPerformance;
    }
}

