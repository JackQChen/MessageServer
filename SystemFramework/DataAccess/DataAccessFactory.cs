using System;
using System.IO;
using System.Reflection;

namespace DataAccess
{
    public class DataAccessFactory
    {
        public DataAccessFactory()
        {
        }

        private static DataAccessFactory instance;

        public static DataAccessFactory Instance
        {
            get
            {
                if (DataAccessFactory.instance == null)
                    DataAccessFactory.instance = new DataAccessFactory();
                return DataAccessFactory.instance;
            }
        }

        public IDataAccess CreateDataAccess()
        {
            return this.CreateDataAccess("Main");
        }

        public IDataAccess CreateDataAccess(string dataKey)
        {
            if (!ConnectionList.List.ContainsKey(dataKey))
                throw new Exception("数据访问尚未就绪");
            var item = ConnectionList.List[dataKey];
            return this.CreateDataAccess(item.DatabaseType, item.ConnectionString);
        }

        public IDataAccess CreateDataAccess(DatabaseType DBType, string DBConnectionString)
        {
            IDataAccess data = null;
            switch (DBType)
            {
                case DatabaseType.SQLite: data = CreateDataAccess("DataAccessSQLite", "DataAccessSQLite.DataAccessSQLite", DBConnectionString); break;
                case DatabaseType.MSSQLServer: data = CreateDataAccess("DataAccessSQL", "DataAccessSQL.DataAccessSQL", DBConnectionString); break;
                case DatabaseType.DB2: data = CreateDataAccess("DataAccessDB2", "DataAccessDB2.DataAccessDB2", DBConnectionString); break;
                case DatabaseType.Oracle: data = CreateDataAccess("DataAccessOracle", "DataAccessOracle.DataAccessOracle", DBConnectionString); break;
                case DatabaseType.MySQL: data = CreateDataAccess("DataAccessMySQL", "DataAccessMySQL.DataAccessMySQL", DBConnectionString); break;
            }
            return data;
        }

        private IDataAccess CreateDataAccess(string path, string typeName, string DbConnectionString)
        {
            ConstructorInfo construct = null;
            lock (ConstructorCache.dic)
            {
                if (ConstructorCache.Exist(typeName))
                    construct = ConstructorCache.GetCache(typeName);
                else
                {
                    string dllPath = System.AppDomain.CurrentDomain.BaseDirectory + path + ".dll";
                    if (!File.Exists(dllPath))
                        throw new Exception(dllPath + "没有找到!");
                    Type type = Assembly.LoadFile(dllPath).GetType(typeName);
                    Type[] tpPara = new Type[] { typeof(string) };
                    construct = type.GetConstructor(tpPara);
                    ConstructorCache.AddCache(typeName, construct);
                }
            }
            IDataAccess instance = null;
            try
            {
                instance = (IDataAccess)construct.Invoke(new object[] { DbConnectionString });
            }
            catch
            {
                throw new Exception("请检查是否缺少必要的数据库连接文件");
            }
            return instance;
        }

    }
}
