
using System;
using System.Data;
namespace DataAccess
{
    public interface IDataAccess
    {
        #region 属性及方法

        DatabaseType DatabaseType { get; }

        IDbConnection DbConnection { get; }

        IDbTransaction Trans { get; set; }

        void Open();

        void Close();

        IDbTransaction BeginTransaction();

        #endregion

        #region 公用方法

        bool Exists(string strSql);

        bool Exists(string strSql, CmdParameterCollection cmdParms);

        int GetMaxID(string ColumnName, string TableName);

        #endregion

        #region  执行简单SQL语句

        int ExecuteNonQuery(string SQLString);

        object ExecuteScalar(string SQLString);

        IDataReader ExecuteReader(string strSQL);

        DataSet ExecuteDataSet(string SQLString);

        #endregion

        #region 执行带参数的SQL语句

        int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms);

        object ExecuteScalar(string SQLString, CmdParameterCollection cmdParms);

        IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms);

        DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms);

        #endregion

        #region 存储过程操作

        IDataReader RunProcedure(string storedProcName, CmdParameterCollection parameters);

        DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName);

        int RunProcedure(string storedProcName, CmdParameterCollection parameters, out int rowsAffected);

        #endregion

        void PrepareCommand(IDbCommand cmd, IDbConnection conn, string cmdText, CmdParameterCollection cmdParms);

        void TryExec(Action<object> act);
        T TryExec<T>(Func<object, T> func);

        void Transaction(Action<object> act);
        T Transaction<T>(Func<object, T> func);
    }
}
