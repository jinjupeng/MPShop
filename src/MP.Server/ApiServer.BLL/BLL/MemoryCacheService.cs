using ApiServer.BLL.IBLL;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ApiServer.BLL.BLL
{
    /// <summary>
    /// 实例化缓存接口ICaching
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        /// <summary>
        /// 
        /// </summary>
        protected IMemoryCache Cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public MemoryCacheService(IMemoryCache cache)
        {
            Cache = cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        public bool Add(string key, object value, int expirationTime = 20)
        {
            if (!string.IsNullOrEmpty(key))
            {
                /*
                 * MemoryCacheEntryOptions单个缓存项配置
                 * 1. AbsoluteExpiration：绝对过期时间
                 * 2. AbsoluteExpirationRelativeToNow 相对于现在的绝对过期时间
                 * 3. SlidingExpiration 滑动过期时间，在时间段范围内 缓存被再次访问，过期时间将会被重置
                 * 4. Priority 优先级
                 * 5. Size 缓存份数
                 */
                MemoryCacheEntryOptions cacheEntityOps = new MemoryCacheEntryOptions()
                {
                    //滑动过期时间 20秒没有访问则清除
                    SlidingExpiration = TimeSpan.FromSeconds(expirationTime),
                    //设置份数
                    Size = 1,
                    //优先级
                    Priority = CacheItemPriority.Low,
                };
                //过期回掉
                cacheEntityOps.RegisterPostEvictionCallback((keyInfo, valueInfo, reason, state) =>
                {
                    Console.WriteLine($"回调函数输出【键:{keyInfo},值:{valueInfo},被清除的原因:{reason}】");
                });
                Cache.Set(key, value, cacheEntityOps);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (Exists(key))
            {
                Cache.Remove(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (Exists(key))
            {
                return Cache.Get(key);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            object cache;
            return Cache.TryGetValue(key, out cache);
        }

    }
}
