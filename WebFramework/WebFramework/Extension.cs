using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace WebFramework
{
    public static class Extension
    {
        /// <summary>
        /// Object转换为Json字符串
        /// </summary>
        /// <param name="objTarget"></param>
        /// <returns></returns>
        public static string ToJsonString(this object objTarget)
        {
            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
            jsonConvert.MaxJsonLength = Int32.MaxValue;
            return jsonConvert.Serialize(objTarget);
        }

        /// <summary>
        /// Json字符串转Object
        /// </summary>
        /// <param name="strTarget"></param>
        /// <returns></returns>
        public static object ToJsonObject(this string jsonString)
        {
            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
            return jsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// DataTable转匿名对象
        /// </summary>
        public static object ToJsonObject(this DataTable dt)
        {
            ArrayList list = new ArrayList();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn clm in dt.Columns)
                    dic.Add(clm.ColumnName, row[clm.ColumnName]);
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// Json对象获取节点对象
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public static T ToJsonProperty<T>(this object jsonObject, string strPara) where T : class
        {
            if (string.IsNullOrEmpty(strPara))
                return jsonObject as T;
            string[] strArr = strPara.Split('.');
            var objTemp = jsonObject;
            foreach (string str in strArr)
            {
                if (objTemp is IDictionary)
                {
                    var dic = objTemp as Dictionary<string, object>;
                    Match match = Regex.Match(str, "(\\[)(.+?)(\\])", RegexOptions.Singleline);
                    //数组方式
                    if (match.Success)
                    {
                        string strKey = str.Replace(match.Value, "");
                        if (dic.ContainsKey(strKey))
                        {
                            IList list = dic[strKey] as IList;
                            if (list == null)
                                return null;
                            string strIndex = match.Value.Replace("[", "").Replace("]", "");
                            int iOut = 0;
                            if (Int32.TryParse(strIndex, out iOut))
                            {
                                if (iOut < 0 || iOut > list.Count - 1)
                                    return null;
                                else
                                    objTemp = list[iOut];
                            }
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    else
                    {
                        if (dic.ContainsKey(str))
                            objTemp = dic[str];
                        else
                            return null;
                    }
                }
                else
                    return null;
            }
            return objTemp as T;
        }

        /// <summary>
        /// Json对象获取属性
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="strPara"></param>
        /// <returns></returns>
        public static string ToJsonProperty(this object jsonObject, string strPara)
        {
            object obj = ToJsonProperty<object>(jsonObject, strPara);
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

    }
}
