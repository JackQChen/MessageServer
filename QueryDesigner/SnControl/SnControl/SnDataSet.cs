namespace SnControl
{
    using System;

    [Serializable]
    public class SnDataSet : ICloneable
    {
        private string dataSetID = string.Empty;
        private string dataSetName = string.Empty;
        private SnControl.DataSetType dataSetType;
        private FieldsCollections fieldsList = new FieldsCollections();
        private SQLParamCollections paramList = new SQLParamCollections();
        private string sqlExpression = string.Empty;
        private string reportPath = string.Empty;

        public SnDataSet()
        {
        }

        public object Clone()
        {
            SnDataSet set = new SnDataSet();
            set.reportPath = this.reportPath;
            set.dataSetID = this.dataSetID;
            set.dataSetName = this.dataSetName;
            set.dataSetType = this.dataSetType;
            set.DataType = this.DataType;
            set.sqlExpression = this.sqlExpression;
            FieldsCollections collections2 = (FieldsCollections)this.fieldsList.Clone();
            SQLParamCollections collections3 = (SQLParamCollections)this.paramList.Clone();
            set.fieldsList = collections2;
            set.paramList = collections3;
            return set;
        }

        public string ReportPath
        {
            get { return reportPath; }
            set { reportPath = value; }
        }

        /// <summary>
        /// 自定义或者默认报表
        /// </summary>
        public string DataType
        {
            get;
            set;
        }

        public string DataSetID
        {
            get
            {
                return this.dataSetID;
            }
            set
            {
                this.dataSetID = value;
            }
        }

        public string DataSetName
        {
            get
            {
                return this.dataSetName;
            }
            set
            {
                this.dataSetName = value;
            }
        }

        public SnControl.DataSetType DataSetType
        {
            get
            {
                return this.dataSetType;
            }
            set
            {
                this.dataSetType = value;
            }
        }

        public FieldsCollections FieldsList
        {
            get
            {
                return this.fieldsList;
            }
            set
            {
                this.fieldsList = value;
            }
        }

        public SQLParamCollections ParamList
        {
            get
            {
                return this.paramList;
            }
            set
            {
                this.paramList = value;
            }
        }

        public string SQLExpression
        {
            get
            {
                return this.sqlExpression;
            }
            set
            {
                this.sqlExpression = value;
            }
        }
    }
}

