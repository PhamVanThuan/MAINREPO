using System;

namespace SAHL.Core.Caching
{
    public class CacheKeyByName<T> : INamedCacheKey
    {
        public CacheKeyByName(string context, string name)
        {
            this.Context = context;
            this.Name = name;
        }

        public Type CacheItemType { get { return typeof(T); } }

        public string Name { get; protected set; }

        public string Context { get; protected set; }
    }
}