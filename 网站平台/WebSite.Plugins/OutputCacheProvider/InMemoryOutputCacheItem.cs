using System;

namespace WebSite.Plugins
{
    public class InMemoryOutputCacheItem
    {
        #region Members

        public DateTime UtcExpiry { get; set; }
        public object Value { get; set; }

        #endregion

        #region Ctor

        public InMemoryOutputCacheItem(object value, DateTime utcExpiry)
        {
            Value = value;
            UtcExpiry = utcExpiry;
        }

        #endregion
    }

}
