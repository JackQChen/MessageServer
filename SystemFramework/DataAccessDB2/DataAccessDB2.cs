
using System;
using System.Data;
using DataAccess;
using IBM.Data.DB2;

namespace DataAccessDB2
{
    public class DataAccessDB2 : AbstractDataAccess
    {
        private DB2Connection _conn;

        public DataAccessDB2(string connectionString)
        {
            this._conn = new DB2Connection(connectionString);
        }

        #region 实现抽象类成员

        public override DatabaseType DatabaseType
        {
            get { return DatabaseType.DB2; }
        }

        public override System.Data.IDbConnection DbConnection
        {
            get { return _conn; }
        }

        public override int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<int>(count =>
                {
                    using (DB2Command cmd = new DB2Command())
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
                    using (DB2Command cmd = new DB2Command())
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

        public override System.Data.IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<IDataReader>(count =>
                {
                    using (DB2Command cmd = new DB2Command())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        IDataReader reader = cmd.ExecuteReader();
                        return reader;
                    }
                }, SQLString, cmdParms);
        }

        public override System.Data.DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<DataSet>(count =>
                {
                    using (DB2Command cmd = new DB2Command())
                    {
                        PrepareCommand(cmd, _conn, SQLString, cmdParms);
                        using (DB2DataAdapter da = new DB2DataAdapter(cmd))
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

        public override System.Data.IDataReader RunProcedure(string storedProcName, CmdParameterCollection parameters)
        {
            return this.ExecWithLog<IDataReader>(count =>
                {
                    using (DB2Command command = new DB2Command())
                    {
                        PrepareCommand(command, _conn, storedProcName, parameters);
                        command.CommandType = CommandType.StoredProcedure;
                        IDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        return returnReader;
                    }
                }, storedProcName, parameters);
        }

        public override System.Data.DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName)
        {
            return this.ExecWithLog<DataSet>(count =>
                {
                    using (DB2DataAdapter sqlDA = new DB2DataAdapter())
                    {
                        DataSet dataSet = new DataSet();
                        sqlDA.SelectCommand = new DB2Command();
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
                      using (DB2Command command = new DB2Command())
                      {
                          PrepareCommand(command, _conn, storedProcName, parameters);
                          command.CommandType = CommandType.StoredProcedure;
                          command.Parameters.Add(new DB2Parameter("ReturnValue",
                             DB2Type.Integer, 4, ParameterDirection.ReturnValue,
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

        public override void PrepareCommand(System.Data.IDbCommand cmd, System.Data.IDbConnection conn, string cmdText, CmdParameterCollection cmdParms)
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
                    DB2Parameter paras = new DB2Parameter(param.Name, param.Value);
                    paras.Direction = param.Direction;
                    paras.Size = param.Size;
                    paras.DbType = param.DbType;
                    cmd.Parameters.Add(paras);
                }
            }
        }

        #endregion
    }
}
