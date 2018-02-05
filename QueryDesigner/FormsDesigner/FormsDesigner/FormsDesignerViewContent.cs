namespace FormsDesigner
{
    using FormsDesigner.Core;
    using FormsDesigner.Gui;
    using FormsDesigner.Services;
    using FormsDesigner.UndoRedo;
    using FormsDesigner.Xml;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class FormsDesignerViewContent : AbstractSecondaryViewContent, IClipboardHandler, IUndoHandler, IHasPropertyContainer
    {
        private string activeTabName;
        private const string ComponentClipboardFormat = "CF_DESIGNERCOMPONENTS";
        private System.ComponentModel.Design.DesignSurface designSurface;
        private bool disposing;
        protected bool failedDesignerInitialize;
        private IDesignerGenerator generator;
        public bool IsFormsDesignerVisible;
        private IDesignerLoaderProvider loaderProvider;
        private Panel p;
        private FormsDesigner.Gui.PropertyContainer propertyContainer;
        private bool shouldUpdateSelectableObjects;
        private bool tabOrderMode;
        private FormsDesignerUndoEngine undoEngine;
        protected IViewContent viewContent;

        public FormsDesignerViewContent(IViewContent viewContent)
        {
            this.p = new Panel();
            this.activeTabName = "Windows Forms";
            this.shouldUpdateSelectableObjects = false;
            this.propertyContainer = new FormsDesigner.Gui.PropertyContainer();
            this.IsFormsDesignerVisible = false;
            this.tabOrderMode = false;
            if (!FormKeyHandler.inserted)
            {
                FormKeyHandler.Insert();
            }
            this.loaderProvider = new XmlDesignerLoaderProvider(string.Empty);
            this.generator = new XmlDesignerGenerator();
            this.p.BackColor = Color.White;
            this.p.RightToLeft = RightToLeft.No;
            this.p.Font = System.Windows.Forms.Control.DefaultFont;
            this.viewContent = viewContent;
        }

        public FormsDesignerViewContent(IViewContent viewContent, IDesignerLoaderProvider loaderProvider, IDesignerGenerator generator)
        {
            this.p = new Panel();
            this.activeTabName = "Windows Forms";
            this.shouldUpdateSelectableObjects = false;
            this.propertyContainer = new FormsDesigner.Gui.PropertyContainer();
            this.IsFormsDesignerVisible = false;
            this.tabOrderMode = false;
            if (!FormKeyHandler.inserted)
            {
                FormKeyHandler.Insert();
            }
            this.loaderProvider = loaderProvider;
            this.generator = generator;
            this.p.BackColor = Color.White;
            this.p.RightToLeft = RightToLeft.No;
            this.p.Font = System.Windows.Forms.Control.DefaultFont;
            this.viewContent = viewContent;
        }

        public void AlignBottom()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignBottom);
        }

        public void AlignHorizontalCenters()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignHorizontalCenters);
        }

        public void AlignLeft()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignLeft);
        }

        public void AlignRight()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignRight);
        }

        public void AlignToGrid()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignToGrid);
        }

        public void AlignTop()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignTop);
        }

        public void AlignVerticalCenters()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.AlignVerticalCenters);
        }

        public void BringToFront()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.BringToFront);
        }

        private void ComponentListChanged(object sender, EventArgs e)
        {
            this.shouldUpdateSelectableObjects = true;
        }

        public void Copy()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.Copy);
        }

        public void Cut()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.Cut);
        }

        public void Delete()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.Delete);
        }

        public void LockControls()
        {
            ((IMenuCommandService)this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.LockControls);
        }

        public void SnapToGrid()
        {
            ((IMenuCommandService)this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.SnapToGrid);
        }

        public override void Deselecting()
        {
            if (this.IsFormsDesignerVisible)
            {
                //LoggingService.Info("Deselecting form designer, unloading..." + this.viewContent.TitleName);
                PropertyPad.PropertyValueChanged -= new PropertyValueChangedEventHandler(this.PropertyValueChanged);
                this.propertyContainer.Clear();
                this.IsFormsDesignerVisible = false;
                this.activeTabName = string.Empty;
                this.UnloadDesigner();
                //LoggingService.Info("Unloading form designer finished");
            }
        }

        public override void Dispose()
        {
            this.disposing = true;
            if (this.IsFormsDesignerVisible)
            {
                this.Deselecting();
            }
            base.Dispose();
        }

        public ICollection GetCompatibleMethods(EventDescriptor edesc)
        {
            return this.generator.GetCompatibleMethods(edesc);
        }

        public ICollection GetCompatibleMethods(EventInfo edesc)
        {
            return this.generator.GetCompatibleMethods(edesc);
        }

        public virtual void HideTabOrder()
        {
            if (this.IsTabOrderMode)
            {
                ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.TabOrder);
                this.tabOrderMode = false;
            }
        }

        private bool IsMenuCommandEnabled(CommandID commandID)
        {
            if (this.designSurface == null)
            {
                return false;
            }
            IMenuCommandService service = (IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService));
            if (service == null)
            {
                return false;
            }
            System.ComponentModel.Design.MenuCommand command = service.FindCommand(commandID);
            if (command == null)
            {
                return false;
            }
            return command.Enabled;
        }

        private void LoadDesigner()
        {
            //LoggingService.Info("Form Designer: BEGIN INITIALIZE");
            DefaultServiceContainer parentProvider = new DefaultServiceContainer();
            parentProvider.AddService(typeof(IUIService), new UIService());
            parentProvider.AddService(typeof(IToolboxService), new ToolboxService());
            parentProvider.AddService(typeof(IPropertyValueUIService), new PropertyValueUIService());
            AmbientProperties serviceInstance = new AmbientProperties();
            parentProvider.AddService(typeof(AmbientProperties), serviceInstance);
            parentProvider.AddService(typeof(IDesignerEventService), new FormsDesigner.Services.DesignerEventService());
            this.designSurface = new System.ComponentModel.Design.DesignSurface(parentProvider);
            parentProvider.AddService(typeof(IMenuCommandService), new FormsDesigner.Services.MenuCommandService(this.p, this.designSurface));
            parentProvider.AddService(typeof(ITypeResolutionService), new TypeResolutionService());
            DesignerLoader loader = this.loaderProvider.CreateLoader(this.generator);
            //加载XML内容
            this.designSurface.BeginLoad(loader);
            this.generator.Attach(this);
            this.undoEngine = new FormsDesignerUndoEngine(this.Host);
            IComponentChangeService service = (IComponentChangeService) this.designSurface.GetService(typeof(IComponentChangeService));
            service.ComponentChanged += delegate {
                this.viewContent.IsDirty = true;
            };
            service.ComponentAdded += new ComponentEventHandler(this.ComponentListChanged);
            service.ComponentRemoved += new ComponentEventHandler(this.ComponentListChanged);
            service.ComponentRename += new ComponentRenameEventHandler(this.ComponentListChanged);
            this.Host.TransactionClosed += new DesignerTransactionCloseEventHandler(this.TransactionClose);
            ISelectionService service2 = (ISelectionService) this.designSurface.GetService(typeof(ISelectionService));
            service2.SelectionChanged += new EventHandler(this.SelectionChangedHandler);
            if (this.IsTabOrderMode)
            {
                this.tabOrderMode = false;
                this.ShowTabOrder();
            }
            //LoggingService.Info("Form Designer: END INITIALIZE");
        }

        public virtual void MergeFormChanges()
        {
            if (!this.failedDesignerInitialize)
            {
                bool isDirty = this.viewContent.IsDirty;
                //LoggingService.Info("Merging form changes...");
                this.designSurface.Flush();
                //LoggingService.Info("Finished merging form changes");
                this.viewContent.IsDirty = isDirty;
            }
        }

        public override void NotifyAfterSave(bool successful)
        {
            base.NotifyAfterSave(successful);
            if (successful)
            {
            }
        }

        public override void NotifyBeforeSave()
        {
            base.NotifyBeforeSave();
            if (this.IsFormsDesignerVisible)
            {
                this.MergeFormChanges();
            }
        }

        public override void NotifyFileNameChanged()
        {
            base.NotifyFileNameChanged();
        }

        public void Paste()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.Paste);
        }

        private void PropertyValueChanged(object source, PropertyValueChangedEventArgs e)
        {
            if ((((e.ChangedItem != null) && (e.OldValue != null)) && (e.ChangedItem.GridItemType == GridItemType.Property)) && ((e.ChangedItem.PropertyDescriptor.Name == "Language") && !e.OldValue.Equals(e.ChangedItem.Value)))
            {
                //LoggingService.Debug("Reloading designer due to language change.");
                this.propertyContainer.Clear();
                if (!this.failedDesignerInitialize)
                {
                    this.MergeFormChanges();
                }
                this.UnloadDesigner();
                this.Reload();
                this.UpdatePropertyPad();
            }
        }

        public virtual void Redo()
        {
            if (this.undoEngine != null)
            {
                this.undoEngine.Redo();
            }
        }

        public void Reload()
        {
            try
            {
                this.failedDesignerInitialize = false;
                //加载XML内容
                this.LoadDesigner();
                bool isDirty = this.viewContent.IsDirty;
                if ((this.designSurface != null) && (this.p.Controls.Count == 0))
                {
                    System.Windows.Forms.Control view = this.designSurface.View as System.Windows.Forms.Control;
                    view.Dock = DockStyle.Fill;
                    this.p.Controls.Add(view);
                }
                this.viewContent.IsDirty = isDirty;
            }
            catch (Exception exception)
            {
                this.failedDesignerInitialize = true;
                TextBox box = new TextBox();
                box.Multiline = true;
                box.Text = exception.ToString();
                box.Dock = DockStyle.Fill;
                this.p.Controls.Add(box);
                System.Windows.Forms.Control control2 = new Label();
                control2.Text = "${res:ICSharpCode.SharpDevelop.FormDesigner.LoadErrorCheckSourceCodeForErrors}";
                control2.Dock = DockStyle.Top;
                this.p.Controls.Add(control2);
            }
        }

        public void SelectAll()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.SelectAll);
        }

        public override void Selected()
        {
            PropertyPad.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyValueChanged);
            this.Reload();
            this.IsFormsDesignerVisible = true;
            this.UpdatePropertyPad();
        }

        private void SelectionChangedHandler(object sender, EventArgs args)
        {
            this.UpdatePropertyPadSelection((ISelectionService) sender);
        }

        public void SendToBack()
        {
            ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.SendToBack);
        }

        public void ShowSourceCode()
        {
            this.WorkbenchWindow.SwitchView(0);
        }

        public void ShowSourceCode(int lineNumber)
        {
            this.ShowSourceCode();
        }

        public void ShowSourceCode(IComponent component, EventDescriptor edesc, string eventMethodName)
        {
        }

        public virtual void ShowTabOrder()
        {
            if (!this.IsTabOrderMode)
            {
                ((IMenuCommandService) this.designSurface.GetService(typeof(IMenuCommandService))).GlobalInvoke(StandardCommands.TabOrder);
                this.tabOrderMode = true;
            }
        }

        public override void SwitchedTo()
        {
        }

        private void TransactionClose(object sender, DesignerTransactionCloseEventArgs e)
        {
            if (this.shouldUpdateSelectableObjects)
            {
                this.UpdatePropertyPad();
                this.shouldUpdateSelectableObjects = false;
            }
        }

        public virtual void Undo()
        {
            if (this.undoEngine != null)
            {
                this.undoEngine.Undo();
            }
        }

        private void UnloadDesigner()
        {
            this.generator.Detach();
            bool isDirty = this.viewContent.IsDirty;
            this.p.Controls.Clear();
            this.viewContent.IsDirty = isDirty;
            if (this.designSurface != null)
            {
                if (this.disposing)
                {
                    this.designSurface.Dispose();
                }
                else
                {
                    this.p.BeginInvoke(new MethodInvoker(this.designSurface.Dispose));
                }
                this.designSurface = null;
            }
        }

        protected void UpdatePropertyPad()
        {
            if (this.Host != null)
            {
                this.propertyContainer.Host = this.Host;
                this.propertyContainer.SelectableObjects = this.Host.Container.Components;
                ISelectionService selectionService = (ISelectionService) this.Host.GetService(typeof(ISelectionService));
                if (selectionService != null)
                {
                    this.UpdatePropertyPadSelection(selectionService);
                }
            }
        }

        private void UpdatePropertyPadSelection(ISelectionService selectionService)
        {
            ICollection selectedComponents = selectionService.GetSelectedComponents();
            object[] array = new object[selectedComponents.Count];
            selectedComponents.CopyTo(array, 0);
            this.propertyContainer.SelectedObjects = array;
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.p;
            }
        }

        public System.ComponentModel.Design.DesignSurface DesignSurface
        {
            get
            {
                return this.designSurface;
            }
        }

        public bool EnableCopy
        {
            get
            {
                return this.IsMenuCommandEnabled(StandardCommands.Copy);
            }
        }

        public bool EnableCut
        {
            get
            {
                return this.IsMenuCommandEnabled(StandardCommands.Cut);
            }
        }

        public bool EnableDelete
        {
            get
            {
                return this.IsMenuCommandEnabled(StandardCommands.Delete);
            }
        }

        public bool EnablePaste
        {
            get
            {
                return this.IsMenuCommandEnabled(StandardCommands.Paste);
            }
        }

        public bool EnableRedo
        {
            get
            {
                return ((this.undoEngine != null) && this.undoEngine.EnableRedo);
            }
        }

        public bool EnableSelectAll
        {
            get
            {
                return (this.designSurface != null);
            }
        }

        public bool EnableUndo
        {
            get
            {
                return ((this.undoEngine != null) && this.undoEngine.EnableUndo);
            }
        }

        public IDesignerHost Host
        {
            get
            {
                if (this.designSurface == null)
                {
                    return null;
                }
                return (IDesignerHost) this.designSurface.GetService(typeof(IDesignerHost));
            }
        }

        public virtual bool IsTabOrderMode
        {
            get
            {
                return this.tabOrderMode;
            }
        }

        public FormsDesigner.Gui.PropertyContainer PropertyContainer
        {
            get
            {
                return this.propertyContainer;
            }
        }

        public override string TabPageText
        {
            get
            {
                return this.viewContent.TabPageText;
            }
        }
    }
}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            