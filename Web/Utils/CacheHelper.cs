/*----------------------------------------------------------------
// 创 建 人:杨瑞卿
// 创建时间:2016/8/23 17:59:24
// 描述信息:
//----------------------------------------------------------------*/

using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Web.Utils
{
    public class CacheHelper
    {
        /// <summary>
        /// 获取当前应用程序指定键值的Cache值
        /// </summary>
        /// <param name="strKey">键值</param>
        /// <returns>缓存对象</returns>
        public static object GetCache(string strKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[strKey];
        }

        /// <summary>
        /// 设置当前应用程序指定键值的Cache值
        /// </summary>
        /// <param name="strKey">键值</param>
        /// <param name="obj">对象值</param>
        public static void SetCache(string strKey, object obj)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(strKey, obj);
        }

        /// <summary>
        /// 设置当前应用程序指定键值的Cache值
        /// </summary>
        /// <param name="strKey">键值</param>
        /// <param name="obj">对象值</param>
        /// <param name="absoluteExpiration">时间表达式</param>
        /// <param name="slidingExpiration">时间戳</param>
        public static void SetCache(string strKey, object obj, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(strKey, obj, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 创建缓存项的文件依赖
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public static void Insert(string key, object obj, string fileName)
        {
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(fileName);
            //创建缓存
            HttpContext.Current.Cache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 创建缓存项过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void Insert(string key, object obj, int expires)
        {
            HttpContext.Current.Cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
        }


        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }



        /// <summary>
        /// 移除(删除)缓存
        /// </summary>
        /// <param name="cacheKey">键值</param>
        public static void DeleteCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        /// <summary>
        /// 判断当前缓存是否已存在
        /// </summary>
        /// <param name="strKey">缓存名称</param>
        /// <returns></returns>
        public static bool IsExist(string strKey)
        {
            return HttpContext.Current.Cache[strKey] != null;
        }

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        public static void RemoveAllCache(string CacheKey)
        {
            Cache cache = HttpRuntime.Cache;
            cache.Remove(CacheKey);
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                cache.Remove(CacheEnum.Key.ToString());
            }
        }

    }
}