namespace FormsDesigner.Gui
{
    using System;
    using System.Windows.Forms;

    public interface IBaseViewContent : IDisposable
    {
        void Deselected();
        void Deselecting();
        void RedrawContent();
        void Selected();
        void SwitchedTo();

        System.Windows.Forms.Control Control { get; }

        string TabPageText { get; }

        IWorkbenchWindow WorkbenchWindow { get; set; }
    }
}

