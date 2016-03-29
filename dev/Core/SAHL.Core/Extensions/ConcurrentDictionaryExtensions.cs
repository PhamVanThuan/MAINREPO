using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SAHL.Core.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static TValue TryGetValueIfNotPresentThenAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key,
            Func<TKey, TValue> methodToCallToCreateIfNotPresent)
        {
            bool wasAdded;
            return TryGetValueIfNotPresentThenAdd(dictionary, key, methodToCallToCreateIfNotPresent, out wasAdded);
        }

        public static TValue TryGetValueIfNotPresentThenAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key,
            Func<TKey, TValue> methodToCallToCreateIfNotPresent, out bool wasAdded)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                wasAdded = false;
                return value;
            }

            value = methodToCallToCreateIfNotPresent(key);
            wasAdded = dictionary.TryAdd(key, value);
            return value;
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer = null)
        {
            var result = new ConcurrentDictionary<TKey, TElement>(comparer ?? EqualityComparer<TKey>.Default);
            var dictionary = (IDictionary<TKey, TElement>) result;
            foreach (var item in source)
            {
                dictionary.Add(keySelector(item), elementSelector(item));
            }
            return result;
        }
    }
}
