namespace FormsDesigner.Gui
{
    using FormsDesigner.Core;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class PropertyPad : AbstractPadContent
    {
        private PropertyContainer activeContainer;
        private ComboBox comboBox;
        private PropertyGrid grid;
        private IDesignerHost host;
        private static PropertyPad instance;
        private bool inUpdate = false;
        private Panel panel;

        public static  event PropertyValueChangedEventHandler PropertyValueChanged;

        public static  event SelectedGridItemChangedEventHandler SelectedGridItemChanged;

        public static  event EventHandler SelectedObjectChanged;

        public PropertyPad()
        {
            instance = this;
            this.panel = new Panel();
            this.grid = new PropertyGrid();
            this.grid.PropertySort = PropertySort.CategorizedAlphabetical;
            this.grid.Dock = DockStyle.Fill;
            this.grid.SelectedObjectsChanged += delegate (object sender, EventArgs e) {
                if (SelectedObjectChanged != null)
                {
                    SelectedObjectChanged(sender, e);
                }
            };
            this.grid.SelectedGridItemChanged += delegate (object sender, SelectedGridItemChangedEventArgs e) {
                if (SelectedGridItemChanged != null)
                {
                    SelectedGridItemChanged(sender, e);
                }
            };
            this.comboBox = new ComboBox();
            this.comboBox.Dock = DockStyle.Top;
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBox.Sorted = true;
            this.comboBox.DrawItem += new DrawItemEventHandler(this.ComboBoxDrawItem);
            this.comboBox.MeasureItem += new MeasureItemEventHandler(this.ComboBoxMeasureItem);
            this.comboBox.SelectedIndexChanged += new EventHandler(this.ComboBoxSelectedIndexChanged);
            this.panel.Controls.Add(this.grid);
            this.panel.Controls.Add(this.comboBox);
            this.grid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyChanged);
            //LoggingService.Debug("PropertyPad created");
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindowChanged += new EventHandler(this.WorkbenchWindowChanged);
            this.WorkbenchWindowChanged(null, null);
        }

        private void CombineClosedEvent(object sender, EventArgs e)
        {
            this.SetDesignableObjects(null);
        }

        private void ComboBoxDrawItem(object sender, DrawItemEventArgs dea)
        {
            if ((dea.Index >= 0) && (dea.Index < this.comboBox.Items.Count))
            {
                Graphics graphics = dea.Graphics;
                Brush controlText = SystemBrushes.ControlText;
                if ((dea.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    if ((dea.State & DrawItemState.Focus) == DrawItemState.Focus)
                    {
                        graphics.FillRectangle(SystemBrushes.Highlight, dea.Bounds);
                        controlText = SystemBrushes.HighlightText;
                    }
                    else
                    {
                        graphics.FillRectangle(SystemBrushes.Window, dea.Bounds);
                    }
                }
                else
                {
                    graphics.FillRectangle(SystemBrushes.Window, dea.Bounds);
                }
                object obj2 = this.comboBox.Items[dea.Index];
                int x = dea.Bounds.X;
                if (obj2 is IComponent)
                {
                    ISite site = ((IComponent) obj2).Site;
                    if (site != null)
                    {
                        string name = site.Name;
                        using (Font font = new Font(this.comboBox.Font, FontStyle.Bold))
                        {
                            graphics.DrawString(name, font, controlText, (float) x, (float) dea.Bounds.Y);
                            x += (int) graphics.MeasureString(name + "-", font).Width;
                        }
                    }
                }
                string s = obj2.GetType().ToString();
                graphics.DrawString(s, this.comboBox.Font, controlText, (float) x, (float) dea.Bounds.Y);
            }
        }

        private void ComboBoxMeasureItem(object sender, MeasureItemEventArgs mea)
        {
            if ((mea.Index < 0) || (mea.Index >= this.comboBox.Items.Count))
            {
                mea.ItemHeight = this.comboBox.Font.Height;
            }
            else
            {
                object obj2 = this.comboBox.Items[mea.Index];
                SizeF ef = mea.Graphics.MeasureString(obj2.GetType().ToString(), this.comboBox.Font);
                mea.ItemHeight = (int) ef.Height;
                mea.ItemWidth = (int) ef.Width;
                if (obj2 is IComponent)
                {
                    ISite site = ((IComponent) obj2).Site;
                    if (site != null)
                    {
                        string name = site.Name;
                        using (Font font = new Font(this.comboBox.Font, FontStyle.Bold))
                        {
                            mea.ItemWidth += (int) mea.Graphics.MeasureString(name + "-", font).Width;
                        }
                    }
                }
            }
        }

        private void ComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.inUpdate && (this.host != null))
            {
                ISelectionService service = (ISelectionService) this.host.GetService(typeof(ISelectionService));
                if (this.comboBox.SelectedIndex >= 0)
                {
                    service.SetSelectedComponents(new object[] { this.comboBox.Items[this.comboBox.SelectedIndex] });
                }
                else
                {
                    this.SetDesignableObject(null);
                    service.SetSelectedComponents(new object[0]);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.grid != null)
            {
                try
                {
                    this.grid.SelectedObjects = null;
                }
                catch
                {
                }
                this.grid.Dispose();
                this.grid = null;
                instance = null;
            }
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (PropertyValueChanged != null)
            {
                PropertyValueChanged(sender, e);
            }
        }

