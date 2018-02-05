namespace SnControl
{
    using System;

    [Serializable]
    public class ParamItem
    {
        private string _TableAliasName;
        private string FParamName;
        private string FTableName;

        public string ParamName
        {
            get
            {
                return this.FParamName;
            }
            set
            {
                this.FParamName = value;
            }
        }

        public string TableAliasName
        {
            get
            {
                return this._TableAliasName;
            }
            set
            {
                this._TableAliasName = value;
            }
        }

        public string TableName
        {
            get
            {
                return this.FTableName;
            }
            set
            {
                this.FTableName = value;
            }
        }
    }
}

