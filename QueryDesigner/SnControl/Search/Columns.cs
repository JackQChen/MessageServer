using System;
using System.Collections;

namespace SnControl
{
    public class Columns : CollectionBase
    {
        public Action<Column> AddColumn;
        private Search _ownSearch;

        public Columns()
        {
        }

        public Columns(Search ownSearch)
        {
            _ownSearch = ownSearch;
        }

        public Column this[int index]
        {
            get
            {
                return (Column)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public bool Contains(object obj)
        {
            return base.List.Contains(obj);
        }

        public void Add(Column column)
        {
            if (column._ownSearch == null || column._ownSearch == this._ownSearch)
                base.List.Add(column);
            else
            {
                Column clm = column.Clone() as Column;
                if (this.AddColumn != null)
                    this.AddColumn(clm);
                base.List.Add(clm);
            }
        }

        protected override void OnValidate(object value)
        {
            if (((Column)value)._ownSearch == null)
                ((Column)value)._ownSearch = this._ownSearch;
            base.OnValidate(value);
        }

        public void AddRange(Column[] values)
        {
            for (int i = 0; i < values.Length; i++)
                this.Add(values[i]);
        }

        public void Remove(int index)
        {
            if (index < base.Count - 1 && index > 0)
            {
                base.List.RemoveAt(index);
            }
        }
    }
}
