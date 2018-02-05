using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using SnControl;
using System.Drawing.Design;
using SnControl.Properties;

namespace SnControl
{
    #region 使用类及枚举

    public class SearchDoneArgs : EventArgs
    {
        public string Value { get; set; }
        public DataRow DataRow { get; set; }
        public SearchDoneArgs()
        {
        }
        public SearchDoneArgs(string val, DataRow dr)
        {
            this.Value = val;
            this.DataRow = dr;
        }
    }

    public class TextArgs : EventArgs
    {
        public string CurrentText { get; set; }
        public TextArgs()
        {
        }
        public TextArgs(string val)
        {
            this.CurrentText = val;
        }
    }

    public class DataHandler
    {
        public string CurrentText { get; set; }
        public Action<object> UpdateDataSource { get; set; }
        public DataHandler()
        {
        }
        public DataHandler(string val, Action<object> update)
        {
            this.CurrentText = val;
            this.UpdateDataSource = update;
        }
    }

    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// 左匹配
        /// </summary>
        Left,
        /// <summary>
        /// 右匹配
        /// </summary>
        Right,
        /// <summary>
        /// 全字匹配
        /// </summary>
        All,
        /// <summary>
        /// 部分匹配
        /// </summary>
        Part,
        /// <summary>
        /// 不匹配
        /// </summary>
        None
    }

    #endregion

    [ToolboxBitmap(typeof(Search), "Search.png"), DefaultProperty("Columns"), DefaultEvent("SearchDone")]
    public class Search : TextEdit, ICommonAttribute
    {
        private class fixedButton : SimpleButton
        {
            public fixedButton()
            {
                this.Dock = DockStyle.Right;
                this.Width = 24;
                this.TabStop = false;
                this.AllowFocus = false;
            }
        }

        #region 属性及事件

        internal string OldText = "";

        private Columns _columns;
        private int _selectIndex = -1;
        private string _searchString = "", _valueMember = "", _value = "", _displayMember = "";
        private bool _customSearch, _autoClear, _showlist, _gridText, _delButton, _searhButton, _needChange = true, _searchDoneClosePopup, _editable = true, _autoColumnWidth = true;
        private fixedButton btnSearch = new fixedButton(), btnDelete = new fixedButton();
        private StringBuilder QueryStr = new StringBuilder();
        private DataRow _selectedRow = null;
        private DataTable _dataTable;
        private object _dataSource;
        private Action<object> updateDataSource;

        [Browsable(false)]
        public Action<DataHandler> UpdateData { get; set; }

        [Category("检索事件"), Description("检索完成事件")]
        public event EventHandler<SearchDoneArgs> SearchDone;

        [Category("检索事件"), Description("文本发生变化事件")]
        public new event EventHandler<TextArgs> TextChanged;

        [Category("检索列表"), Description("获取或设置检索列表数据源"), DefaultValue(null), AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get
            {
                return this._dataSource;
            }
            set
            {
                if (value != null)
                    if (value.GetType().Equals(typeof(DataTable)))
                        this._dataTable = value as DataTable;
                    else if ((value as IList).Count > 0)
                    {
                        DataTable dt = new DataTable();
                        IList list = value as IList;
                        PropertyInfo[] piArr = list[0].GetType().GetProperties(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        foreach (PropertyInfo pi in piArr)
                            try
                            {
                                dt.Columns.Add(pi.Name, pi.PropertyType);
                            }
                            catch (NotSupportedException)
                            {
                                dt.Columns.Add(pi.Name);
                            }
                        foreach (object obj in list)
                        {
                            DataRow dr = dt.NewRow();
                            foreach (PropertyInfo pi in piArr)
                                dr[pi.Name] = pi.GetValue(obj, null);
                            dt.Rows.Add(dr);
                        }
                        this._dataTable = dt;
                        this.RefreshIndexData();
                    }
                this._dataSource = value;
            }
        }

        [Category("检索列表"), Description("获取或设置列集合")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Columns Columns
        {
            get
            {
                if (this._columns == null)
                    this._columns = new Columns(this);
                return this._columns;
            }
        }

        [Category("检索列表"), Description("是否使用自定义检索"), DefaultValue(false)]
        public bool CustomSearch
        {
            get
            {
                return _customSearch;
            }
            set
            {
                _customSearch = value;
            }
        }

        [Category("检索列表"), Description("使用自定义检索时检索字符串\r\n使用{0}代替当前检索文本"), DefaultValue("")]
        public string CustomSearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
            }
        }

        [Category("检索列表"), Description("数据容器宽度"), DefaultValue(200)]
        public int SearchWidth
        {
            get
            {
                return this.ccSearch.Width;
            }
            set
            {
                this.ccSearch.Width = value;
            }
        }

        [Category("检索列表"), Description("数据容器高度"), DefaultValue(100)]
        public int SearchHeight
        {
            get
            {
                return this.ccSearch.Height;
            }
            set
            {
                this.ccSearch.Height = value;
            }
        }

        [Category("检索列表"), Description("显示字段"), DefaultValue("")]
        public string DisplayMember
        {
            get
            {
                return _displayMember;
            }
            set
            {
                _displayMember = value;
            }
        }

        [Category("检索列表"), Description("检索完成跳至下一控件"), DefaultValue(false)]
        public bool SearchDoneMoveNextControl { get; set; }

        private void RefreshIndexData()
        {
            if (this._dataTable != null && this._selectIndex > -1
                && !string.IsNullOrEmpty(this._displayMember)
                && !string.IsNullOrEmpty(this._valueMember))
            {
                if (this._selectIndex <= this._dataTable.Rows.Count - 1)
                {
                    this._value = this._dataTable.Rows[this._selectIndex][this._valueMember].ToString();
                    this.Text = this._dataTable.Rows[this._selectIndex][this._displayMember].ToString();
                }
            }
        }

        [Category("检索列表"), Description("控件默认索引位置"), DefaultValue(-1)]
        public int SelectedRowIndex
        {
            get
            {
                return this._selectIndex;
            }
            set
            {
                if (value < -1)
                    return;
                else
                    this._selectIndex = value;
                this.RefreshIndexData();
            }
        }

        [Category("检索列表"), Description("检索取消保留当前文本"), DefaultValue(false)]
        public bool CancelKeepText { get; set; }

        [Category("检索列表"), Description("检索按钮是否可见"), DefaultValue(null)]
        public bool SearchButtonVisible
        {
            get { return this._searhButton; }
            set
            {
                if (this._searhButton == value)
                    return;
                this._searhButton = value;
                if (value)
                {
                    btnSearch.Image = Resources.Search;
                    this.btnSearch.ToolTip = "检索全部信息";
                    this.Controls.Add(btnSearch);
                    this.btnSearch.BringToFront();
                    this.btnSearch.Click += SearchButton_Click;
                    this.Controls.Add(this.btnSearch);
                    this.btnSearch.BringToFront();
                }
                else
                {
                    this.btnSearch.Click -= this.SearchButton_Click;
                    this.Controls.Remove(this.btnSearch);
                }
            }
        }

        [Category("检索列表"), Description("删除按钮是否可见"), DefaultValue(null)]
        public bool DeleteButtonVisible
        {
            get { return this._delButton; }
            set
            {
                if (this._delButton == value)
                    return;
                this._delButton = value;
                if (value)
                {
                    btnDelete.Image = Resources.Delete;
                    btnDelete.ToolTip = "删除检索信息\r\n快捷键Delete";
                    btnDelete.Click += DeleteButton_Click;
                    this.Controls.Add(this.btnDelete);
                    this.btnDelete.BringToFront();
                }
                else
                {
                    this.btnDelete.Click -= this.DeleteButton_Click;
                    this.Controls.Remove(this.btnDelete);
                }
            }
        }

        [Category("检索列表"), Description("返回值字段"), DefaultValue("")]
        public string ValueMember
        {
            get
            {
                return _valueMember;
            }
            set
            {
                _valueMember = value;
            }
        }

        [Category("检索列表"), Description("使用实时数据"), DefaultValue(false)]
        public bool UseRealData { get; set; }

        [Category("检索列表"), Description("返回值"), DefaultValue("")]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        [Category("检索列表"), Description("条件样式设置")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StyleFormatConditionCollection StyleConditionCollection
        {
            get { return this.gvSearch.FormatConditions; }
        }

        [Category("检索列表"), Description("选择的列"), DefaultValue(null), Browsable(false)]
        public DataRow SelectedRow
        {
            get
            {
                return _selectedRow;
            }
            set
            {
                _selectedRow = value;
            }
        }

        [Category("检索列表"), Description("是否显示统计栏"), DefaultValue(false)]
        public bool ShowFooter
        {
            get
            {
                return this.gvSearch.OptionsView.ShowFooter;
            }
            set
            {
                this.gvSearch.OptionsView.ShowFooter = value;
            }
        }

        [Category("检索列表"), Description("自动清空内容"), DefaultValue(false)]
        public bool AutoClear
        {
            get
            {
                return _autoClear;
            }
            set
            {
                _autoClear = value;
            }
        }


        [Category("检索列表"), Description("自动检索列宽"), DefaultValue(true)]
        public bool AutoColumnWidth
        {
            get
            {
                return _autoColumnWidth;
            }
            set
            {
                _autoColumnWidth = value;
            }
        }

        [Category("检索列表"), Description("是否可以编辑"), DefaultValue(true)]
        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (_editable == value)
                    return;
                if (_editable)
                {
                    this.Properties.ReadOnly = true;
                    this.btnSearch.Enabled = false;
                }
                else
                {
                    this.Properties.ReadOnly = false;
                    this.btnSearch.Enabled = true;
                }
                _editable = value;
            }
        }

        [Category("检索列表"), Description("是否使用第一行数据"), DefaultValue(false)]
        public bool UseFirstData { get; set; }

        #endregion

        #region 控件设计代码

        private PopupContainerEdit ceSearch;
        private PopupContainerControl ccSearch;
        private GridControl gcSearch;
        private GridView gvSearch;

        public Search()
        {

            this.ceSearch = new PopupContainerEdit();
            this.ccSearch = new PopupContainerControl();
            this.gcSearch = new GridControl();
            this.gvSearch = new GridView();

            ((System.ComponentModel.ISupportInitialize)(this.ccSearch)).BeginInit();
            this.ccSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // ccSearch
            // 
            this.ccSearch.Controls.Add(this.gcSearch);
            // 
            // gcSearch
            // 
            this.gcSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSearch.MainView = this.gvSearch;
            this.gcSearch.ViewCollection.Add(this.gvSearch);
            // 
            // gvSearch
            // 
            this.gvSearch.GridControl = this.gcSearch;
            this.gvSearch.OptionsBehavior.Editable = false;
            this.gvSearch.OptionsCustomization.AllowFilter = false;
            this.gvSearch.OptionsMenu.EnableColumnMenu = false;
            this.gvSearch.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvSearch.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            this.gvSearch.Appearance.HeaderPanel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.gvSearch.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvSearch.Appearance.Row.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.gvSearch.Appearance.Row.Options.UseFont = true;
            this.gvSearch.OptionsView.ShowGroupPanel = false;
            this.gvSearch.OptionsView.ShowIndicator = false;
            this.gvSearch.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvSearch.MouseDown += new MouseEventHandler(gvSearch_MouseDown);
            this.gvSearch.KeyDown += new KeyEventHandler(gvSearch_KeyDown);
            this.gvSearch.KeyPress += new KeyPressEventHandler(gvSearch_KeyPress);

            //
            //ceSearch
            //    
            this.ceSearch.Height = 0;
            this.ceSearch.Width = 0;
            this.ceSearch.TabStop = false;
            this.ceSearch.Properties.PopupControl = ccSearch;
            this.ceSearch.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ceSearch.Properties.ShowPopupCloseButton = false;
            this.ceSearch.QueryResultValue += new QueryResultValueEventHandler(ceSearch_QueryResultValue);

            //
            //Search
            //   
            this.EditValue = "";
            this.Columns.AddColumn = clm =>
            {
                if (base.Container != null && clm.Container == null)
                    base.Container.Add(clm);
            };
            this.Controls.Add(ceSearch);

            ((System.ComponentModel.ISupportInitialize)(this.ccSearch)).EndInit();
            this.ccSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region 事件及重写

        private void ceSearch_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            _needChange = false;
            if (CancelKeepText && !_searchDoneClosePopup)
                OldText = this.Text;
            this.Text = OldText;
            _needChange = true;
            this.Refresh();
            this.Select(this.Text.Length, 0);
        }

        private void gvSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OnSearchDone();
            else if (e.KeyCode == Keys.Delete)
                DeleteText();
            else if (e.KeyCode == Keys.Tab)
                e.Handled = true;
        }

        private void gvSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            _gridText = true;
            if (e.KeyChar == '\b' && this.Text.Length > 0)
                this.Text = this.Text.Remove(this.Text.Length - 1);
            else if ((int)e.KeyChar > 31 && (int)e.KeyChar < 128)
                this.Text = this.Text + e.KeyChar;
            else
                e.Handled = true;
            _gridText = false;
            this.Focus();
            this.Select(this.Text.Length, 0);
        }

        private void gvSearch_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
                if (this.gvSearch.FocusedRowHandle >= 0)
                {
                    OnSearchDone();
                    this.Focus();
                    this.Select(this.Text.Length, 0);
                }
        }

        internal void SearchButton_Click(object sender, EventArgs e)
        {
            if (UseRealData)
                if (UpdateData == null)
                    return;
                else
                    UpdateData(new DataHandler(this.Text, updateDataSource));
            SearchInfo("%");
            this.ceSearch.ShowPopup();
            this.Focus();
            this.SelectAll();
        }

        internal void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteText();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !base.IsDisposed)
            {
                foreach (Column clm in this.Columns)
                    clm.Dispose();
            }
            base.Dispose(disposing);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                _showlist = false;
                if (!_gridText)
                    OldText = value;
                base.Text = value;
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            if (_autoClear)
            {
                this.Text = "";
                this.ForeColor = Color.Black;
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (_autoClear)
            {
                this.Text = "请输入检索信息...";
                this.ForeColor = Color.Gray;
            }
            if (this.ceSearch.IsPopupOpen)
            {
                this.ceSearch.ClosePopup();
            }
            if (this.UseRealData)
                this.Text = this.OldText;
            base.OnLeave(e);
        }

        protected override void OnLoaded()
        {
            if (_autoClear)
            {
                this.Text = "请输入检索信息...";
                this.ForeColor = Color.Gray;
            }
            InitGrid();
            base.OnLoaded();
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 522 && this.ceSearch.IsPopupOpen)
                return;
            base.WndProc(ref msg);
        }

        public void InitGrid()
        {
            List<GridColumnSortInfo> sortList = new List<GridColumnSortInfo>();
            foreach (Column clm in Columns)
            {
                gvSearch.Columns.Add(clm);
                clm.FilterInfo = new ColumnFilterInfo(clm.FilterString);
                if (clm.SortOrders != ColumnSortOrder.None)
                    sortList.Add(new GridColumnSortInfo(clm, clm.SortOrders));
            }
            this.gvSearch.OptionsView.ColumnAutoWidth = AutoColumnWidth;
            this.gvSearch.SortInfo.AddRange(sortList.ToArray());
            updateDataSource = data =>
            {
                this.DataSource = data;
            };
        }

        public void ShowPopup()
        {
            if (!this.ceSearch.IsPopupOpen && CanFocus)
            {
                this.ceSearch.ShowPopup();
                this.Focus();
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            this.SelectAll();
            base.OnDoubleClick(e);
        }

        private void DeleteText()
        {
            this.ceSearch.ClosePopup();
            this.SelectedRow = null;
            this._value = "";
            this.Text = "";
            this.OldText = "";
            if (SearchDone != null)
                SearchDone(this, new SearchDoneArgs(this._value, this._selectedRow));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteText();
                return;
            }
            if (!_showlist)
                _showlist = true;
            if (this.ceSearch.IsPopupOpen)
            {
                if (e.KeyCode == Keys.Enter && this.gvSearch.FocusedRowHandle >= 0)
                {
                    OnSearchDone();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    gvSearch.FocusedRowHandle--;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    gvSearch.FocusedRowHandle++;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.ceSearch.ClosePopup();
                }
            }
            else if (UseRealData && e.KeyCode == Keys.Enter)
            {
                if (UpdateData != null)
                {
                    UpdateData(new DataHandler(this.Text, updateDataSource));
                    if (_needChange)
                    {
                        if (IsHandleCreated)
                            SearchInfo(this.Text.Replace("'", "").Replace("[", "").Replace("]", ""));
                        if ((this.gcSearch.DataSource as DataTable) != null)
                            if ((this.gcSearch.DataSource as DataTable).Rows.Count > 0)
                                if (!this.ceSearch.IsPopupOpen && CanFocus && _showlist)
                                {
                                    this.ceSearch.ShowPopup();
                                    this.Focus();
                                }
                    }
                }
            }
            else
                base.OnKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (_needChange)
            {
                if (!UseRealData)
                    if (!this.ceSearch.IsPopupOpen && CanFocus && _showlist)
                    {
                        this.ceSearch.ShowPopup();
                        this.Focus();
                    }
                if (IsHandleCreated && this.ceSearch.IsPopupOpen)
                    SearchInfo(this.Text.Replace("'", "").Replace("[", "").Replace("]", ""));
            }
            if (this.TextChanged != null)
                this.TextChanged(this, new TextArgs(this.Text));
        }

        #endregion

        #region 检索数据

        public bool SetDisplayValue(string keyValue)
        {
            DataRow[] drRst = this._dataTable.Select(this._valueMember + "='" + keyValue + "'");
            if (drRst.Length > 0)
            {
                this.Value = drRst[0][this._valueMember].ToString();
                this.Text = drRst[0][this._displayMember].ToString();
                this._selectedRow = drRst[0];
                return drRst.Length == 1 ? true : false;
            }
            else
                return false;
        }

        public bool SetKeyValue(string displayValue)
        {
            DataRow[] drRst = this._dataTable.Select(this._displayMember + "='" + displayValue + "'");
            if (drRst.Length > 0)
            {
                this.Value = drRst[0][this._valueMember].ToString();
                this.Text = drRst[0][this._displayMember].ToString();
                this._selectedRow = drRst[0];
                return drRst.Length == 1 ? true : false;
            }
            else
                return false;
        }

        private void OnSearchDone()
        {
            DataRow dr = this.gvSearch.GetDataRow(this.gvSearch.FocusedRowHandle);
            try
            {
                if (dr != null)
                {
                    this._value = dr[_valueMember].ToString();
                    this._selectedRow = dr;
                    OldText = dr[_displayMember].ToString();
                    _searchDoneClosePopup = true;
                    this.ceSearch.ClosePopup();
                    _searchDoneClosePopup = false;
                    if (SearchDone != null)
                        SearchDone(this, new SearchDoneArgs(this._value, this._selectedRow));
                    if (this.SearchDoneMoveNextControl)
                        this.ProcessDialogKey(Keys.Tab);
                }
            }
            catch (ArgumentException ex)
            {
                //MessageBoxEx.Show(ex.Message, "提示", BoxIcon.Error);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void SearchInfo(string searchText)
        {
            if (this._dataTable != null)
            {
                QueryStr.Remove(0, QueryStr.Length);
                if (_customSearch)
                    QueryStr.Append(string.Format(_searchString, searchText));
                else
                {
                    string tmp = "";
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        if (Columns[i].IsSearch)
                        {
                            switch (Columns[i].MatchMode)
                            {
                                case MatchMode.All: tmp = Columns[i].FieldName + " like '" + searchText + "'"; break;
                                case MatchMode.Left: tmp = Columns[i].FieldName + " like '" + searchText + "%'"; break;
                                case MatchMode.Right: tmp = Columns[i].FieldName + " like '%" + searchText + "'"; break;
                                case MatchMode.Part: tmp = Columns[i].FieldName + " like '%" + searchText + "%'"; break;
                            }
                            QueryStr.Append(QueryStr.Length == 0 ? tmp : " or " + tmp);
                        }
                    }
                }
                DataView dv = null;
                try
                {
                    dv = this._dataTable.DefaultView;
                    dv.RowFilter = QueryStr.ToString();
                    this.gcSearch.DataSource = dv.ToTable();
                }
                catch (Exception ex)
                {
                    //MessageBoxEx.Show(ex.Message, "提示", BoxIcon.Error);
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        #endregion

        //#region ICommonAttribute 成员
        //private string FDataSetName;
        //private string FFieldChineseName;
        //private string FFieldName;
        //private string FFieldType;
        //private string FFunction;
        //private string FParamName;

        //[Description("数据集合"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        //public string DataSetName
        //{
        //    get
        //    {
        //        return this.FDataSetName;
        //    }
        //    set
        //    {
        //        this.FDataSetName = value;
        //    }
        //}

        //[Description("参数名称中文名称"), Category("数据"), Editor(typeof(ParamEditor), typeof(UITypeEditor))]
        //public string FieldChineseName
        //{
        //    get
        //    {
        //        return this.FFieldChineseName;
        //    }
        //    set
        //    {
        //        this.FFieldChineseName = value;
        //    }
        //}

        //[Description("参数名称"), Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据")]
        //public string FieldName
        //{
        //    get
        //    {
        //        return this.FFieldName;
        //    }
        //    set
        //    {
        //        this.FFieldName = value;
        //    }
        //}

        //[Category("数据"), Description("参数名称类型\r\n{String,Int,Decimal,DateTime}"), Editor(typeof(ParamEditor), typeof(UITypeEditor))]
        //public string FieldType
        //{
        //    get
        //    {
        //        return this.FFieldType;
        //    }
        //    set
        //    {
        //        this.FFieldType = value;
        //    }
        //}

        //[Editor(typeof(ParamEditor), typeof(UITypeEditor)), Category("数据"), Description("执行函数")]
        //public string Function
        //{
        //    get
        //    {
        //        return this.FFunction;
        //    }
        //    set
        //    {
        //        this.FFunction = value;
        //    }
        //}

        //[Category("数据"), Description("变参"), Editor(typeof(ParamEditor), typeof(UITypeEditor))]
        //public string ParamName
        //{
        //    get
        //    {
        //        return this.FParamName;
        //    }
        //    set
        //    {
        //        this.FParamName = value;
        //    }
        //}
        //public string ProcParamName
        //{
        //    get;
        //    set;
        //}

        //public CommandProcType ProcParamType
        //{
        //    get;
        //    set;
        //}

        //#endregion 

        #region ICommonAttribute 成员

        public string DataSetName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Function
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ParamName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ParamType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

}
