using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace WebFramework
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        static InvokeCache _instance;

        static InvokeCache instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InvokeCache();
                return _instance;
            }
        }

        //invoke:Type,Dll|Method类型格式
        //param:object[]类型格式
        public void ProcessRequest(HttpContext context)
        {
            string strReturn = "", strRequest = "";
            try
            {
                strRequest = HttpUtility.UrlDecode(context.Request.Form.ToString());
                object objPara = strRequest.ToJsonObject();
                string strInvoke = objPara.ToJsonProperty("invoke");
                string strType = strInvoke.Split('|')[0],
                    strMethod = strInvoke.Split('|')[1];
                if (!instance.dicConstructor.ContainsKey(strType))
                {
                    Type tp = Type.GetType(strType);
                    instance.dicConstructor.TryAdd(strType, tp.GetConstructor(new Type[] { }));
                }
                if (!instance.dicMethod.ContainsKey(strInvoke))
                {
                    instance.dicMethod.TryAdd(strInvoke, instance.dicConstructor[strType].ReflectedType.GetMethod(strMethod));
                }
                ProcessBase process = instance.dicConstructor[strType].Invoke(null) as ProcessBase;
                process.Context = context;
                strReturn = process.OnLoad();
                if (strReturn == "")
                    strReturn = instance.dicMethod[strInvoke].Invoke(process, objPara.ToJsonProperty<object[]>("param")).ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(strRequest, ex);
                strReturn = new
                {
                    action = "alert('操作出现异常，请联系管理员');"
                }.ToJsonString();
            }
            context.Response.Write(strReturn);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class InvokeCache
    {
        public ConcurrentDictionary<string, ConstructorInfo> dicConstructor;
        public ConcurrentDictionary<string, MethodInfo> dicMethod;

        public InvokeCache()
        {
            this.dicConstructor = new ConcurrentDictionary<string, ConstructorInfo>();
            this.dicMethod = new ConcurrentDictionary<string, MethodInfo>();
        }
    }
}
