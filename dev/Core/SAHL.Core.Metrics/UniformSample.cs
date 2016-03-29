using System;
using System.Collections.Generic;
using System.Threading;

namespace SAHL.Core.Metrics
{
    internal class UniformSample : ISample
    {
        #region variables

        private readonly AtomicLong count = new AtomicLong();
        private readonly long[] values;

        #endregion variables

        #region properties

        public int Count
        {
            get
            {
                long c = this.count.Get();
                if (c > (long)this.values.Length)
                {
                    return this.values.Length;
                }
                return (int)c;
            }
        }

        public ICollection<long> Values
        {
            get
            {
                int size = this.Count;
                List<long> copy = new List<long>(size);
                for (int i = 0; i < size; i++)
                {
                    copy.Add(Interlocked.Read(ref this.values[i]));
                }
                return copy;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.values.Length; i++)
            {
                Interlocked.Exchange(ref this.values[i], 0L);
            }
            this.count.Set(0L);
        }

        public void Update(long value)
        {
            long count = this.count.Increment();
            if (count <= (long)this.values.Length)
            {
                int index = (int)count - 1;
                Interlocked.Exchange(ref this.values[index], value);
                return;
            }
            long random = Math.Abs(Random.NextLong()) % count;
            if (random < (long)this.values.Length)
            {
                int index2 = (int)random;
                Interlocked.Exchange(ref this.values[index2], value);
            }
        }

        #endregion properties

        #region constructor

        public UniformSample(int reservoirSize)
        {
            this.values = new long[reservoirSize];
            this.Clear();
        }

        #endregion constructor
    }
}