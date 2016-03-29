using System.Collections.Generic;

namespace AspectAttribute
{
    public class PerfCache
    {
        static Dictionary<int, PerfObj> Cache = new Dictionary<int, PerfObj>();

        public static void Add(int Key, PerfObj obj)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(Key))
                    Cache.Remove(Key);
                Cache.Add(Key, obj);
            }
        }

        public static PerfObj Get(int Key)
        {
            if (Cache.ContainsKey(Key))
            {
                return Cache[Key];
            }
            return null;
        }

        public static void Remove(int Key)
        {
            lock (Cache)
            {
                Cache.Remove(Key);
            }
        }
    }
}