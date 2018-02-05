namespace SnControl
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    [Serializable]
    public class FieldsCollections : CollectionBase, ICloneable
    {
        public void Add(FieldItem fieldItem)
        {
            if (fieldItem != null)
            {
                base.List.Add(fieldItem);
            }
        }

        public object Clone()
        {
            FieldsCollections collections = new FieldsCollections();
            foreach (FieldItem item in this)
            {
                FieldItem fieldItem = new FieldItem();
                fieldItem.CalcType = item.CalcType;
                fieldItem.DataTableName = item.DataTableName;
                fieldItem.TableAliasName = item.TableAliasName;
                fieldItem.DisplayOrder = item.DisplayOrder;
                fieldItem.Expression = item.Expression;
                fieldItem.FieldChineseName = item.FieldChineseName;
                fieldItem.FieldName = item.FieldName;
                fieldItem.FieldType = item.FieldType;
                fieldItem.FunctionName = item.FunctionName;
                fieldItem.IsGroup = item.IsGroup;
                fieldItem.Precision = item.Precision;
                fieldItem.ColumnVisible = item.ColumnVisible;
                fieldItem.Converge = item.Converge;
                fieldItem.DisplayWidth = item.DisplayWidth;
                fieldItem.DecimalDigits = item.DecimalDigits;
                StyleFormat[] s = new StyleFormat[item.StyleFormat.Count];
                item.StyleFormat.CopyTo(s);
                fieldItem.StyleFormat = new System.Collections.Generic.List<StyleFormat>();

                foreach (StyleFormat st in s)
                {
                    fieldItem.StyleFormat.Add(st);
                }

                collections.Add(fieldItem);
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

        public FieldItem this[string fieldFullName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    FieldItem item = (FieldItem)base.List[i];
                    if (item.FieldName == fieldFullName)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public FieldItem this[int index]
        {
            get
            {
                if ((index < 0) || (index > (base.List.Count - 1)))
                {
                    return null;
                }
                return (FieldItem)base.List[index];
            }
        }
    }
}

