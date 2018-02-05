namespace FormsDesigner.Services
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ToolboxService : IToolboxService
    {
        private IDictionary creators = new HybridDictionary();
        private IDictionary creatorsByHost = new ListDictionary();
        private string selectedCategory = null;
        private ToolboxItem selectedItem = null;
        private IDictionary toolboxByCategory = new ListDictionary();
        private IDictionary toolboxByHost = new ListDictionary();
        private ArrayList toolboxItems = new ArrayList();

        public event EventHandler SelectedCategoryChanged;

        public event EventHandler SelectedCategoryChanging;

        public event EventHandler SelectedItemChanged;

        public event EventHandler SelectedItemChanging;

        public event EventHandler SelectedItemUsed;

        public event ToolboxEventHandler ToolboxItemAdded;

        public event ToolboxEventHandler ToolboxItemRemoved;

        public ToolboxService()
        {
            IList list = new ArrayList();
            this.toolboxByCategory.Add("查询方案", list);
            list = new ArrayList();
            this.toolboxByHost.Add("ALL_HOSTS", list);
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format)
        {
            this.AddCreator(creator, format, null);
        }

        public void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
        {
            if (host == null)
            {
                this.creators.Add(format, creator);
            }
            else
            {
                IDictionary dictionary = (IDictionary) this.creatorsByHost[host];
                if (dictionary == null)
                {
                    dictionary = new HybridDictionary();
                    this.creatorsByHost.Add(host, dictionary);
                }
                dictionary[format] = creator;
            }
        }

        public void AddItemToToolbox(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
            IList list;
            this.toolboxItems.Add(toolboxItem);
            if (host != null)
            {
                list = (IList) this.toolboxByHost[host];
                if (list == null)
                {
                    list = new ArrayList();
                    this.toolboxByHost.Add(host, list);
                }
                list.Add(toolboxItem);
            }
            else
            {
                list = (IList) this.toolboxByHost["ALL_HOSTS"];
                list.Add(toolboxItem);
            }
            if (category != null)
            {
                list = (IList) this.toolboxByCategory[category];
                if (list == null)
                {
                    list = new ArrayList();
                    this.toolboxByCategory.Add(category, list);
                }
                list.Add(toolboxItem);
            }
            else
            {
                ((IList) this.toolboxByCategory["查询方案"]).Add(toolboxItem);
            }
            this.FireToolboxItemAdded(toolboxItem, category, host);
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
        {
            this.AddLinkedToolboxItem(toolboxItem, null, host);
        }

        public void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
            this.AddItemToToolbox(toolboxItem, category, host);
        }

        public void AddToolboxItem(ToolboxItem toolboxItem)
        {
            this.AddItemToToolbox(toolboxItem, null, null);
        }

        public void AddToolboxItem(ToolboxItem toolboxItem, string category)
        {
            this.AddItemToToolbox(toolboxItem, category, null);
        }

        public ToolboxItem DeserializeToolboxItem(object serializedObject)
        {
            if ((serializedObject is IDataObject) && ((IDataObject) serializedObject).GetDataPresent(typeof(ToolboxItem)))
            {
                return (ToolboxItem) ((IDataObject) serializedObject).GetData(typeof(ToolboxItem));
            }
            return null;
        }

        public ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host)
        {
            if ((serializedObject is IDataObject) && ((IDataObject) serializedObject).GetDataPresent(typeof(ToolboxItem)))
            {
                ToolboxItem data = (ToolboxItem) ((IDataObject) serializedObject).GetData(typeof(ToolboxItem));
                if (host != null)
                {
                    ArrayList list = (ArrayList) this.toolboxByHost[host];
                    if ((list != null) && list.Contains(data))
                    {
                        return data;
                    }
                    list = (ArrayList) this.toolboxByHost["ALL_HOSTS"];
                    if ((list != null) && list.Contains(data))
                    {
                        return data;
                    }
                }
            }
            return null;
        }

        private void FireSelectedCategoryChanged()
        {
            if (this.SelectedCategoryChanged != null)
            {
                this.SelectedCategoryChanged(this, EventArgs.Empty);
            }
        }

        private void FireSelectedCategoryChanging()
        {
            if (this.SelectedCategoryChanging != null)
            {
                this.SelectedCategoryChanging(this, EventArgs.Empty);
            }
        }

        private void FireSelectedItemChanged()
        {
            if (this.SelectedCategoryChanged != null)
            {
                this.SelectedItemChanged(this, EventArgs.Empty);
            }
        }

        private void FireSelectedItemChanging()
        {
            if (this.SelectedCategoryChanging != null)
            {
                this.SelectedItemChanging(this, EventArgs.Empty);
            }
        }

        private void FireSelectedItemUsed()
        {
            if (this.SelectedItemUsed != null)
            {
                this.SelectedItemUsed(this, EventArgs.Empty);
            }
        }

        private void FireToolboxItemAdded(ToolboxItem item, string category, IDesignerHost host)
        {
            if (this.ToolboxItemAdded != null)
            {
                this.ToolboxItemAdded(this, new ToolboxEventArgs(item, category, host));
            }
        }

        private void FireToolboxItemRemoved(ToolboxItem item, string category, IDesignerHost host)
        {
            if (this.ToolboxItemAdded != null)
            {
                this.ToolboxItemRemoved(this, new ToolboxEventArgs(item, category, host));
            }
        }

        public ToolboxItem GetSelectedToolboxItem()
        {
            return this.selectedItem;
        }

        public ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
        {
            IList list = (IList) this.toolboxByHost[host];
            if ((list != null) && list.Contains(this.selectedItem))
            {
                return this.selectedItem;
            }
            list = (IList) this.toolboxByHost["ALL_HOSTS"];
            if (list.Contains(this.selectedItem))
            {
                return this.selectedItem;
            }
            return null;
        }

        public ToolboxItemCollection GetToolboxItems()
        {
            ToolboxItem[] array = new ToolboxItem[this.toolboxItems.Count];
            this.toolboxItems.CopyTo(array);
            return new ToolboxItemCollection(array);
        }

        public ToolboxItemCollection GetToolboxItems(IDesignerHost host)
        {
            ArrayList list = null;
            if (host == null)
            {
                list = (ArrayList) this.toolboxByHost["ALL_HOSTS"];
            }
            else
            {
                list = (ArrayList) this.toolboxByHost[host];
            }
            ArrayList list2 = (ArrayList) this.toolboxByHost[host];
            list2.Add((ArrayList) this.toolboxByHost["ALL_HOSTS"]);
            ToolboxItem[] array = new ToolboxItem[list2.Count];
            this.toolboxItems.CopyTo(array);
            return new ToolboxItemCollection(array);
        }

        public ToolboxItemCollection GetToolboxItems(string category)
        {
            if (category == null)
            {
                category = "查询方案";
            }
            ArrayList list = (ArrayList) this.toolboxByCategory[category];
            list.Add((ArrayList) this.toolboxByCategory["查询方案"]);
            ToolboxItem[] array = new ToolboxItem[list.Count];
            this.toolboxItems.CopyTo(array);
            return new ToolboxItemCollection(array);
        }

        public ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
        {
            if (category == null)
            {
                category = "查询方案";
            }
            ArrayList list = null;
            if (host == null)
            {
                list = (ArrayList) this.toolboxByHost["ALL_HOSTS"];
            }
            else
            {
                list = (ArrayList) this.toolboxByHost[host];
            }
            ArrayList list2 = (ArrayList) this.toolboxByCategory[category];
            ArrayList list3 = new ArrayList();
            foreach (ToolboxItem item in list)
            {
                if (list2.Contains(item))
                {
                    list3.Add(item);
                }
            }
            ToolboxItem[] array = new ToolboxItem[list3.Count];
            this.toolboxItems.CopyTo(array);
            return new ToolboxItemCollection(array);
        }

        public bool IsSupported(object serializedObject, ICollection filterAttributes)
        {
            return false;
        }

        public bool IsSupported(object serializedObject, IDesignerHost host)
        {
            return false;
        }

        public bool IsToolboxItem(object serializedObject)
        {
            return ((serializedObject is IDataObject) && ((IDataObject) serializedObject).GetDataPresent(typeof(ToolboxItem)));
        }

        public bool IsToolboxItem(object serializedObject, IDesignerHost host)
        {
            if ((serializedObject is IDataObject) && ((IDataObject) serializedObject).GetDataPresent(typeof(ToolboxItem)))
            {
                ToolboxItem data = (ToolboxItem) ((IDataObject) serializedObject).GetData(typeof(ToolboxItem));
                if (host != null)
                {
                    ArrayList list = (ArrayList) this.toolboxByHost[host];
                    if ((list != null) && list.Contains(data))
                    {
                        return true;
                    }
                    list = (ArrayList) this.toolboxByHost["ALL_HOSTS"];
                    if ((list != null) && list.Contains(data))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Refresh()
        {
        }

        public void RemoveCreator(string format)
        {
        }

        public void RemoveCreator(string format, IDesignerHost host)
        {
        }

        public void RemoveToolboxItem(ToolboxItem toolboxItem)
        {
            this.toolboxItems.Remove(toolboxItem);
            ((ArrayList) this.toolboxByCategory["查询方案"]).Remove(toolboxItem);
            ((ArrayList) this.toolboxByHost["ALL_HOSTS"]).Remove(toolboxItem);
            this.FireToolboxItemRemoved(toolboxItem, null, null);
        }

        public void RemoveToolboxItem(ToolboxItem toolboxItem, string category)
        {
            this.toolboxItems.Remove(toolboxItem);
            ((ArrayList) this.toolboxByCategory[category]).Remove(toolboxItem);
            this.FireToolboxItemRemoved(toolboxItem, category, null);
        }

        public void SelectedToolboxItemUsed()
        {
            this.FireSelectedItemUsed();
        }

        public object SerializeToolboxItem(ToolboxItem toolboxItem)
        {
            return null;
        }

        public bool SetCursor()
        {
            if (this.selectedItem == null)
            {
                return false;
            }
            if (this.selectedItem.DisplayName == "Pointer")
            {
                return false;
            }
            return true;
        }

        public void SetSelectedToolboxItem(ToolboxItem toolboxItem)
        {
            if (toolboxItem != this.selectedItem)
            {
                this.FireSelectedItemChanging();
                this.selectedItem = toolboxItem;
                this.FireSelectedItemChanged();
            }
        }

        public CategoryNameCollection CategoryNames
        {
            get
            {
                string[] array = new string[this.toolboxByCategory.Count];
                this.toolboxByCategory.Keys.CopyTo(array, 0);
                return new CategoryNameCollection(array);
            }
        }

        public string SelectedCategory
        {
            get
            {
                return this.selectedCategory;
            }
            set
            {
                if (value != this.selectedCategory)
                {
                    this.FireSelectedCategoryChanging();
                    this.selectedCategory = value;
                    this.FireSelectedCategoryChanged();
                }
            }
        }
    }
}

