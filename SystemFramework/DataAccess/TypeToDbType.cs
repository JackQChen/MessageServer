
using System;
using System.Data;
namespace DataAccess
{
    public static class TypeToDbType
    {
        public static DbType GetDbType(Type type)
        {
            DbType dbt;
            try
            {
                if (type.Equals(typeof(byte[])))
                    return DbType.Binary;
                dbt = (DbType)Enum.Parse(typeof(DbType), type.Name);
            }
            catch (ArgumentException)
            {
                dbt = DbType.Object;
            }
            return dbt;
        }
    }
}
