using System;
using System.Collections;
using System.Data;

namespace DataAccess
{
    public class CmdParameterCollection : CollectionBase
    {
        public CmdParameter Add(CmdParameter para)
        {
            this.List.Add(para);
            return para;
        }

        public CmdParameter Add(string ParameterName, object Value)
        {
            return Add(new CmdParameter(ParameterName, Value));
        }

        public CmdParameter Add(string ParameterName, object Value, DbType type)
        {
            return Add(new CmdParameter(ParameterName, Value, type));
        }

        public CmdParameter Add(string ParameterName, object Value, ParameterDirection Direction)
        {
            return Add(new CmdParameter(ParameterName, Value, Direction));
        }

        public CmdParameter this[int Index]
        {
            get
            {
                CheckIndex(Index);
                return (CmdParameter)this.List[Index];
            }
            set
            {
                this.List[Index] = value;
            }
        }

        public CmdParameter this[string ParameterName]
        {
            get
            {
                int i = CheckName(ParameterName);
                if (i < 0)
                    throw new IndexOutOfRangeException("ParameterName " + ParameterName + " dose not exist");
                else
                    return (CmdParameter)this.List[i];
            }
            set
            {
                this.List[CheckName(ParameterName)] = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new IndexOutOfRangeException("Number " + index.ToString() + " is out of Range");
            }
        }

        private int CheckName(string Name)
        {
            int index = -1;
            for (int i = 0; i < this.List.Count; i++)
            {
                if (((CmdParameter)this.List[i]).Name.Equals(Name))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

    }
}
