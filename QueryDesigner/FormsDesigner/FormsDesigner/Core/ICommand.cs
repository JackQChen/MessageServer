namespace FormsDesigner.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ICommand
    {
        event EventHandler OwnerChanged;

        void Run();

        object Owner { get; set; }
    }
}

