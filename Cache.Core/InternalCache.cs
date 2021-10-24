using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core
{
    public class InternalCache : ICache
    {
        public string GetValue(string key)
        {
            CheckKey(key);
            return Cache.Get<string>(key);
        }

        public T GetValue<T>(string key)
        {
            CheckKey(key);
            return Cache.Get<T>(key);
        }

        public bool KeyExists(string key)
        {
            CheckKey(key);
            return Cache.TryGetValue(key, out _);
        }

        public bool KeyExpiry(string key, int expiry = -1)
        {
            CheckKey(key);
            return true;
        }

        public bool RemoveKey(string key)
        {
            CheckKey(key);
            Cache.Remove(key);

            return !KeyExists(key);
        }

        public bool SetValue(string key, string value, int expiry = -1)
        {
            CheckKey(key);
            CheckKey(value);
            if (expiry == -1)
                return Cache.Set(key, value) != null;
            return Cache.Set(key, value, new TimeSpan(0, 0, expiry)) != null;
        }

        public bool SetValue<T>(string key, T value, int expiry = -1)
        {
            CheckKey(key);
            if (value == null)
                throw new ArgumentNullException("value is null!");
            if (expiry == -1)
                return Cache.Set(key, value) != null;
            return Cache.Set(key, value, new TimeSpan(0, 0, expiry)) != null;
        }

        private void CheckKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("The key is null or empty!");
        }

        public InternalCache(MemoryCache _cache)
        {
            Cache = _cache;
        }

        private MemoryCache Cache { get; set; }
    }
}
