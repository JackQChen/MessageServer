namespace SnControl
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    [Serializable]
    public class SQLParamCollections : CollectionBase, ICloneable
    {
        public void Add(SQLParamItem paramItem)
        {
            if (paramItem != null)
            {
                base.List.Add(paramItem);
            }
        }

        public object Clone()
        {
            SQLParamCollections collections = new SQLParamCollections();
            foreach (SQLParamItem item in this)
            {
                SQLParamItem copyItem = new SQLParamItem();
                copyItem.ParamName = item.ParamName;
                copyItem.ParamType = item.ParamType;
                collections.Add(copyItem);
            }
            return collections;
        }

        public void Insert(int index, FieldItem fieldItem)
        {
            base.InnerList.Insert(index, fieldItem);
        }

        public void Remove(FieldItem fieldItem)
        {
            base.InnerList.Remove(fieldItem);
        }

        public void Remove(int index)
        {
            if ((index > (base.List.Count - 1)) || (index < 0))
            {
                MessageBox.Show("无效的索引值!");
            }
            else
            {
                base.List.RemoveAt(index);
            }
        }

        public SQLParamItem this[string paramFullName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    SQLParamItem item = (SQLParamItem)base.List[i];
                    if (item.ParamName == paramFullName)
                        return item;
                }
                return null;
            }
        }

        public SQLParamItem this[int index]
        {
            get
            {
                if ((index < 0) || (index > (base.List.Count - 1)))
                {
                    return null;
                }
                return (SQLParamItem)base.List[index];
            }
        }
    }
}

