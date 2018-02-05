namespace FormsDesigner.UndoRedo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;

    public class FormsDesignerUndoEngine : UndoEngine, IUndoHandler
    {
        private Stack<UndoEngine.UndoUnit> redoStack;
        private Stack<UndoEngine.UndoUnit> undoStack;

        public FormsDesignerUndoEngine(IServiceProvider provider) : base(provider)
        {
            this.undoStack = new Stack<UndoEngine.UndoUnit>();
            this.redoStack = new Stack<UndoEngine.UndoUnit>();
        }

        protected override void AddUndoUnit(UndoEngine.UndoUnit unit)
        {
            this.undoStack.Push(unit);
        }

        public void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                UndoEngine.UndoUnit item = this.redoStack.Pop();
                item.Undo();
                this.undoStack.Push(item);
            }
        }

        public void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                UndoEngine.UndoUnit item = this.undoStack.Pop();
                item.Undo();
                this.redoStack.Push(item);
            }
        }

        public bool EnableRedo
        {
            get
            {
                return (this.redoStack.Count > 0);
            }
        }

        public bool EnableUndo
        {
            get
            {
                return (this.undoStack.Count > 0);
            }
        }
    }
}

