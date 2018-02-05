namespace FormsDesigner.Gui
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class AbstractViewContent : AbstractBaseViewContent, IViewContent, IBaseViewContent, IDisposable, ICanBeDirty
    {
        private string fileName;
        private bool isDirty;
        private bool isViewOnly;
        private List<ISecondaryViewContent> secondaryViewContents;
        private string titleName;
        private string untitledName;

        public event EventHandler DirtyChanged;

        public event EventHandler FileNameChanged;

        public event SaveEventHandler Saved;

        public event EventHandler Saving;

        public event EventHandler TitleNameChanged;

        public AbstractViewContent()
        {
            this.untitledName = string.Empty;
            this.titleName = null;
            this.fileName = null;
            this.isViewOnly = false;
            this.secondaryViewContents = new List<ISecondaryViewContent>();
            this.isDirty = false;
        }

        public AbstractViewContent(string titleName)
        {
            this.untitledName = string.Empty;
            this.titleName = null;
            this.fileName = null;
            this.isViewOnly = false;
            this.secondaryViewContents = new List<ISecondaryViewContent>();
            this.isDirty = false;
            this.titleName = titleName;
        }

        public AbstractViewContent(string titleName, string fileName)
        {
            this.untitledName = string.Empty;
            this.titleName = null;
            this.fileName = null;
            this.isViewOnly = false;
            this.secondaryViewContents = new List<ISecondaryViewContent>();
            this.isDirty = false;
            this.titleName = titleName;
            this.fileName = fileName;
        }

        public override void Dispose()
        {
            foreach (ISecondaryViewContent content in this.secondaryViewContents)
            {
                content.Dispose();
            }
            base.Dispose();
        }

        public virtual void LoadFromFileName(string fileName)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnDirtyChanged(EventArgs e)
        {
            if (this.DirtyChanged != null)
            {
                this.DirtyChanged(this, e);
            }
        }

        protected virtual void OnFileNameChanged(EventArgs e)
        {
            foreach (ISecondaryViewContent content in this.SecondaryViewContents)
            {
                content.NotifyFileNameChanged();
            }
            if (this.FileNameChanged != null)
            {
                this.FileNameChanged(this, e);
            }
        }

        protected virtual void OnSaved(SaveEventArgs e)
        {
            foreach (ISecondaryViewContent content in this.SecondaryViewContents)
            {
                content.NotifyAfterSave(e.Successful);
            }
            if (this.Saved != null)
            {
                this.Saved(this, e);
            }
        }

        protected virtual void OnSaving(EventArgs e)
        {
            foreach (ISecondaryViewContent content in this.SecondaryViewContents)
            {
                content.NotifyBeforeSave();
            }
            if (this.Saving != null)
            {
                this.Saving(this, e);
            }
        }

        protected virtual void OnTitleNameChanged(EventArgs e)
        {
            if (this.TitleNameChanged != null)
            {
                this.TitleNameChanged(this, e);
            }
        }

        public virtual void Save()
        {
            if (this.IsDirty)
            {
                this.Save(this.fileName);
            }
        }

        public virtual void Save(string fileName)
        {
            throw new NotImplementedException();
        }

        protected void SetTitleAndFileName(string fileName)
        {
            this.TitleName = Path.GetFileName(fileName);
            this.FileName = fileName;
            this.IsDirty = false;
        }

        public virtual string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
                this.OnFileNameChanged(EventArgs.Empty);
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                return this.isDirty;
            }
            set
            {
                if (this.isDirty != value)
                {
                    this.isDirty = value;
                    this.OnDirtyChanged(EventArgs.Empty);
                }
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsUntitled
        {
            get
            {
                return (this.titleName == null);
            }
        }

        public virtual bool IsViewOnly
        {
            get
            {
                return this.isViewOnly;
            }
            set
            {
                this.isViewOnly = value;
            }
        }

        public List<ISecondaryViewContent> SecondaryViewContents
        {
            get
            {
                return this.secondaryViewContents;
            }
        }

        public virtual string TitleName
        {
            get
            {
                return (this.titleName ?? Path.GetFileName(this.untitledName));
            }
            set
            {
                this.titleName = value;
                this.OnTitleNameChanged(EventArgs.Empty);
            }
        }

        public virtual string UntitledName
        {
            get
            {
                return this.untitledName;
            }
            set
            {
                this.untitledName = value;
            }
        }
    }
}

