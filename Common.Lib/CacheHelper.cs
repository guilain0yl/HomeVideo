using Cache.Core;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Lib
{
    public class CacheHelper
    {
        /// <summary>
        /// 会话缓存
        /// </summary>
        public static readonly ICache SessionCache = new InternalCache(new MemoryCache(new MemoryCacheOptions()));
    }
}
