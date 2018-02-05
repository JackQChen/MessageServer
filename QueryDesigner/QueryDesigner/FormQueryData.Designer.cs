namespace QueryDesigner
{
    partial class FormQueryData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryData));
            this.panel1 = new System.Windows.Forms.Panel();
            this.group = new System.Windows.Forms.GroupBox();
            this.dataList = new System.Windows.Forms.ListView();
            this.columnId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUse = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tab = new System.Windows.Forms.TabControl();
            this.pageSQL = new System.Windows.Forms.TabPage();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnGetParam = new System.Windows.Forms.ToolStripButton();
            this.btnGetColumns = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.pageParameter = new System.Windows.Forms.TabPage();
            this.dgvParam = new System.Windows.Forms.DataGridView();
            this.clmParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmParamType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmParamTest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageField = new System.Windows.Forms.TabPage();
            this.btnClearColumns = new System.Windows.Forms.Button();
            this.btnRemoveSection = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.checkRepeat = new System.Windows.Forms.CheckBox();
            this.fieldList = new System.Windows.Forms.ListView();
            this.columnFieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFieldType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStatType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnConverge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDisplayWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDigits = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnVisible = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pageResult = new System.Windows.Forms.TabPage();
            this.grdResult = new System.Windows.Forms.DataGridView();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChangeParameter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.toolNew = new System.Windows.Forms.ToolStripButton();
            this.toolDelete = new System.Windows.Forms.ToolStripButton();
            this.toolClear = new System.Windows.Forms.ToolStripButton();
            this.toolTable = new System.Windows.Forms.ToolStripButton();
            this.toolChangeParameter = new System.Windows.Forms.ToolStripButton();
            this.toolSave = new System.Windows.Forms.ToolStripButton();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            this.group.SuspendLayout();
            this.tab.SuspendLayout();
            this.pageSQL.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pageParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParam)).BeginInit();
            this.pageField.SuspendLayout();
            this.pageResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).BeginInit();
            this.mainMenu.SuspendLayout();
            this.mainTool.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.group);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 565);
            this.panel1.TabIndex = 2;
            // 
            // group
            // 
            this.group.Controls.Add(this.dataList);
            this.group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group.Location = new System.Drawing.Point(0, 0);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(262, 565);
            this.group.TabIndex = 0;
            this.group.TabStop = false;
            this.group.Text = "数据集合列表";
            // 
            // dataList
            // 
            this.dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnId,
            this.columnName,
            this.columnUse});
            this.dataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataList.FullRowSelect = true;
            this.dataList.HideSelection = false;
            this.dataList.Location = new System.Drawing.Point(3, 17);
            this.dataList.MultiSelect = false;
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(256, 545);
            this.dataList.TabIndex = 0;
            this.dataList.UseCompatibleStateImageBehavior = false;
            this.dataList.View = System.Windows.Forms.View.Details;
            this.dataList.SelectedIndexChanged += new System.EventHandler(this.dataList_SelectedIndexChanged);
            this.dataList.DoubleClick += new System.EventHandler(this.dataList_DoubleClick);
            // 
            // columnId
            // 
            this.columnId.Text = "数据集ID";
            this.columnId.Width = 100;
            // 
            // columnName
            // 
            this.columnName.Text = "中文名称";
            this.columnName.Width = 140;
            // 
            // columnUse
            // 
            this.columnUse.Text = "用途";
            this.columnUse.Width = 100;
            // 
            // tab
            // 
            this.tab.Controls.Add(this.pageSQL);
            this.tab.Controls.Add(this.pageParameter);
            this.tab.Controls.Add(this.pageField);
            this.tab.Controls.Add(this.pageResult);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(727, 565);
            this.tab.TabIndex = 3;
            // 
            // pageSQL
            // 
            this.pageSQL.Controls.Add(this.txtSQL);
            this.pageSQL.Controls.Add(this.toolStrip1);
            this.pageSQL.Location = new System.Drawing.Point(4, 22);
            this.pageSQL.Name = "pageSQL";
            this.pageSQL.Size = new System.Drawing.Size(719, 539);
            this.pageSQL.TabIndex = 4;
            this.pageSQL.Text = "SQL脚本";
            this.pageSQL.UseVisualStyleBackColor = true;
            // 
            // txtSQL
            // 
            this.txtSQL.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSQL.Location = new System.Drawing.Point(0, 25);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQL.Size = new System.Drawing.Size(719, 514);
            this.txtSQL.TabIndex = 0;
            this.txtSQL.Leave += new System.EventHandler(this.txtSQL_Leave);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGetParam,
            this.btnGetColumns,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(719, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnGetParam
            // 
            this.btnGetParam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetParam.Image = ((System.Drawing.Image)(resources.GetObject("btnGetParam.Image")));
            this.btnGetParam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetParam.Name = "btnGetParam";
            this.btnGetParam.Size = new System.Drawing.Size(60, 22);
            this.btnGetParam.Text = "获取参数";
            this.btnGetParam.Click += new System.EventHandler(this.btnGetParam_Click);
            // 
            // btnGetColumns
            // 
            this.btnGetColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGetColumns.Image = ((System.Drawing.Image)(resources.GetObject("btnGetColumns.Image")));
            this.btnGetColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetColumns.Name = "btnGetColumns";
            this.btnGetColumns.Size = new System.Drawing.Size(84, 22);
            this.btnGetColumns.Text = "获取字段列表";
            this.btnGetColumns.Click += new System.EventHandler(this.btnGetColumns_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(54, 22);
            this.toolStripButton3.Text = "查询(&Q)";
            this.toolStripButton3.Click += new System.EventHandler(this.menuQueryData_Click);
            // 
            // pageParameter
            // 
            this.pageParameter.Controls.Add(this.dgvParam);
            this.pageParameter.Location = new System.Drawing.Point(4, 22);
            this.pageParameter.Name = "pageParameter";
            this.pageParameter.Size = new System.Drawing.Size(719, 539);
            this.pageParameter.TabIndex = 3;
            this.pageParameter.Text = "参数列表";
            this.pageParameter.UseVisualStyleBackColor = true;
            // 
            // dgvParam
            // 
            this.dgvParam.AllowUserToAddRows = false;
            this.dgvParam.AllowUserToDeleteRows = false;
            this.dgvParam.AllowUserToResizeColumns = false;
            this.dgvParam.AllowUserToResizeRows = false;
            this.dgvParam.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParam.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmParamName,
            this.clmParamType,
            this.clmParamTest});
            this.dgvParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParam.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvParam.Location = new System.Drawing.Point(0, 0);
            this.dgvParam.Name = "dgvParam";
            this.dgvParam.RowTemplate.Height = 23;
            this.dgvParam.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParam.Size = new System.Drawing.Size(719, 539);
            this.dgvParam.TabIndex = 0;
            // 
            // clmParamName
            // 
            this.clmParamName.DataPropertyName = "ParamName";
            this.clmParamName.HeaderText = "参数名称";
            this.clmParamName.Name = "clmParamName";
            this.clmParamName.ReadOnly = true;
            // 
            // clmParamType
            // 
            this.clmParamType.DataPropertyName = "ParamType";
            this.clmParamType.HeaderText = "参数类型";
            this.clmParamType.Items.AddRange(new object[] {
            "String",
            "Int",
            "Decimal",
            "DateTime"});
            this.clmParamType.Name = "clmParamType";
            // 
            // clmParamTest
            // 
            this.clmParamTest.DataPropertyName = "ParamTest";
            this.clmParamTest.HeaderText = "测试内容";
            this.clmParamTest.Name = "clmParamTest";
            // 
            // pageField
            // 
            this.pageField.Controls.Add(this.btnClearColumns);
            this.pageField.Controls.Add(this.btnRemoveSection);
            this.pageField.Controls.Add(this.btnDown);
            this.pageField.Controls.Add(this.btnUp);
            this.pageField.Controls.Add(this.checkRepeat);
            this.pageField.Controls.Add(this.fieldList);
            this.pageField.Location = new System.Drawing.Point(4, 22);
            this.pageField.Name = "pageField";
            this.pageField.Padding = new System.Windows.Forms.Padding(3);
            this.pageField.Size = new System.Drawing.Size(719, 539);
            this.pageField.TabIndex = 0;
            this.pageField.Text = "字段列表";
            this.pageField.UseVisualStyleBackColor = true;
            // 
            // btnClearColumns
            // 
            this.btnClearColumns.Location = new System.Drawing.Point(200, 6);
            this.btnClearColumns.Name = "btnClearColumns";
            this.btnClearColumns.Size = new System.Drawing.Size(97, 23);
            this.btnClearColumns.TabIndex = 5;
            this.btnClearColumns.Text = "清空字段列表";
            this.btnClearColumns.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSection
            // 
            this.btnRemoveSection.Location = new System.Drawing.Point(96, 6);
            this.btnRemoveSection.Name = "btnRemoveSection";
            this.btnRemoveSection.Size = new System.Drawing.Size(98, 23);
            this.btnRemoveSection.TabIndex = 4;
            this.btnRemoveSection.Text = "移除选中信息";
            this.btnRemoveSection.UseVisualStyleBackColor = true;
            this.btnRemoveSection.Click += new System.EventHandler(this.btnRemoveSection_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(727, 6);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(27, 23);
            this.btnDown.TabIndex = 3;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(694, 6);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(27, 23);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // checkRepeat
            // 
            this.checkRepeat.AutoSize = true;
            this.checkRepeat.Location = new System.Drawing.Point(6, 10);
            this.checkRepeat.Name = "checkRepeat";
            this.checkRepeat.Size = new System.Drawing.Size(84, 16);
            this.checkRepeat.TabIndex = 1;
            this.checkRepeat.Text = "移除重复列";
            this.checkRepeat.UseVisualStyleBackColor = true;
            // 
            // fieldList
            // 
            this.fieldList.AllowColumnReorder = true;
            this.fieldList.AllowDrop = true;
            this.fieldList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFieldName,
            this.columnDisplayName,
            this.columnFieldType,
            this.columnStatType,
            this.columnConverge,
            this.columnDisplayWidth,
            this.columnDigits,
            this.columnVisible});
            this.fieldList.FullRowSelect = true;
            this.fieldList.GridLines = true;
            this.fieldList.HideSelection = false;
            this.fieldList.Location = new System.Drawing.Point(3, 32);
            this.fieldList.Name = "fieldList";
            this.fieldList.Size = new System.Drawing.Size(754, 505);
            this.fieldList.TabIndex = 0;
            this.fieldList.UseCompatibleStateImageBehavior = false;
            this.fieldList.View = System.Windows.Forms.View.Details;
            this.fieldList.DoubleClick += new System.EventHandler(this.fieldList_DoubleClick);
            // 
            // columnFieldName
            // 
            this.columnFieldName.Text = "字段名称";
            this.columnFieldName.Width = 180;
            // 
            // columnDisplayName
            // 
            this.columnDisplayName.Text = "显示名称";
            this.columnDisplayName.Width = 180;
            // 
            // columnFieldType
            // 
            this.columnFieldType.Text = "字段类型";
            this.columnFieldType.Width = 180;
            // 
            // columnStatType
            // 
            this.columnStatType.Text = "统计类型";
            this.columnStatType.Width = 160;
            // 
            // columnConverge
            // 
            this.columnConverge.Text = "聚合";
            // 
            // columnDisplayWidth
            // 
            this.columnDisplayWidth.Text = "显示宽度";
            this.columnDisplayWidth.Width = 74;
            // 
            // columnDigits
            // 
            this.columnDigits.Text = "小数位数";
            this.columnDigits.Width = 71;
            // 
            // columnVisible
            // 
            this.columnVisible.Text = "是否可见";
            // 
            // pageResult
            // 
            this.pageResult.Controls.Add(this.grdResult);
            this.pageResult.Location = new System.Drawing.Point(4, 22);
            this.pageResult.Name = "pageResult";
            this.pageResult.Size = new System.Drawing.Size(719, 539);
            this.pageResult.TabIndex = 5;
            this.pageResult.Text = "查询结果";
            this.pageResult.UseVisualStyleBackColor = true;
            // 
            // grdResult
            // 
            this.grdResult.AllowUserToAddRows = false;
            this.grdResult.AllowUserToDeleteRows = false;
            this.grdResult.AllowUserToResizeColumns = false;
            this.grdResult.AllowUserToResizeRows = false;
            this.grdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResult.Location = new System.Drawing.Point(0, 0);
            this.grdResult.MultiSelect = false;
            this.grdResult.Name = "grdResult";
            this.grdResult.ReadOnly = true;
            this.grdResult.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdResult.RowTemplate.Height = 23;
            this.grdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResult.Size = new System.Drawing.Size(719, 539);
            this.grdResult.TabIndex = 1;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuQuery,
            this.menuHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(993, 25);
            this.mainMenu.TabIndex = 4;
            this.mainMenu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuDelete,
            this.menuClear,
            this.menuTable,
            this.menuChangeParameter,
            this.menuSave,
            this.toolStripSeparator1,
            this.menuClose});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(58, 21);
            this.menuFile.Text = "文件(&F)";
            // 
            // menuNew
            // 
            this.menuNew.Image = global::QueryDesigner.Properties.Resources._66;
            this.menuNew.Name = "menuNew";
            this.menuNew.Size = new System.Drawing.Size(181, 22);
            this.menuNew.Text = "新增(&N)      Ctrl+N";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Image = global::QueryDesigner.Properties.Resources._57;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(181, 22);
            this.menuDelete.Text = "删除(&D)";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // menuClear
            // 
            this.menuClear.Image = global::QueryDesigner.Properties.Resources._69;
            this.menuClear.Name = "menuClear";
            this.menuClear.Size = new System.Drawing.Size(181, 22);
            this.menuClear.Text = "清除(&C)";
            this.menuClear.Click += new System.EventHandler(this.menuClear_Click);
            // 
            // menuTable
            // 
            this.menuTable.Image = global::QueryDesigner.Properties.Resources._77;
            this.menuTable.Name = "menuTable";
            this.menuTable.Size = new System.Drawing.Size(181, 22);
            this.menuTable.Text = "表              Ctrl+T";
            this.menuTable.Click += new System.EventHandler(this.menuTable_Click);
            // 
            // menuChangeParameter
            // 
            this.menuChangeParameter.Image = global::QueryDesigner.Properties.Resources._71;
            this.menuChangeParameter.Name = "menuChangeParameter";
            this.menuChangeParameter.Size = new System.Drawing.Size(181, 22);
            this.menuChangeParameter.Text = "变参           Ctrl+R";
            this.menuChangeParameter.Click += new System.EventHandler(this.menuQueryData_Click);
            // 
            // menuSave
            // 
            this.menuSave.Image = global::QueryDesigner.Properties.Resources._58;
            this.menuSave.Name = "menuSave";
            this.menuSave.Size = new System.Drawing.Size(181, 22);
            this.menuSave.Text = "保存(&S)       Ctrl+S";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuClose
            // 
            this.menuClose.Image = global::QueryDesigner.Properties.Resources._60;
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(181, 22);
            this.menuClose.Text = "退出(&X)       Esc";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuQuery
            // 
            this.menuQuery.Name = "menuQuery";
            this.menuQuery.Size = new System.Drawing.Size(62, 21);
            this.menuQuery.Text = "查询(&Q)";
            this.menuQuery.Click += new System.EventHandler(this.menuQueryData_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(61, 21);
            this.menuHelp.Text = "帮助(&H)";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(137, 22);
            this.menuAbout.Text = "关于      F1";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // mainTool
            // 
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolDelete,
            this.toolClear,
            this.toolStripSeparator2,
            this.toolTable,
            this.toolStripSeparator3,
            this.toolChangeParameter,
            this.toolSave,
            this.toolStripSeparator4,
            this.toolClose});
            this.mainTool.Location = new System.Drawing.Point(0, 25);
            this.mainTool.Name = "mainTool";
            this.mainTool.Size = new System.Drawing.Size(993, 25);
            this.mainTool.TabIndex = 5;
            this.mainTool.Text = "toolStrip1";
            // 
            // toolNew
            // 
            this.toolNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNew.Image = global::QueryDesigner.Properties.Resources._66;
            this.toolNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNew.Name = "toolNew";
            this.toolNew.Size = new System.Drawing.Size(23, 22);
            this.toolNew.Text = "新增";
            this.toolNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // toolDelete
            // 
            this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolDelete.Image = global::QueryDesigner.Properties.Resources._72;
            this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(23, 22);
            this.toolDelete.Text = "删除";
            this.toolDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // toolClear
            // 
            this.toolClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolClear.Image = global::QueryDesigner.Properties.Resources._69;
            this.toolClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClear.Name = "toolClear";
            this.toolClear.Size = new System.Drawing.Size(23, 22);
            this.toolClear.Text = "清除";
            this.toolClear.Click += new System.EventHandler(this.menuClear_Click);
            // 
            // toolTable
            // 
            this.toolTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolTable.Image = global::QueryDesigner.Properties.Resources._77;
            this.toolTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolTable.Name = "toolTable";
            this.toolTable.Size = new System.Drawing.Size(23, 22);
            this.toolTable.Text = "表";
            this.toolTable.Click += new System.EventHandler(this.menuTable_Click);
            // 
            // toolChangeParameter
            // 
            this.toolChangeParameter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolChangeParameter.Image = global::QueryDesigner.Properties.Resources._71;
            this.toolChangeParameter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolChangeParameter.Name = "toolChangeParameter";
            this.toolChangeParameter.Size = new System.Drawing.Size(23, 22);
            this.toolChangeParameter.Text = "测试";
            this.toolChangeParameter.Click += new System.EventHandler(this.menuQueryData_Click);
            // 
            // toolSave
            // 
            this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSave.Image = global::QueryDesigner.Properties.Resources._58;
            this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(23, 22);
            this.toolSave.Text = "保存";
            this.toolSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // toolClose
            // 
            this.toolClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolClose.Image = global::QueryDesigner.Properties.Resources._60;
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(23, 22);
            this.toolClose.Text = "退出";
            this.toolClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tab);
            this.splitContainer1.Size = new System.Drawing.Size(993, 565);
            this.splitContainer1.SplitterDistance = 262;
            this.splitContainer1.TabIndex = 0;
            // 
            // FormQueryData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 615);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainTool);
            this.Controls.Add(this.mainMenu);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(819, 534);
            this.Name = "FormQueryData";
            this.Text = "查询方案数据集合";
            this.Activated += new System.EventHandler(this.FormQueryData_Activated);
            this.Deactivate += new System.EventHandler(this.FormQueryData_Deactivate);
            this.Load += new System.EventHandler(this.FormQueryData_Load);
            this.panel1.ResumeLayout(false);
            this.group.ResumeLayout(false);
            this.tab.ResumeLayout(false);
            this.pageSQL.ResumeLayout(false);
            this.pageSQL.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pageParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParam)).EndInit();
            this.pageField.ResumeLayout(false);
            this.pageField.PerformLayout();
            this.pageResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResult)).EndInit();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage pageField;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem menuClear;
        private System.Windows.Forms.ToolStripMenuItem menuTable;
        private System.Windows.Forms.ToolStripMenuItem menuChangeParameter;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.ToolStripButton toolNew;
        private System.Windows.Forms.ToolStripButton toolDelete;
        private System.Windows.Forms.ToolStripButton toolClear;
        private System.Windows.Forms.ToolStripButton toolTable;
        private System.Windows.Forms.ToolStripButton toolChangeParameter;
        private System.Windows.Forms.ToolStripButton toolClose;
        private System.Windows.Forms.TabPage pageParameter;
        private System.Windows.Forms.TabPage pageSQL;
        private System.Windows.Forms.TabPage pageResult;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnUse;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStrip mainTool;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuQuery;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ListView fieldList;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.CheckBox checkRepeat;
        private System.Windows.Forms.ColumnHeader columnFieldName;
        private System.Windows.Forms.ColumnHeader columnDisplayName;
        private System.Windows.Forms.ColumnHeader columnFieldType;
        private System.Windows.Forms.ColumnHeader columnStatType;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.DataGridView grdResult;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ColumnHeader columnConverge;
        private System.Windows.Forms.ColumnHeader columnDisplayWidth;
        private System.Windows.Forms.ColumnHeader columnDigits;
        private System.Windows.Forms.Button btnRemoveSection;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader columnVisible;
        private System.Windows.Forms.DataGridView dgvParam;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmParamName;
        private System.Windows.Forms.DataGridViewComboBoxColumn clmParamType;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmParamTest;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnGetParam;
        private System.Windows.Forms.ToolStripButton btnGetColumns;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Button btnClearColumns;
        private System.Windows.Forms.ToolStripButton toolSave;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 