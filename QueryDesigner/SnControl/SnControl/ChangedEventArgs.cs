namespace SnControl
{
    using System;

    public class ChangedEventArgs : EventArgs
    {
        public object changedComponent;
        public ChangedType changeType;
    }
}

