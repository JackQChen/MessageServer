
using System;
using System.ComponentModel;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
namespace SnControl
{
    public class Column : GridColumn, ICloneable
    {
        private bool _isSearch = true;
        internal Search _ownSearch;
        public Column()
        {
            this.Visible = true;
            this.VisibleIndex = 0;
            MatchMode = MatchMode.Part;
        }

        [DefaultValue(true)]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        [DefaultValue(0)]
        public override int VisibleIndex
        {
            get
            {
                return base.VisibleIndex;
            }
            set
            {
                base.VisibleIndex = value;
            }
        }

        [Browsable(false), Obsolete("检索控件中请使用SortOrderMode属性进行排序", true)]
        public new ColumnSortOrder SortOrder { get; set; }

        [Category("数据"), Description("排序方式"), DefaultValue(ColumnSortOrder.None)]
        public ColumnSortOrder SortOrders { get; set; }

        [Category("检索"), Description("是否为检索项目"), DefaultValue(true)]
        public bool IsSearch
        {
            get { return _isSearch; }
            set
            {
                _isSearch = value;
                MatchMode = value ? MatchMode.Part : MatchMode.None;
            }
        }

        [Category("检索"), Description("检索匹配模式"), DefaultValue(MatchMode.Part)]
        public MatchMode MatchMode { get; set; }

        [Category("检索"), Description("列筛选字符"), DefaultValue(null)]
        public string FilterString
        { get; set; }

        #region ICloneable 成员

        public object Clone()
        {
            Column clm = new Column();
            clm.IsSearch = this.IsSearch;
            clm.MatchMode = this.MatchMode;
            clm.FilterString = this.FilterString;
            clm.Visible = this.Visible;
            clm.VisibleIndex = this.VisibleIndex;
            clm.Width = this.Width;
            clm.FieldName = this.FieldName;
            clm.Caption = this.Caption;
            clm.SortOrders = this.SortOrders;
            return clm;
        }

        #endregion
    }
}
