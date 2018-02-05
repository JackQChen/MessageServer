namespace SnControl
{
    using System;

    public class ConChangedEventArgs : EventArgs
    {
        public ChangedType changeType;
        public object From;
        public string FromFieldName;
        public string FromTableName;
        public object To;
        public string ToFieldName;
        public string ToTableName;
    }
}

