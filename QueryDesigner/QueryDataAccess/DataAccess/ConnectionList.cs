using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class ConnectionInfo
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
    }

    public class ConnectionList
    {
        private static Dictionary<string, ConnectionInfo> _list = new Dictionary<string, ConnectionInfo>();

        public static Dictionary<string, ConnectionInfo> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public static void SetConnection(string databaseType, string connString)
        {
            SetConnection((DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType), connString);
        }

        public static void SetConnection(DatabaseType databaseType, string connString)
        {
            SetConnection("Main", databaseType, connString);
        }

        public static void SetConnection(string key, DatabaseType databaseType, string connString)
        {
            lock (_list)
            {
                if (!_list.ContainsKey(key))
                    _list.Add(key,
                        new ConnectionInfo()
                        {
                            DatabaseType = databaseType,
                            ConnectionString = connString
                        });
            }
        }

        public static void RemoveConnection(string key)
        {
            lock (_list)
            {
                if (_list.ContainsKey(key))
                    _list.Remove(key);
            }
        }
    }
}
