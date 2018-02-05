using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess;

namespace QueryDataAccess
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
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, this.DbConnection, SQLString, cmdParms);
                return cmd.ExecuteNonQuery();
            }
        }

        public override object ExecuteScalar(string SQLString, CmdParameterCollection cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, _conn, SQLString, cmdParms);
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    return null;
                else
                    return obj;
            }
        }

        public override IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, _conn, SQLString, cmdParms);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
        }

        public override DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                PrepareCommand(cmd, _conn, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "DataSet");
                    return ds;
                }
            }
        }

        public override IDataReader RunProcedure(string storedProcName, CmdParameterCollection parameters)
        {
            using (SqlCommand command = new SqlCommand())
            {
                PrepareCommand(command, _conn, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return returnReader;
            }
        }

        public override DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName)
        {
            using (SqlDataAdapter sqlDA = new SqlDataAdapter())
            {
                DataSet dataSet = new DataSet();
                sqlDA.SelectCommand = new SqlCommand();
                PrepareCommand(sqlDA.SelectCommand, _conn, storedProcName, parameters);
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.Fill(dataSet, tableName);
                return dataSet;
            }
        }

        public override int RunProcedure(string storedProcName, CmdParameterCollection parameters, out int rowsAffected)
        {
            using (SqlCommand command = new SqlCommand())
            {
                PrepareCommand(command, _conn, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("ReturnValue",
                    SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                    false, 0, 0, string.Empty, DataRowVersion.Default, null));
                int result;
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                return result;
            }
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
