using System;
using System.Collections.Generic;
using System.Threading;

namespace SAHL.X2.Common
{
    public enum KnownGoodKeys
    {
        ORGSTRUCTURE_DATASET_Client,
        ORGSTRUCTURE_DATASET_DS
    };

    public class DSCache
    {
        // Declaring the ReaderWriterLock at the class level
        // makes it visible to all threads.
        private static ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();
        private static Dictionary<string, object> DSCacheItems = new Dictionary<string, object>();

        public static void Clear()
        {
            rwls.EnterWriteLock();
            try
            {
                DSCacheItems.Clear();
            }
            finally
            {
                // Ensure that the lock is released.
                rwls.ExitWriteLock();
            }
        }

        public static void Add(string Key, Object Value)
        {
            rwls.EnterWriteLock();
            try
            {
                DSCacheItems.Add(Key, Value);
            }
            finally
            {
                // Ensure that the lock is released.
                rwls.ExitWriteLock();
            }
        }

        public static object Get(string Key)
        {
            rwls.EnterReadLock();
            try
            {
                if (DSCacheItems.ContainsKey(Key))
                {
                    return DSCacheItems[Key];
                }
                return null;
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }
    }
}