namespace FormsDesigner.Gui
{
    using System;

    public interface IStringValueFilter
    {
        string GetFilteredValue(string originalValue);
    }
}

