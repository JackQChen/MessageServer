using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DataAccess;

namespace QueryDataAccess
{
    public class QueryDAO
    {
        public static string ConnectionString { get; set; }
        /// <summary>
        /// 取值范围db2，sqlserver，oledb，oracle
        /// </summary>
        public static string DataType { get; set; }

        private IDataAccess _data;

        public QueryDAO()
        {
            ConnectionList.SetConnection(DataType, ConnectionString);
        }

        public DataTable ExecSQL(string sql)
        {
            return ExecSQL(sql, null);
        }

        public DataTable ExecSQL(string sql, Hashtable htAppend)
        {
            CmdParameterCollection cpc = new CmdParameterCollection();
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                if (htAppend != null)
                {
                    foreach (DictionaryEntry de in htAppend)
                    {
                        CmdParameter cp = new CmdParameter(de.Key.ToString(), de.Value);
                        cpc.Add(cp);
                    }
                }
                _data.Open();
                dt = _data.ExecuteDataSet(sql, cpc).Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public DataTable ExecSQLGetColumn(string sql)
        {
            switch (DataType.Trim().ToLower())
            {
                case "db2":
                    ConnectionList.List["Main"].DatabaseType = DatabaseType.DB2;
                    break;
                case "sqlserver":
                case "oledb":
                    int firstSelect = sql.ToLower().IndexOf("select");
                    sql = sql.Insert(firstSelect + 6, " top 1");
                    break;
                case "oracle":
                    //ConnectionList.List["Main"].DatabaseType = DatabaseType.
                    break;
            }
            return new DataTable();
        }

        public DataTable ExecProc(string procName, Dictionary<string, object> dic)
        {
            CmdParameterCollection cpc = new CmdParameterCollection();
            _data = DataAccessFactory.Instance.CreateDataAccess();
            foreach (KeyValuePair<string, object> de in dic)
                cpc.Add(de.Key, de.Value);
            DataTable dt = _data.RunProcedure(procName, cpc, procName).Tables[0];
            return dt;
        }

        public DataTable GetAllTableList()
        {
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                _data.Open();
                dt = _data.ExecuteDataSet("SELECT * FROM T_TABLE_DICTIONARY").Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public DataTable GetFieldListByTableName(string tableName)
        {
            if (tableName == "CustomTable")
                return null;
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                _data.Open();
                dt = _data.ExecuteDataSet("SELECT * FROM T_FIELD_DICTIONARY WHERE TABLENAME IN ('" + tableName + "')").Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public DataTable GetFieldList()
        {
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                _data.Open();
                dt = _data.ExecuteDataSet("SELECT * FROM T_FIELD_DICTIONARY").Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public DataTable GetMethodList()
        {
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                _data.Open();
                dt = _data.ExecuteDataSet("SELECT * FROM T_METHODS").Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public DataTable GetMethod(string methodID)
        {
            DataTable dt = null;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            try
            {
                _data.Open();
                dt = _data.ExecuteDataSet("SELECT * FROM T_METHODS WHERE MID='" + methodID + "'").Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
            return dt;
        }

        public void InsertMethod(DataTable dt)
        {
            string sql = "INSERT INTO T_METHODS(MID,MNAME,COMMENTARY,DETAIL,VALUETYPE,METHODTYPE) " +
                                "Values(@MID,@MNAME,@COMMENTARY,@DETAIL,@VALUETYPE,@METHODTYPE)";

            CmdParameterCollection para = new CmdParameterCollection();
            para.Add("@MID", dt.Rows[0]["MID"]);
            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);

            _data = DataAccessFactory.Instance.CreateDataAccess();
            _data.Open();

            try
            {
                _data.ExecuteNonQuery(sql, para);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _data.Close();
            }
        }

        public void UpdateMethod(DataTable dt)
        {
            string sql;

            _data = DataAccessFactory.Instance.CreateDataAccess();
            _data.Open();
            var tran = _data.BeginTransaction();
            CmdParameterCollection para = new CmdParameterCollection();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                            sql = "INSERT INTO T_METHODS(MID,MNAME,COMMENTARY,DETAIL,VALUETYPE,METHODTYPE)" +
                                "Values(@MID,@MNAME,@COMMENTARY,@DETAIL,@VALUETYPE,@METHODTYPE)";

                            para.Add("@MID", dt.Rows[0]["MID"]);
                            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
                            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
                            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
                            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
                            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);
                            break;
                        case DataRowState.Modified:
                            sql = "UPDATE T_METHODS SET MNAME=@MNAME,COMMENTARY=@COMMENTARY,DETAIL=@DETAIL,METHODTYPE=@METHODTYPE Where MID=@MID ";

                            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
                            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
                            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
                            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
                            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);
                            para.Add("@MID", dt.Rows[0]["MID"]);
                            break;
                        case DataRowState.Deleted:
                            sql = "Delete From T_METHODS Where MID=@MID ";
                            para.Add("@MID", dt.Rows[0]["MID"]);
                            break;
                        default:
                            continue;
                    }

                    _data.ExecuteNonQuery(sql, para);
                }

                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                _data.Close();
            }
        }

        public void DeleteMethod(DataRow row)
        {
            string sql;

            _data = DataAccessFactory.Instance.CreateDataAccess();
            _data.Open();
            var tran = _data.BeginTransaction();
            CmdParameterCollection para = new CmdParameterCollection();

            try
            {
                sql = "Delete From T_METHODS Where MID=@MID";

                para.Add("@MID", row["MID"]);

                _data.ExecuteNonQuery(sql, para);

                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                _data.Close();
            }
        }

        public DataTable GetProcParams(string procName)
        {
            string sql = string.Empty;
            _data = DataAccessFactory.Instance.CreateDataAccess();
            switch (_data.DatabaseType)
            {
                case DatabaseType.DB2:
                    sql = @"select distinct PARMNAME
                        from SYSCAT.ROUTINEPARMS 
                        WHERE ROUTINENAME = @PROCNAME";
                    break;
                case DatabaseType.MSSQLServer:
                    sql = @"select distinct c.name as PARMNAME
                            from SysColumns c,sysobjects o,systypes b 
                            where c.id = o.id and o.type = 'p' and c.xtype=b.xtype and b.status=0 
                            AND o.name = @PROCNAME ";
                    break;
                //                case DatabaseType.OleDBSupported:
                //                    break;
                //                case DatabaseType.Oracle:
                //                    sql = @"
                //select distinct ARGUMENT_NAME AS PARMNAME
                //from user_arguments
                //WHERE OBJECT_NAME =@PROCNAME and PACKAGE_NAME = @PACKAGENAME";
                //                    break;
            }
            if (string.IsNullOrEmpty(sql))
                return null;

            CmdParameterCollection qpc = new CmdParameterCollection();
            //if (_data.DatabaseType == DatabaseType.Oracle)
            //{
            //    qpc.Add("@PROCNAME", procName.Split('.')[1]);
            //    qpc.Add("@PACKAGENAME", procName.ToUpper().Split('.')[0]);
            //}
            //else
            qpc.Add("@PROCNAME", procName.ToUpper());
            return _data.ExecuteDataSet(sql, qpc).Tables[0];
        }
    }
}
