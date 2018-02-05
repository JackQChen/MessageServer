namespace FormsDesigner.Gui
{
    using System;
    using System.Xml;

    public interface IObjectCreator
    {
        object CreateObject(string name, XmlElement el, object obj);
        Type GetType(string name);
    }
}

