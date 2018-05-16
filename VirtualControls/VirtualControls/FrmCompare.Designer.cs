namespace VirtualControls
{
    partial class FrmCompare
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
            this.button1 = new System.Windows.Forms.Button();
            this.vcc1 = new System.Windows.Forms.Panel();
            this.vcc2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "切换";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // vcc1
            // 
            this.vcc1.Location = new System.Drawing.Point(12, 12);
            this.vcc1.Name = "vcc1";
            this.vcc1.Size = new System.Drawing.Size(262, 299);
            this.vcc1.TabIndex = 1;
            // 
            // vcc2
            // 
            this.vcc2.Location = new System.Drawing.Point(305, 12);
            this.vcc2.Name = "vcc2";
            this.vcc2.Size = new System.Drawing.Size(262, 299);
            this.vcc2.TabIndex = 2;
            // 
            // FrmCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::VirtualControls.Properties.Resources._5;
            this.ClientSize = new System.Drawing.Size(689, 424);
            this.Controls.Add(this.vcc2);
            this.Controls.Add(this.vcc1);
            this.Controls.Add(this.button1);
            this.Name = "FrmCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmCompare";
            this.Load += new System.EventHandler(this.FrmCompare_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel vcc1;
        private System.Windows.Forms.Panel vcc2;
    }
}