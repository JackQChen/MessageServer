using System;
using System.Data;
using Skynet.Data;

namespace QueryDesigner
{
    public class QueryDAO
    {
        private IDataAccess _data;
        public QueryDAO()
        {
            _data = DataAccessFactory.instance.CreateDataAccess();
        }

        public DataTable ExecSQL(string sql)
        {
            return _data.ExecuteDataset(sql).Tables[0];
        }

        public DataTable GetAllTableList()
        {
            return _data.ExecuteDataset("SELECT * FROM T_TABLE_DICTIONARY").Tables[0];
        }

        public DataTable GetFieldListByTableName(string tableName, bool isOne)
        {
            if (isOne)
            {
                return _data.ExecuteDataset("SELECT * FROM T_FIELD_DICTIONARY WHERE TABLENAME='" + tableName + "'").Tables[0];
            }

            return _data.ExecuteDataset("SELECT * FROM T_FIELD_DICTIONARY WHERE TABLENAME IN " + tableName).Tables[0];
        }

        public DataTable GetFieldList()
        {
            return _data.ExecuteDataset("SELECT * FROM T_FIELD_DICTIONARY").Tables[0];
        }

        public DataTable GetMethodList()
        {
            return _data.ExecuteDataset("SELECT * FROM T_METHODS").Tables[0];
        }

        public DataTable GetMethod(string methodID)
        {
            return _data.ExecuteDataset("SELECT * FROM T_METHODS WHERE MID='" + methodID + "'").Tables[0];
        }

        public void InsertMethod(DataTable dt)
        {
            string sql = "INSERT INTO T_METHODS(MID,MNAME,COMMENTARY,DETAIL,VALUETYPE,METHODTYPE) " +
                                "Values(@MID,@MNAME,@COMMENTARY,@DETAIL,@VALUETYPE,@METHODTYPE)";

            QueryParameterCollection para = new QueryParameterCollection();
            para.Add("@MID", dt.Rows[0]["MID"]);
            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);

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

            _data.Open();
            SingleTransaction tran = new SingleTransaction(_data);
            tran.Begin();
            QueryParameterCollection para = new QueryParameterCollection();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                            sql = "INSERT INTO T_METHODS(MID,MNAME,COMMENTARY,DETAIL,VALUETYPE,METHODTYPE) " +
                                "Values(@MID,@MNAME,@COMMENTARY,@DETAIL,@VALUETYPE,@METHODTYPE)";
                           
                            para.Add("@MID", dt.Rows[0]["MID"]);
                            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
                            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
                            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
                            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
                            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);
                            break;
                        case DataRowState.Modified:
                            sql = "UPDATE T_METHODS SET MNAME=@MNAME,COMMENTARY=@COMMENTARY,DETAIL=@DETAIL,METHODTYPE=@METHODTYPE " +
                                         "Where MID=@MID";

                            para.Add("@MNAME", dt.Rows[0]["MNAME"]);
                            para.Add("@COMMENTARY", dt.Rows[0]["COMMENTARY"]);
                            para.Add("@DETAIL", dt.Rows[0]["DETAIL"]);
                            para.Add("@VALUETYPE", dt.Rows[0]["VALUETYPE"]);
                            para.Add("@METHODTYPE", dt.Rows[0]["METHODTYPE"]);
                            para.Add("@MID", dt.Rows[0]["MID"]);
                            break;
                        case DataRowState.Deleted:
                            sql = "Delete From T_METHODS Where MID=@MID";

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
    }
}
