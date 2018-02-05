namespace QueryDesigner
{
    partial class FormToolBox
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
            DevExpress.XtraNavBar.ViewInfo.FlatViewInfoRegistrator flatViewInfoRegistrator1 = new DevExpress.XtraNavBar.ViewInfo.FlatViewInfoRegistrator();
            this.toolboxTabList = new DevExpress.XtraNavBar.NavBarControl();
            this.toolCommon = new DevExpress.XtraNavBar.NavBarGroup();
            this.toolExtension = new DevExpress.XtraNavBar.NavBarGroup();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxTabList)).BeginInit();
            this.SuspendLayout();
            // 
            // toolboxTabList
            // 
            this.toolboxTabList.ActiveGroup = this.toolCommon;
            this.toolboxTabList.AllowSelectedLink = true;
            this.toolboxTabList.BackColor = System.Drawing.SystemColors.Control;
            this.toolboxTabList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolboxTabList.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.toolCommon,
            this.toolExtension});
            this.toolboxTabList.Location = new System.Drawing.Point(0, 0);
            this.toolboxTabList.Name = "toolboxTabList";
            this.toolboxTabList.OptionsNavPane.ExpandedWidth = 175;
            this.toolboxTabList.Size = new System.Drawing.Size(175, 476);
            this.toolboxTabList.TabIndex = 0;
            this.toolboxTabList.View = flatViewInfoRegistrator1;
            this.toolboxTabList.SelectedLinkChanged += new DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventHandler(this.toolboxTabList_SelectedLinkChanged);
            // 
            // navBarGroup1
            // 
            this.toolCommon.Caption = "常规";
            this.toolCommon.Expanded = true;
            this.toolCommon.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Small;
            this.toolCommon.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.toolCommon.Name = "navBarGroup1";
            // 
            // toolExtension
            // 
            this.toolExtension.Caption = "扩展";
            this.toolExtension.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Small;
            this.toolExtension.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.toolExtension.Name = "toolExtension";
            // 
            // FormToolBox
            // 
            this.AutoHidePortion = 0.2D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(175, 476);
            this.Controls.Add(this.toolboxTabList);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormToolBox";
            this.Text = "工具箱";
            ((System.ComponentModel.ISupportInitialize)(this.toolboxTabList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl toolboxTabList;
        private DevExpress.XtraNavBar.NavBarGroup toolCommon;
        private DevExpress.XtraNavBar.NavBarGroup toolExtension;
    }
}