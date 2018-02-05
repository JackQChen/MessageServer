
using SystemFramework.BaseControl.Properties;
namespace SystemFramework.BaseControl
{
    partial class WorkPlat
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tcMain = new DevExpress.XtraTab.XtraTabControl();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭当前选项卡ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.除此之外ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.设置布局RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.全部关闭AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).BeginInit();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.tcMain.ContextMenuStrip = this.cms;
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.Size = new System.Drawing.Size(623, 317);
            this.tcMain.TabIndex = 4;
            this.tcMain.CloseButtonClick += new System.EventHandler(this.tcMain_CloseButtonClick);
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭当前选项卡ToolStripMenuItem,
            this.除此之外ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.设置布局RToolStripMenuItem,
            this.toolStripMenuItem2,
            this.全部关闭AToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(185, 126);
            this.cms.Opening += new System.ComponentModel.CancelEventHandler(this.cms_Opening);
            // 
            // 关闭当前选项卡ToolStripMenuItem
            // 
            this.关闭当前选项卡ToolStripMenuItem.Name = "关闭当前选项卡ToolStripMenuItem";
            this.关闭当前选项卡ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.关闭当前选项卡ToolStripMenuItem.Text = "关闭(&X)";
            this.关闭当前选项卡ToolStripMenuItem.Click += new System.EventHandler(this.关闭当前选项卡ToolStripMenuItem_Click);
            // 
            // 除此之外ToolStripMenuItem
            // 
            this.除此之外ToolStripMenuItem.Name = "除此之外ToolStripMenuItem";
            this.除此之外ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.除此之外ToolStripMenuItem.Text = "除此之外全部关闭(&E)";
            this.除此之外ToolStripMenuItem.Click += new System.EventHandler(this.除此之外ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // 设置布局RToolStripMenuItem
            // 
            this.设置布局RToolStripMenuItem.Image = Resources.Layout;
            this.设置布局RToolStripMenuItem.Name = "设置布局RToolStripMenuItem";
            this.设置布局RToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.设置布局RToolStripMenuItem.Text = "设置布局(&L)";
            this.设置布局RToolStripMenuItem.Click += new System.EventHandler(this.设置布局RToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // 全部关闭AToolStripMenuItem
            // 
            this.全部关闭AToolStripMenuItem.Name = "全部关闭AToolStripMenuItem";
            this.全部关闭AToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.全部关闭AToolStripMenuItem.Text = "全部关闭(&A)";
            this.全部关闭AToolStripMenuItem.Click += new System.EventHandler(this.全部关闭AToolStripMenuItem_Click);
            // 
            // WorkPlat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcMain);
            this.Name = "WorkPlat";
            this.Size = new System.Drawing.Size(623, 317);
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).EndInit();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tcMain;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem 关闭当前选项卡ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 除此之外ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 全部关闭AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置布局RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}
