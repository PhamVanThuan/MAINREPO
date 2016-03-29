using System;

namespace SAHL.Core.Caching
{
    public class CacheKeyById<T> : IKeyedCacheKey
    {
        public CacheKeyById(string context, int key)
        {
            this.Context = context;
            this.Key = key;
        }

        public int Key { get; protected set; }

        public Type CacheItemType { get { return typeof(T); } }

        public string Context { get; protected set; }
    }
}