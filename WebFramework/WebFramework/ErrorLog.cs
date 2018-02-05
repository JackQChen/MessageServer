using System;
using System.Text;
namespace WebFramework
{
    public class ErrorLog
    {
        public ErrorLog()
        {
        }

        static void getAllInnerException(int deep, StringBuilder strInfo, Exception ex)
        {
            if (ex.InnerException != null)
            {
                strInfo.Append(string.Format(@"
InnerException{0}:
{1}
StackTrace:
{2}",
    deep == 1 ? "" : deep.ToString(),
    ex.InnerException.Message,
    ex.InnerException.StackTrace));
                deep++;
                getAllInnerException(deep, strInfo, ex.InnerException);
            }
        }

        public static void WriteLog(string strPara, Exception ex)
        {
            int deep = 1;
            StringBuilder strInner = new StringBuilder();
            getAllInnerException(deep, strInner, ex);
            LogService.Error(
                string.Format(@"{0}
Params:
{1}
Exception:
{2}
StackTrace:
{3}{4}",
                "".PadRight(100, '*'),
                strPara,
                ex.Message,
                ex.StackTrace,
                strInner.ToString()));
        }
    }
}