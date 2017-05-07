using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace CommonUtils
{
    /// <summary>
    /// Cache操作扩展
    /// </summary>
    public abstract class CacheHelper
    {
        private static readonly Cache _cache;
        static CacheHelper()
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                _cache = current.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }
        /// <summary>
        /// 简单创建/修改Cache，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <param name="strValue">Cache值</param>
        /// <param name="iExpires">有效期，分钟数（使用的是当前时间+分钟数得到一个绝对到期值）</param>
        /// <param name="priority">保留优先级，1最不会被清除，6最容易被内存管理清除（1:NotRemovable；2:High；3:AboveNormal；4:Normal；5:BelowNormal；6:Low）</param>
        public static void Insert(String strCacheName, Object strValue, int iExpires, int priority)
        {
            TimeSpan ts = new TimeSpan(0, iExpires, 0);
            CacheItemPriority cachePriority;
            switch (priority)
            {
                case 6:
                    cachePriority = CacheItemPriority.Low;
                    break;
                case 5:
                    cachePriority = CacheItemPriority.BelowNormal;
                    break;
                case 4:
                    cachePriority = CacheItemPriority.Normal;
                    break;
                case 3:
                    cachePriority = CacheItemPriority.AboveNormal;
                    break;
                case 2:
                    cachePriority = CacheItemPriority.High;
                    break;
                case 1:
                    cachePriority = CacheItemPriority.NotRemovable;
                    break;
                default:
                    cachePriority = CacheItemPriority.Default;
                    break;
            }
            _cache.Insert(strCacheName, strValue, null, DateTime.Now.Add(ts), System.Web.Caching.Cache.NoSlidingExpiration, cachePriority, null);
        }
        /// <summary>
        /// 简单读取Cache对象的值，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <returns>Cache字符串值</returns>
        public static Object GetCache(String strCacheName)
        {
            return _cache[strCacheName];
        }
        /// <summary>
        /// 删除Cache对象
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        public static void Delete(String strCacheName)
        {
            _cache.Remove(strCacheName);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        //public static void Clear()
        //{
        //    IDictionaryEnumerator enumerator = _cache.GetEnumerator();
        //    ArrayList list = new ArrayList();
        //    while (enumerator.MoveNext())
        //    {
        //        list.Add(enumerator.Key);
        //    }
        //    foreach (String str in list)
        //    {
        //        _cache.Remove(str);
        //    }
        //}
    }
}
