namespace FormsDesigner.Gui
{
    using System;

    public abstract class AbstractSecondaryViewContent : AbstractBaseViewContent, ISecondaryViewContent, IBaseViewContent, IDisposable
    {
        protected AbstractSecondaryViewContent()
        {
        }

        public virtual void NotifyAfterSave(bool successful)
        {
        }

        public virtual void NotifyBeforeSave()
        {
        }

        public virtual void NotifyFileNameChanged()
        {
        }
    }
}

