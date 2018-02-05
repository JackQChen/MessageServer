using System;
using System.Collections;
using System.Data;
using QueryDataAccess;

namespace QueryDesigner
{
    [Serializable]
    public class Method
    {
        private QueryDAO _dao;

        private string _operatorId = "admin";
        private string _operatorName = "管理员";
        private string _operatorOffice = "信息科";


        public Method(QueryDAO dao)
        {
            _dao = dao;
        }

        public void ExecMethod(string methodName, out string valueType, out object returnValue)
        {
            DataTable dtMethod = _dao.GetMethod(methodName);
            if (dtMethod.Rows.Count == 0)
            {
                valueType = "";
                returnValue = "";
                return;
            }
            DataRow row = dtMethod.Rows[0];
            valueType = row["VALUETYPE"].ToString();
            returnValue = "";
            switch (valueType)
            {
                case "字符":
                case "日期":
                    if (row["METHODTYPE"].ToString() == "SYSTEM")
                    {
                        switch (methodName)
                        {
                            case "GetOperatorID": returnValue = _operatorId; break;
                            case "GetOperator": returnValue = _operatorName; break;
                            case "GetOprOffice": returnValue = _operatorOffice; break;
                            case "Now": returnValue = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); break;
                            case "Date": returnValue = System.DateTime.Today.ToShortDateString(); break;
                        }
                    }
                    else
                    {
                        DataTable dt;
                        try
                        {
                            dt = _dao.ExecSQL(row["DETAIL"].ToString());
                        }
                        catch
                        {
                            dt = null;
                        }
                        if (dt != null)
                            if (dt.Rows.Count > 0)
                                returnValue = dt.Rows[0][0].ToString();
                    }
                    break;
                case "数据集":
                    try
                    {
                        returnValue = _dao.ExecSQL(row["DETAIL"].ToString());
                    }
                    catch
                    {
                        returnValue = null;
                    }
                    break;
            }
        }
    }
}
