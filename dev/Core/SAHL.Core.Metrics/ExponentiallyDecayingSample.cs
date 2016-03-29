using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SAHL.Core.Metrics
{
    internal class ExponentiallyDecayingSample : ISample
    {
        #region variables

        private static readonly long RescaleThreshold = 3600000000000;
        private readonly ConcurrentDictionary<double, long> values;
        private readonly ReaderWriterLockSlim internalLock;
        private readonly int reservoirSize;
        private readonly double alpha;
        private readonly AtomicLong count = new AtomicLong(0L);
        private VolatileLong startTime;
        private readonly AtomicLong nextScaleTime = new AtomicLong(0L);

        #endregion variables

        #region properties

        public int Count
        {
            get
            {
                return (int)Math.Min((long)this.reservoirSize, this.count.Get());
            }
        }

        public ICollection<long> Values
        {
            get
            {
                this.internalLock.EnterReadLock();
                ICollection<long> result;
                try
                {
                    result = new List<long>(this.values.Values);
                }
                finally
                {
                    this.internalLock.ExitReadLock();
                }
                return result;
            }
        }

        #endregion properties

        #region constructor

        public ExponentiallyDecayingSample(int reservoirSize, double alpha)
        {
            this.internalLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            this.values = new ConcurrentDictionary<double, long>();
            this.alpha = alpha;
            this.reservoirSize = reservoirSize;
            this.Clear();
        }

        #endregion constructor

        #region methods

        public void Clear()
        {
            this.values.Clear();
            this.count.Set(0L);
            this.startTime = ExponentiallyDecayingSample.Tick();
        }

        public void Update(long value)
        {
            this.Update(value, ExponentiallyDecayingSample.Tick());
        }

        #endregion methods

        #region private methods

        private void Update(long value, long timestamp)
        {
            this.internalLock.EnterReadLock();
            try
            {
                double priority = this.Weight(timestamp - this.startTime) / (double)Random.NextLong();
                long newCount = this.count.Increment();
                if (newCount <= (long)this.reservoirSize)
                {
                    this.values.AddOrUpdate(priority, value, (double p, long v) => v);
                }
                else
                {
                    double first = this.values.Keys.First<double>();
                    if (first < priority)
                    {
                        this.values.AddOrUpdate(priority, value, (double p, long v) => v);
                        long removed;
                        while (!this.values.TryRemove(first, out removed))
                        {
                            first = this.values.Keys.First<double>();
                        }
                    }
                }
            }
            finally
            {
                this.internalLock.ExitReadLock();
            }
            long now = DateTime.Now.Ticks;
            long next = this.nextScaleTime.Get();
            if (now >= next)
            {
                this.Rescale(now, next);
            }
        }

        private static long Tick()
        {
            return DateTime.Now.Ticks;
        }

        private double Weight(long t)
        {
            return Math.Exp(this.alpha * (double)t);
        }

        private void Rescale(long now, long next)
        {
            if (!this.nextScaleTime.CompareAndSet(next, now + ExponentiallyDecayingSample.RescaleThreshold))
            {
                return;
            }
            this.internalLock.EnterWriteLock();
            try
            {
                VolatileLong oldStartTime = this.startTime;
                this.startTime = ExponentiallyDecayingSample.Tick();
                List<double> keys = new List<double>(this.values.Keys);
                foreach (double key in keys)
                {
                    long value;
                    this.values.TryRemove(key, out value);
                    this.values.AddOrUpdate(key * Math.Exp(-this.alpha * (double)(this.startTime - oldStartTime)), value, (double k, long v) => v);
                }
            }
            finally
            {
                this.internalLock.ExitWriteLock();
            }
        }

        #endregion private methods
    }
}