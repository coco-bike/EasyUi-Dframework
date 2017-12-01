using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
public  class CacheHelper
    {
        public static bool Set(string key, object value, DateTime dt)
        {
            return Set(key, value, null, dt, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        public static object Get(string key)
        {
            return GetPrivate(key);
        }

        /// <summary>
        /// 判断缓存中是否含有缓存该键
        /// </summary>
        public static bool Exists(string key)
        {
            return (GetPrivate(key) != null);
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            HttpRuntime.Cache.Remove(key);
            return true;
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns></returns>
        public static bool RemoveAll()
        {
            IDictionaryEnumerator iDictionaryEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (iDictionaryEnumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(Convert.ToString(iDictionaryEnumerator.Key));
            }
            return true;
        }



        //============方法封装=====================

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="cacheDependency"></param>
        /// <param name="dateTime">过期时间</param>
        /// <param name="timeSpan">timeSpan必须为Cache.NoSlidingExpiration否则抛异常</param>
        /// <param name="cacheItemPriority"></param>
        /// <param name="cacheItemRemovedCallback"></param>
        /// <returns></returns>
        public static bool Set(string key, object value, CacheDependency cacheDependency, DateTime dateTime,
    TimeSpan timeSpan, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return false;
            }
            HttpRuntime.Cache.Add(key, value, cacheDependency, dateTime, timeSpan, cacheItemPriority, cacheItemRemovedCallback);
            return true;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        private static object GetPrivate(string key)
        {
            return string.IsNullOrEmpty(key) ? null : HttpRuntime.Cache.Get(key);
        }
    }
}
