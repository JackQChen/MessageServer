namespace FormsDesigner.Gui
{
    using System;

    public interface IClipboardHandler
    {
        void Copy();
        void Cut();
        void Delete();
        void Paste();
        void SelectAll();

        bool EnableCopy { get; }

        bool EnableCut { get; }

        bool EnableDelete { get; }

        bool EnablePaste { get; }

        bool EnableSelectAll { get; }
    }
}

