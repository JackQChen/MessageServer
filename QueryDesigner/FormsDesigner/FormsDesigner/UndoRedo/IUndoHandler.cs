namespace FormsDesigner.UndoRedo
{
    using System;

    public interface IUndoHandler
    {
        void Redo();
        void Undo();

        bool EnableRedo { get; }

        bool EnableUndo { get; }
    }
}