        private void PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            this.OnPropertyValueChanged(sender, e);
        }

        public override void RedrawContent()
        {
            this.grid.Refresh();
        }

        private void RemoveHost(IDesignerHost host)
        {
            this.host = null;
            this.grid.Site = null;
        }

        private void SelectedObjectsChanged()
        {
            if ((this.grid.SelectedObjects != null) && (this.grid.SelectedObjects.Length == 1))
            {
                for (int i = 0; i < this.comboBox.Items.Count; i++)
                {
                    if (this.grid.SelectedObject == this.comboBox.Items[i])
                    {
                        this.comboBox.SelectedIndex = i;
                    }
                }
            }
            else
            {
                this.comboBox.SelectedIndex = -1;
            }
        }

        private void SetActiveContainer(PropertyContainer pc)
        {
            if ((this.activeContainer != pc) && (pc != null))
            {
                this.activeContainer = pc;
                UpdateHostIfActive(pc);
                UpdateSelectedObjectIfActive(pc);
                UpdateSelectableIfActive(pc);
            }
        }

        private void SetDesignableObject(object obj)
        {
            this.inUpdate = true;
            this.grid.SelectedObject = obj;
            this.SelectedObjectsChanged();
            this.inUpdate = false;
        }

        private void SetDesignableObjects(object[] obj)
        {
            this.inUpdate = true;
            this.grid.SelectedObjects = obj;
            this.SelectedObjectsChanged();
            this.inUpdate = false;
        }

        private void SetDesignerHost(IDesignerHost host)
        {
            this.host = host;
            if (host != null)
            {
                this.grid.Site = new IDEContainer(host).CreateSite(this.grid);
                this.grid.PropertyTabs.AddTabType(typeof(EventsTab), PropertyTabScope.Document);
            }
            else
            {
                this.grid.Site = null;
            }
        }

        private void SetSelectableObjects(ICollection coll)
        {
            this.inUpdate = true;
            try
            {
                this.comboBox.Items.Clear();
                if (coll != null)
                {
                    foreach (object obj2 in coll)
                    {
                        this.comboBox.Items.Add(obj2);
                    }
                }
                this.SelectedObjectsChanged();
            }
            finally
            {
                this.inUpdate = false;
            }
        }

        internal static void UpdateHostIfActive(PropertyContainer container)
        {
            if ((instance != null) && (instance.activeContainer == container))
            {
                //LoggingService.Debug("UpdateHostIfActive");
                if (instance.host != container.Host)
                {
                    if (instance.host != null)
                    {
                        instance.RemoveHost(instance.host);
                    }
                    if (container.Host != null)
                    {
                        instance.SetDesignerHost(container.Host);
                    }
                }
            }
        }

        internal static void UpdateSelectableIfActive(PropertyContainer container)
        {
            if ((instance != null) && (instance.activeContainer == container))
            {
                //LoggingService.Debug("UpdateSelectableIfActive");
                instance.SetSelectableObjects(container.SelectableObjects);
            }
        }

        internal static void UpdateSelectedObjectIfActive(PropertyContainer container)
        {
            if ((instance != null) && (instance.activeContainer == container))
            {
                //LoggingService.Debug("UpdateSelectedObjectIfActive");
                if (container.SelectedObjects != null)
                {
                    instance.SetDesignableObjects(container.SelectedObjects);
                }
                else
                {
                    instance.SetDesignableObject(container.SelectedObject);
                }
            }
        }

        private void WorkbenchWindowChanged(object sender, EventArgs e)
        {
            IHasPropertyContainer activeContent = WorkbenchSingleton.Workbench.ActiveContent as IHasPropertyContainer;
            if (activeContent == null)
            {
                IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                if (activeWorkbenchWindow != null)
                {
                    activeContent = activeWorkbenchWindow.ActiveViewContent as IHasPropertyContainer;
                }
            }
            if (activeContent != null)
            {
                this.SetActiveContainer(activeContent.PropertyContainer);
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.panel;
            }
        }

        public static PropertyGrid Grid
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                return instance.grid;
            }
        }

        public static PropertyPad Instance
        {
            get
            {
                return instance;
            }
        }
    }
}

