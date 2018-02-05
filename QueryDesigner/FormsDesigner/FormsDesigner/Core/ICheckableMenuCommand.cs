namespace FormsDesigner.Core
{
    using System;

    public interface ICheckableMenuCommand : IMenuCommand, ICommand
    {
        bool IsChecked { get; set; }
    }
}

