using System;
using System.Collections.Concurrent;

namespace MessageServer
{
    public class ListExtra<T>
    {
        internal ConcurrentDictionary<string, T> dict = new ConcurrentDictionary<string, T>();
        internal bool isChanged;
        /// <summary>
        /// 获取附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get(string key)
        {
            T value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            return default(T);
        }

        /// <summary>
        /// 设置附加数据
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(string key, T newValue)
        {
            isChanged = true;
            try
            {
                dict.AddOrUpdate(key, newValue, (tKey, existingVal) => { return newValue; });
                return true;
            }
            catch (OverflowException)
            {
                // 字典数目超过int.max
                return false;
            }
            catch (ArgumentNullException)
            {
                // 参数为空
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            isChanged = true;
            T value;
            return dict.TryRemove(key, out value);
        }

    }
}
