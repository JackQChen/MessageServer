namespace FormsDesigner.Core
{
    using System;

    public interface IMenuCommand : ICommand
    {
        bool IsEnabled { get; set; }
    }
}

