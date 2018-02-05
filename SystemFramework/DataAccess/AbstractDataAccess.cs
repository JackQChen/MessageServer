
using System;
using System.Data;
using System.Diagnostics;
using SystemFramework;
using System.Text;

namespace DataAccess
{
    public abstract class AbstractDataAccess : IDataAccess
    {

        #region IDataAccess 成员

        /// <summary>
        /// 数据库类型
        /// </summary>
        public abstract DatabaseType DatabaseType { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public abstract IDbConnection DbConnection { get; }

        /// <summary>
        /// 连接的事务
        /// </summary>
        public IDbTransaction Trans { get; set; }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            if (this.DbConnection.State == ConnectionState.Closed)
                DbConnection.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (Trans == null)
                this.DbConnection.Close();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            if (this.DbConnection != null)
            {
                this.Trans = this.DbConnection.BeginTransaction();
                return this.Trans;
            }
            else
                return null;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <returns></returns>
        public bool Exists(string strSql)
        {
            return Exists(strSql, null);
        }

        /// <summary>
        /// 判断是否存在(使用参数)
        /// </summary>
        /// <param name="strSql">查询语句(包含参数)</param>
        /// <param name="cmdParms">查询参数</param>
        /// <returns></returns>
        public bool Exists(string strSql, CmdParameterCollection cmdParms)
        {
            object obj = ExecuteScalar(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = Convert.ToInt32(obj);
            }
            return cmdresult == 0 ? false : true;
        }


        /// <summary>
        /// 获取最大ID
        /// </summary>
        /// <param name="ColumnName">列名称</param>
        /// <param name="TableName">表名称</param>
        /// <returns>最大号</returns>
        public virtual int GetMaxID(string ColumnName, string TableName)
        {
            object obj = ExecuteScalar(string.Format("select max(cast({0} as int)) + 1 from {1}", ColumnName, TableName));
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return 1;
            }
            else
                return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="SQLString">执行语句</param>
        /// <returns>受影响行数</returns>
        public int ExecuteNonQuery(string SQLString)
        {
            return ExecuteNonQuery(SQLString, null);
        }

        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>结果</returns>
        public object ExecuteScalar(string SQLString)
        {
            return ExecuteScalar(SQLString, null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>Reader</returns>
        public IDataReader ExecuteReader(string strSQL)
        {
            return ExecuteReader(strSQL, null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string SQLString)
        {
            return ExecuteDataSet(SQLString, null);
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="SQLString">执行语句</param>
        /// <param name="cmdParms">Command参数</param>
        /// <returns>受影响行数</returns>
        public abstract int ExecuteNonQuery(string SQLString, CmdParameterCollection cmdParms);

        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="cmdParms">Command参数</param>
        /// <returns>结果</returns>
        public abstract object ExecuteScalar(string SQLString, CmdParameterCollection cmdParms);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="cmdParms">Command参数</param>
        /// <returns>Reader</returns>
        public abstract IDataReader ExecuteReader(string SQLString, CmdParameterCollection cmdParms);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <param name="cmdParms">Command参数</param>
        /// <returns>DataSet</returns>
        public abstract DataSet ExecuteDataSet(string SQLString, CmdParameterCollection cmdParms);

        /// <summary>
        /// 执行存储过程 注意：调用该方法后，要对DataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataReader</returns>
        public abstract IDataReader RunProcedure(string storedProcName, CmdParameterCollection parameters);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public abstract DataSet RunProcedure(string storedProcName, CmdParameterCollection parameters, string tableName);

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns>影响的行数</returns>
        public abstract int RunProcedure(string storedProcName, CmdParameterCollection parameters, out int rowsAffected);

        /// <summary>
        /// 准备Command
        /// </summary>
        /// <param name="cmd">Command</param>
        /// <param name="conn">Command连接</param> 
        /// <param name="cmdType">Command类型</param> 
        /// <param name="cmdText">Command语句</param>
        /// <param name="cmdParms">Command参数</param>
        public abstract void PrepareCommand(IDbCommand cmd, IDbConnection conn, string cmdText, CmdParameterCollection cmdParms);

        #endregion

        public T ExecWithLog<T>(Func<Action<string>, T> funcExec, string strSQL, CmdParameterCollection cmdParms)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string strLog = @"{0}
DBType:{1}
SQL:
{2}
Params:{3}
{4}
Elapsed:{5}", strInfo = "Count:1";
            T objRst = default(T);
            try
            {
                objRst = funcExec(uCount =>
                {
                    strInfo = "Count:" + uCount;
                });
            }
            catch (Exception ex)
            {
                strInfo = "Error:" + ex.Message;
                throw ex;
            }
            finally
            {
                sw.Stop();
                LogService.DataAccessMessage(string.Format(strLog,
                    "".PadLeft(100, '*'),
                    this.DatabaseType,
                    strSQL,
                    this.GetParaLog(cmdParms),
                    strInfo,
                    sw.Elapsed));
            }
            return objRst;
        }

        private string GetParaLog(CmdParameterCollection cmdParms)
        {
            StringBuilder strLog = new StringBuilder();
            if (cmdParms == null)
                strLog.Append("Null");
            else
                foreach (CmdParameter para in cmdParms)
                    strLog.Append(string.Format("\r\n{0}={1}", para.Name, para.Value));
            return strLog.ToString();
        }

        public void TryExec(Action<object> act)
        {
            this.TryExec<object>(func =>
            {
                act(null);
                return null;
            });
        }

        public T TryExec<T>(Func<object, T> func)
        {
            this.Open();
            try
            {
                return func(null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// 没有返回值的事务
        /// </summary>
        /// <param name="act"></param>
        public void Transaction(Action<object> act)
        {
            this.Transaction<object>(func =>
            {
                act(null);
                return null;
            });
        }

        /// <summary>
        /// 有返回值的事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Transaction<T>(Func<object, T> func)
        {
            if (Trans != null)
            {
                try
                {
                    return func(null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Open();
                Trans = DbConnection.BeginTransaction();
                try
                {
                    T rst = default(T);
                    rst = func(null);
                    Trans.Commit();
                    return rst;
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw ex;
                }
                finally
                {
                    Trans = null;
                    Close();
                }
            }
        }

    }
}
