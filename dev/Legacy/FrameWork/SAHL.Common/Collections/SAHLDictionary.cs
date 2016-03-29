using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Collections
{
    /// <summary>
    /// Extension of the standard dictionary for SAHL.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SAHLDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISAHLDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets the key at a specified index.
        /// </summary>
        /// <param name="index">The index of the key in the Keys collection.</param>
        /// <returns>The key object</returns>
        public TKey GetKeyAt(int index)
        {
            int i = 0;

            foreach (TKey k in this.Keys)
            {
                if (i == index)
                    return k;

                i++;
            }

            return default(TKey);
        }

        /// <summary>
        /// Gets the first key out of the dictionary for the supplied value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public TKey GetKeyByValue(TValue val)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                if (kvp.Value.Equals(val))
                    return kvp.Key;
            }
            return default(TKey);
        }
    }
}