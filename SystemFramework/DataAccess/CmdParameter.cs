
using System.Data;
using System;
namespace DataAccess
{
    public class CmdParameter
    {
        private string _name;
        private object _value;
        private int _size;
        private DbType _dbType;


        private ParameterDirection _direction = ParameterDirection.Input;

        public DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                if (value == null)
                    _value = DBNull.Value;
                else
                {
                    _value = value;
                    _dbType = TypeToDbType.GetDbType(value.GetType());
                }
            }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public CmdParameter()
        {
        }

        public CmdParameter(string ParameterName, object Value)
        {
            _name = ParameterName;
            this.Value = Value;
        }

        public CmdParameter(string ParameterName, object Value, DbType DbType)
        {
            _name = ParameterName;
            this.Value = Value;
            _dbType = DbType;
        }

        public CmdParameter(string ParameterName, object Value, ParameterDirection Direction)
        {
            _name = ParameterName;
            this.Value = Value;
            _direction = Direction;
        }

    }
}
