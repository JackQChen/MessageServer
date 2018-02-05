
using System.Collections.Generic;
using System.Reflection;
namespace DataAccess
{
    internal static class ConstructorCache
    {
        internal static Dictionary<string, ConstructorInfo> dic = new Dictionary<string, ConstructorInfo>();

        public static ConstructorInfo AddCache(string key, ConstructorInfo construct)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            dic.Add(key, construct);
            return construct;
        }

        public static ConstructorInfo GetCache(string key)
        {
            if (string.IsNullOrEmpty(key) && !dic.ContainsKey(key))
                return null;
            return dic[key];
        }

        public static bool Exist(string key)
        {
            if (!string.IsNullOrEmpty(key) && dic.ContainsKey(key))
                return true;
            else
                return false;
        }

    }
}
