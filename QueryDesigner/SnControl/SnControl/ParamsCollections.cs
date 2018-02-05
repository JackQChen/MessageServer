namespace SnControl
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    [Serializable]
    public class ParamsCollections : CollectionBase, ICloneable
    {
        public void Add(ParamItem paramItem)
        {
            base.List.Add(paramItem);
        }

        public object Clone()
        {
            ParamsCollections collections = new ParamsCollections();
            foreach (ParamItem item in this)
            {
                ParamItem paramItem = new ParamItem();
                paramItem.ParamName = item.ParamName;
                paramItem.TableName = item.TableName;
                paramItem.TableAliasName = item.TableAliasName;
                collections.Add(paramItem);
            }
            return collections;
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

        public ParamItem this[string paramName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    ParamItem item = (ParamItem) base.List[i];
                    if (item.ParamName == paramName)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public ParamItem this[int index]
        {
            get
            {
                if ((index < 0) || (index > (base.List.Count - 1)))
                {
                    return null;
                }
                return (ParamItem) base.List[index];
            }
        }
    }
}

