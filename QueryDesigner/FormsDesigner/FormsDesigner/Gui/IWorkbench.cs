namespace FormsDesigner.Gui
{
    using FormsDesigner;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public interface IWorkbench
    {
        event EventHandler ActiveWorkbenchWindowChanged;

        void CloseAllViews();
        void CloseContent(IViewContent content);
        IPadContent GetPad(Type type);
        void RedrawAllComponents();
        void ShowPad(IPadContent content);
        void ShowView(IViewContent content);
        void UnloadPad(IPadContent content);

        object ActiveContent { get; }

        IWorkbenchWindow ActiveWorkbenchWindow { get; }

        bool IsActiveWindow { get; }

        List<IPadContent> PadContentCollection { get; }

        string Title { get; set; }

        List<IViewContent> ViewContentCollection { get; }
    }
}

