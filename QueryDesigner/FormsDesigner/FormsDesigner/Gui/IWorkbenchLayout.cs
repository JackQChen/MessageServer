namespace FormsDesigner.Gui
{
    using FormsDesigner;
    using System;
    using System.Runtime.CompilerServices;

    public interface IWorkbenchLayout
    {
        event EventHandler ActiveWorkbenchWindowChanged;

        void ActivatePad(IPadContent content);
        void ActivatePad(string fullyQualifiedTypeName);
        void Attach(IWorkbench workbench);
        void Detach();
        void HidePad(IPadContent content);
        bool IsVisible(IPadContent padContent);
        void LoadConfiguration();
        void OnActiveWorkbenchWindowChanged(EventArgs e);
        void RedrawAllComponents();
        void ShowPad(IPadContent content);
        IWorkbenchWindow ShowView(IViewContent content);
        void StoreConfiguration();
        void UnloadPad(IPadContent content);

        object ActiveContent { get; }

        IWorkbenchWindow ActiveWorkbenchwindow { get; }
    }
}

