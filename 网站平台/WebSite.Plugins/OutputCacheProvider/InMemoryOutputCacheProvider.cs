using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace WebSite.Plugins
{
    public class InMemoryOutputCacheProvider : OutputCacheProvider
    {
        #region Members
        private Dictionary<string, InMemoryOutputCacheItem> _cache = new Dictionary<string, InMemoryOutputCacheItem>();
        private readonly static object _syncLock = new object();
        #endregion

        #region Methods
        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            Set(key, entry, utcExpiry);
            return entry;
        }

        public override object Get(string key)
        {
            InMemoryOutputCacheItem item = null;
            if (_cache.TryGetValue(key, out item))
            {
                if (item.UtcExpiry < DateTime.UtcNow)
                {
                    Remove(key);
                    return null;
                }
                return item.Value;
            }
            return null;
        }

        public override void Remove(string key)
        {
            InMemoryOutputCacheItem item = null;
            if (_cache.TryGetValue(key, out item))
            {
                _cache.Remove(key);
            }
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            var item = new InMemoryOutputCacheItem(entry, utcExpiry);
            lock (_syncLock)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache[key] = item;
                }
                else
                {
                    _cache.Add(key, item);
                }
            }
        }

        #endregion

    }
}
