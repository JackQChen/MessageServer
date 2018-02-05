using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess;

namespace DataAccessSQL
{
    /// <summary>
    /// 数据访问类 
    /// </summary>
    public class DataAccessSQL : AbstractDataAccess
    {

        private SqlConnection _conn;

        public DataAccessSQL(string connectionString)
        {
            this._conn = new SqlConnection(connectionString);
        }

        public override DatabaseType DatabaseType
        {
            get { return DatabaseType.MSSQLServer; }
        }

        public override IDbConnection DbConnection
        {
            get { return _conn; }
        }

        public override int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<int>(count =>
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, this.DbConnection, SQLString, cmdParms);
                        int rowCount = cmd.ExecuteNonQuery();
                        count(rowCount.ToString());
                        return rowCount;
                    }
                }, SQLString, cmdParms);
        }

        public override object ExecuteScalar(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<object>(count =>
                {
                    using (SqlCommand cmd = new SqlCommand())
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

        public override IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<IDataReader>(count =>
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        SqlDataReader reader = cmd.ExecuteReader();
                        return reader;
                    }
                }, SQLString, cmdParms);
        }

        public override DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<DataSet>(count =>
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return this.ExecWithLog<IDataReader>(count =>
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        PrepareCommand(command, _conn, storedProcName, parameters);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        return returnReader;
                    }
                }, storedProcName, parameters);
        }

        public override DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName)
        {
            return this.ExecWithLog<DataSet>(count =>
                {
                    using (SqlDataAdapter sqlDA = new SqlDataAdapter())
                    {
                        DataSet dataSet = new DataSet();
                        sqlDA.SelectCommand = new SqlCommand();
                        PrepareCommand(sqlDA.SelectCommand, _conn, storedProcName, parameters);
                        sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDA.Fill(dataSet, tableName);
                        if (dataSet.Tables.Count > 0)
                            count(dataSet.Tables[0].Rows.Count.ToString());
                        else
                            count("0");
                        return dataSet;
                    }
                }, storedProcName, parameters);
        }

        public override int RunProcedure(string storedProcName, CmdParameterCollection parameters, out int rowsAffected)
        {
            int result = 0, affectCount = 0;
            result = this.ExecWithLog<int>(count =>
                  {
                      using (SqlCommand command = new SqlCommand())
                      {
                          PrepareCommand(command, _conn, storedProcName, parameters);
                          command.CommandType = CommandType.StoredProcedure;
                          command.Parameters.Add(new SqlParameter("ReturnValue",
                              SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                              false, 0, 0, string.Empty, DataRowVersion.Default, null));
                          affectCount = command.ExecuteNonQuery();
                          int rValue = (int)command.Parameters["ReturnValue"].Value;
                          count(rValue.ToString());
                          return rValue;
                      }
                  }, storedProcName, parameters);
            rowsAffected = affectCount;
            return result;
        }

        public override void PrepareCommand(IDbCommand cmd, IDbConnection conn, string cmdText, CmdParameterCollection cmdParms)
        {
            cmd.Connection = _conn;
            cmd.CommandText = cmdText;
            cmd.Transaction = Trans;
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (CmdParameter param in cmdParms)
                {
                    SqlParameter paras = new SqlParameter(param.Name, param.Value);
                    paras.Direction = param.Direction;
                    paras.Size = param.Size;
                    paras.DbType = param.DbType;
                    cmd.Parameters.Add(paras);
                }
            }
        }
    }

}
