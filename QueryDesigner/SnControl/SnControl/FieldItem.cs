namespace SnControl
{
    using System;
    using System.Xml;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class FieldItem
    {
        private string FCalcType;
        private string FDataTableName;
        private int FDisplayOrder;
        private string FExpression;
        private string FFieldChineseName;
        private string FFieldName;
        private string FFieldType;
        private string FFunctionName;
        private bool FIsGroup;
        private bool FIsJoin;
        private bool FIsParam;
        private int FPrecision;
        private string FTableAliasName;
        private bool FColumnVisible = true;
        private bool FVisible;
        private int FDecimalDigits = 2;
        private List<StyleFormat> FStyleFormat = new List<StyleFormat>();

        private int FDisplayWidth;

        [System.Xml.Serialization.XmlElement("StyleFormat")]
        public List<StyleFormat> StyleFormat
        {
            get { return FStyleFormat; }
            set { FStyleFormat = value; }
        }

        public string CalcType
        {
            get
            {
                return this.FCalcType;
            }
            set
            {
                this.FCalcType = value;
            }
        }

        public string DataTableName
        {
            get
            {
                return this.FDataTableName;
            }
            set
            {
                this.FDataTableName = value;
            }
        }

        public int DisplayOrder
        {
            get
            {
                return this.FDisplayOrder;
            }
            set
            {
                this.FDisplayOrder = value;
            }
        }

        public string Expression
        {
            get
            {
                return this.FExpression;
            }
            set
            {
                this.FExpression = value;
            }
        }

        public string FieldChineseName
        {
            get
            {
                return this.FFieldChineseName;
            }
            set
            {
                this.FFieldChineseName = value;
            }
        }

        public string FieldName
        {
            get
            {
                return this.FFieldName;
            }
            set
            {
                this.FFieldName = value;
            }
        }

        public string FieldType
        {
            get
            {
                return this.FFieldType;
            }
            set
            {
                this.FFieldType = value;
            }
        }

        public string FunctionName
        {
            get
            {
                return this.FFunctionName;
            }
            set
            {
                this.FFunctionName = value;
            }
        }

        public bool IsGroup
        {
            get
            {
                return this.FIsGroup;
            }
            set
            {
                this.FIsGroup = value;
            }
        }

        public bool IsJoin
        {
            get
            {
                return this.FIsJoin;
            }
            set
            {
                this.FIsJoin = value;
            }
        }

        public bool IsParam
        {
            get
            {
                return this.FIsParam;
            }
            set
            {
                this.FIsParam = value;
            }
        }

        public int Precision
        {
            get
            {
                return this.FPrecision;
            }
            set
            {
                this.FPrecision = value;
            }
        }

        public string TableAliasName
        {
            get
            {
                return this.FTableAliasName;
            }
            set
            {
                this.FTableAliasName = value;
            }
        }

        public bool ColumnVisible
        {
            get
            {
                return this.FColumnVisible;
            }
            set
            {
                this.FColumnVisible = value;
            }
        }

        /// <summary>
        /// 支持旧报表(弃用)
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.FVisible;
            }
            set
            {
                this.FVisible = value;
            }
        }

        public string Converge { get; set; }

        public int DisplayWidth
        {
            get
            {
                return this.FDisplayWidth;
            }
            set
            {
                this.FDisplayWidth = value;
            }
        }

        public int DecimalDigits
        {
            get
            {
                return FDecimalDigits;
            }
            set
            {
                FDecimalDigits = value;
            }
        }
    }
}

