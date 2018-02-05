namespace FormsDesigner.Gui
{
    using System;

    public interface IPropertyValueCreator
    {
        bool CanCreateValueForType(Type propertyType);
        object CreateValue(Type propertyType, string valueString);
    }
}

