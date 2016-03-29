using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Core
{
    public sealed class Cache<T>
    {
        private Hashtable cache ;

        public Cache()
        {
            this.cache = new Hashtable();
        }

        public void Add(string name, T value)
        {
            cache.Add(name, value);
        }

        public T Get(string name)
        {
            if (!this.Exists(name))
            {
                throw new Exception(String.Format("Could not locate cache item by name:", name));
            }

            return (T)this.cache[name];
        }

        public bool Exists(string name)
        {
            return this.cache.ContainsKey(name);
        }

        public IEnumerable<T> GetAll()
        {
            foreach (var item in this.cache.Values)
            {
                yield return (T)item;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.cache.Count == 0;
            }
        }
    }
}
