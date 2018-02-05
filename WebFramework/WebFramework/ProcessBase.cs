using System.Web;
using System.Web.SessionState;

namespace WebFramework
{
    public abstract class ProcessBase
    {
        public HttpContext Context { get; set; }

        public HttpRequest Request { get { return this.Context.Request; } }
        public HttpResponse Response { get { return this.Context.Response; } }
        public HttpSessionState Session { get { return this.Context.Session; } }

        public ProcessBase()
        {
        }

        public virtual string OnLoad()
        {
            return "";
        }

        #region 公共方法

        public string GetSession(string key)
        {
            object obj = this.Session[key];
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        #endregion

    }
}
