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
        }

        public static void AddConnection(string databaseType, string connString)
        {
            AddConnection("Main", databaseType, connString);
        }

        public static void AddConnection(DatabaseType databaseType, string connString)
        {
            AddConnection("Main", databaseType, connString);
        }

        public static void AddConnection(string key, string databaseType, string connString)
        {
            AddConnection(key,
                (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType),
                connString);
        }

        public static void AddConnection(string key, DatabaseType databaseType, string connString)
        {
            lock (ConnectionList.List)
            {
                if (!ConnectionList.List.ContainsKey(key))
                    ConnectionList.List.Add(key,
                        new ConnectionInfo()
                        {
                            DatabaseType = databaseType,
                            ConnectionString = connString
                        });
            }
        }

        public static void RemoveConnection(string key)
        {
            lock (ConnectionList.List)
            {
                ConnectionList.List.Remove(key);
            }
        }
    }
}
