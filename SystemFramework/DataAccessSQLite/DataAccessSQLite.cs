using System;
using System.Data;
using System.Data.SQLite;
using DataAccess;

namespace DataAccessSQLite
{
    /// <summary> 
    /// 数据访问基础类(基于SQLite) 
    /// </summary>
    public class DataAccessSQLite : AbstractDataAccess
    {

        private SQLiteConnection _conn;

        public DataAccessSQLite(string connectionString)
        {
            this._conn = new SQLiteConnection(connectionString);
        }

        #region 公用方法

        public override DatabaseType DatabaseType
        {
            get { return DatabaseType.SQLite; }
        }

        public override IDbConnection DbConnection
        {
            get { return _conn; }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public override int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<int>(count =>
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        PrepareCommand(cmd, this.DbConnection, SQLString, cmdParms);
                        int rowCount = cmd.ExecuteNonQuery();
                        count(rowCount.ToString());
                        return rowCount;
                    }
                }, SQLString, cmdParms);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public override object ExecuteScalar(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<object>(count =>
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            count("0");
                            return null;
                        }
                        else
                        {
                            count("1");
                            return obj;
                        }
                    }
                }, SQLString, cmdParms);
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public override IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<IDataReader>(count =>
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        return reader;
                    }
                }, SQLString, cmdParms);
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public override DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<DataSet>(count =>
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds, "DataSet");
                            if (ds.Tables.Count > 0)
                                count(ds.Tables[0].Rows.Count.ToString());
                            else
                                count("0");
                            return ds;
                        }
                    }
                }, SQLString, cmdParms);
        }

        public override IDataReader RunProcedure(string storedProcName, CmdParameterCollection parameters)
        {
            throw new NotImplementedException();
        }

        public override DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName)
        {
            throw new NotImplementedException();
        }

        public override int RunProcedure(string storedProcName, CmdParameterCollection parameters, out int rowsAffected)
        {
            throw new NotImplementedException();
        }

        public override void PrepareCommand(IDbCommand cmd, IDbConnection conn, string cmdText, CmdParameterCollection cmdParms)
        {
            cmd.Connection = _conn;
            cmd.CommandText = cmdText;
            cmd.Transaction = Trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 600;
            if (cmdParms != null)
            {
                foreach (CmdParameter param in cmdParms)
                {
                    SQLiteParameter paras = new SQLiteParameter(param.Name, param.DbType, param.Size);
                    paras.Direction = param.Direction;
                    paras.Value = param.Value;
                    cmd.Parameters.Add(paras);
                }
            }
        }

        #endregion
    }
}
