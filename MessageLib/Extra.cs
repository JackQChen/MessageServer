using System;
using System.Collections.Concurrent;

namespace MessageLib
{
    public class Extra<TKey, TValue>
    {
        public bool Changed { get; set; }
        public ConcurrentDictionary<TKey, TValue> Dictionary
        {
            get
            {
                return this.dict;
            }
        }

        ConcurrentDictionary<TKey, TValue> dict = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// 获取附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }

        /// <summary>
        /// 设置附加数据
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set(TKey key, TValue newValue)
        {
            try
            {
                dict.AddOrUpdate(key, newValue, (tKey, existingVal) => { return newValue; });
                this.Changed = true;
                return true;
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
        public bool Remove(TKey key)
        {
            TValue value;
            bool result = dict.TryRemove(key, out value);
            this.Changed = true;
            return result;
        }
    }

    public class ConnectionExtra : Extra<IntPtr, object>
    {

        /// <summary>
        /// 获取附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetExtra(IntPtr key)
        {
            return this.Get(key);
        }

        /// <summary>
        /// 获取附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetExtra<T>(IntPtr key)
        {
            object value = this.Get(key);
            if (value == null)
                return default(T);
            else
                return (T)value;
        }

        /// <summary>
        /// 设置附加数据
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetExtra(IntPtr key, object newValue)
        {
            return this.Set(key, newValue);
        }

        /// <summary>
        /// 删除附加数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveExtra(IntPtr key)
        {
            return this.Remove(key);
        }
    }
}
