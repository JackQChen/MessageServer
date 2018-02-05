namespace FormsDesigner.Gui
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IWorkbenchWindow
    {
        event EventHandler CloseEvent;

        event EventHandler TitleChanged;

        event EventHandler WindowDeselected;

        event EventHandler WindowSelected;

        bool CloseWindow(bool force);
        void OnWindowDeselected(EventArgs e);
        void OnWindowSelected(EventArgs e);
        void RedrawContent();
        void SelectWindow();
        void SwitchView(int viewNumber);

        IBaseViewContent ActiveViewContent { get; }

        bool IsDisposed { get; }

        string Title { get; set; }

        IViewContent ViewContent { get; }
    }
}

