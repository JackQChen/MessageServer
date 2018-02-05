using System;
using System.Data;
using DataAccess;
using MySql.Data.MySqlClient;

namespace DataAccessMySQL
{
    public class DataAccessMySQL : AbstractDataAccess
    {

        private MySqlConnection _conn;

        public DataAccessMySQL(string connectionString)
        {
            this._conn = new MySqlConnection(connectionString);
        }

        public override DatabaseType DatabaseType
        {
            get { return DatabaseType.MySQL; }
        }

        public override IDbConnection DbConnection
        {
            get { return this._conn; }
        }

        public override int GetMaxID(string ColumnName, string TableName)
        {
            object obj = ExecuteScalar(string.Format("select max(cast({0} as DECIMAL)) + 1 from {1}", ColumnName, TableName));
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return 1;
            }
            else
                return Convert.ToInt32(obj);
        }

        public override int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<int>(count =>
            {
                using (MySqlCommand cmd = new MySqlCommand())
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
                using (MySqlCommand cmd = new MySqlCommand())
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
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, _conn, SQLString, cmdParms);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    return reader;
                }
            }, SQLString, cmdParms);
        }

        public override DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms)
        {
            return this.ExecWithLog<DataSet>(count =>
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, _conn, SQLString, cmdParms);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
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
                using (MySqlCommand command = new MySqlCommand())
                {
                    PrepareCommand(command, _conn, storedProcName, parameters);
                    command.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    return returnReader;
                }
            }, storedProcName, parameters);
        }

        public override DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName)
        {
            return this.ExecWithLog<DataSet>(count =>
            {
                using (MySqlDataAdapter sqlDA = new MySqlDataAdapter())
                {
                    DataSet dataSet = new DataSet();
                    sqlDA.SelectCommand = new MySqlCommand();
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
                using (MySqlCommand command = new MySqlCommand())
                {
                    PrepareCommand(command, _conn, storedProcName, parameters);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new MySqlParameter("ReturnValue",
                       MySqlDbType.Int32, 4, ParameterDirection.ReturnValue,
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
                    MySqlParameter paras = new MySqlParameter(param.Name, param.Value);
                    paras.Direction = param.Direction;
                    paras.Size = param.Size;
                    paras.DbType = param.DbType;
                    cmd.Parameters.Add(paras);
                }
            }
        }
    }
}
