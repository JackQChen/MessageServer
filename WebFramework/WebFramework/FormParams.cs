using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebFramework
{
    public class FormParams
    {
        public static Dictionary<string, List<string>> GetFormParams(string strPara)
        {
            Dictionary<string, List<string>> dicPara = new Dictionary<string, List<string>>();
            string strKey = "";
            foreach (string strRow in strPara.Split('&'))
            {
                strKey = strRow.Split('=')[0];
                if (!dicPara.ContainsKey(strKey))
                {
                    List<string> list = new List<string>();
                    dicPara.Add(strKey, list);
                }
                dicPara[strKey].Add(strRow.Split('=')[1]);
            }
            return dicPara;
        }

        static void setEntityVal(PropertyInfo p, string strType, object entity, Dictionary<string, List<string>> dicPara, string separator)
        {
            try
            {
                switch (strType)
                {
                    case "int32":
                        p.SetValue(entity, Convert.ToInt32(dicPara[p.Name][0]), null);
                        break;
                    case "decimal":
                        p.SetValue(entity, Convert.ToDecimal(dicPara[p.Name][0]), null);
                        break;
                    case "datetime":
                        p.SetValue(entity, Convert.ToDateTime(dicPara[p.Name][0]), null);
                        break;
                    default:
                        p.SetValue(entity, string.Join(separator, dicPara[p.Name].ToArray()), null);
                        break;
                }
            }
            catch
            {
                throw new Exception(string.Format("类型为[{0}]的属性[{1}]赋值失败！", strType, p.Name));
            }
        }

        public static void UpdateEntityByFormParams(object entity, Dictionary<string, List<string>> dicPara)
        {
            UpdateEntityByFormParams(entity, dicPara, ",");
        }

        public static void UpdateEntityByFormParams(object entity, Dictionary<string, List<string>> dicPara, string separator)
        {
            var pList = new List<PropertyInfo>(entity.GetType().GetProperties());
            foreach (string key in dicPara.Keys)
            {
                var p = pList.Find(m => m.Name == key);
                if (p == null)
                    continue;
                if (p.PropertyType.IsGenericType)
                {
                    var tp = p.PropertyType.GetGenericArguments()[0];
                    setEntityVal(p, tp.Name.ToLower(), entity, dicPara, separator);
                }
                else
                {
                    setEntityVal(p, p.PropertyType.Name.ToLower(), entity, dicPara, separator);
                }
            }
        }
    }
}
