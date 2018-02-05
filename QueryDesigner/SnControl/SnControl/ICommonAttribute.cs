namespace SnControl
{
    using System;

    public interface ICommonAttribute
    {

        string DataSetName { get; set; }

        string Function { get; set; }

        string ParamName { get; set; }

        string ParamType { get; set; }

        string Value { get; }
    }
}

