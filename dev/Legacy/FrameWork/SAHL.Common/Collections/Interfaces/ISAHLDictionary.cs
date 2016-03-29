using System.Collections.Generic;

namespace SAHL.Common.Collections.Interfaces
{
    /// <summary>
    /// Extension of the standard dictionary for SAHL.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ISAHLDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets the key at a specified index.
        /// </summary>
        /// <param name="index">The index of the key in the Keys collection.</param>
        /// <returns>The key object</returns>
        TKey GetKeyAt(int index);

        /// <summary>
        /// Gets the first key out of the dictionary for the supplied value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        TKey GetKeyByValue(TValue val);
    }
}