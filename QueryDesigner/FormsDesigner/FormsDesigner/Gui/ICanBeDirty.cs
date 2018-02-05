namespace FormsDesigner.Gui
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ICanBeDirty
    {
        event EventHandler DirtyChanged;

        bool IsDirty { get; set; }
    }
}

