namespace FormsDesigner.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public interface IViewContent : IBaseViewContent, IDisposable, ICanBeDirty
    {
        event EventHandler FileNameChanged;

        event SaveEventHandler Saved;

        event EventHandler Saving;

        event EventHandler TitleNameChanged;

        void LoadFromFileName(string fileName);
        void Save();
        void Save(string fileName);

        string FileName { get; set; }

        bool IsReadOnly { get; }

        bool IsUntitled { get; }

        bool IsViewOnly { get; }

        List<ISecondaryViewContent> SecondaryViewContents { get; }

        string TitleName { get; set; }

        string UntitledName { get; set; }
    }
}

