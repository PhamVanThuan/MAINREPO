using System.Collections.Generic;

namespace SAHL.Core.Caching
{
    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string, object> cacheDictionary = new Dictionary<string, object>();

        public void AddItem<T>(string key, T value)
        {
            this.cacheDictionary.Add(key, value);
        }

        public void Clear()
        {
            this.cacheDictionary.Clear();
        }

        public bool Contains(string key)
        {
            return this.cacheDictionary.ContainsKey(key);
        }

        public T GetItem<T>(string key)
        {
            object cacheValue;
            var gotItem = this.cacheDictionary.TryGetValue(key, out cacheValue);
            if (gotItem)
            {
                return (T) cacheValue;
            }
            return default(T);
        }

        public void RemoveItem(string key)
        {
            this.cacheDictionary.Remove(key);
        }

        public void SetItem<T>(string key, T value)
        {
            this.cacheDictionary[key] = value;
        }

        public void AddOrSetItem<T>(string key, T value)
        {
            if (Contains(key))
            {
                SetItem(key, value);
            }
            else
            {
                AddItem(key, value);
            }
        }
    }
}