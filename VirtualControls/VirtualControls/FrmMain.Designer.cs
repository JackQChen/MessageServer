namespace VirtualControls
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.vcc2 = new VirtualControls.Controls.VirtualControlContainer();
            this.vcc1 = new VirtualControls.Controls.VirtualControlContainer();
            this.vccControl = new VirtualControls.Controls.VirtualControlContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(574, 367);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(119, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // vcc2
            // 
            this.vcc2.BackColor = System.Drawing.SystemColors.Control;
            this.vcc2.Image = null;
            this.vcc2.Location = new System.Drawing.Point(157, 21);
            this.vcc2.Name = "vcc2";
            this.vcc2.Size = new System.Drawing.Size(139, 318);
            this.vcc2.TabIndex = 3;
            this.vcc2.Text = "virtualControlContainer2";
            // 
            // vcc1
            // 
            this.vcc1.BackColor = System.Drawing.SystemColors.Control;
            this.vcc1.Image = null;
            this.vcc1.Location = new System.Drawing.Point(12, 21);
            this.vcc1.Name = "vcc1";
            this.vcc1.Size = new System.Drawing.Size(139, 318);
            this.vcc1.TabIndex = 2;
            this.vcc1.Text = "virtualControlContainer2";
            // 
            // vccControl
            // 
            this.vccControl.Image = null;
            this.vccControl.Location = new System.Drawing.Point(1, 345);
            this.vccControl.Name = "vccControl";
            this.vccControl.Size = new System.Drawing.Size(567, 100);
            this.vccControl.TabIndex = 4;
            this.vccControl.Text = "virtualControlContainer1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::VirtualControls.Properties.Resources._5;
            this.ClientSize = new System.Drawing.Size(693, 445);
            this.Controls.Add(this.vccControl);
            this.Controls.Add(this.vcc2);
            this.Controls.Add(this.vcc1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private Controls.VirtualControlContainer vcc1;
        private Controls.VirtualControlContainer vcc2;
        private Controls.VirtualControlContainer vccControl;




    }
}

