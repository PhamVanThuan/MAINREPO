using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Shared.Extensions
{
	public static class DictionaryExtensions
	{
		public static void AddOrUpdateValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(key))
				{
					dictionary[key] = value;
				}
				else
				{
					dictionary.Add(key, value);
				}
			}
		}

		public static void RemoveKeyIfExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(key))
				{
					dictionary.Remove(key);
				}
			}
		}
	}
}
