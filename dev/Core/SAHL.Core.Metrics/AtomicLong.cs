using System.Threading;

namespace SAHL.Core.Metrics
{
    internal class AtomicLong
    {
        #region variables

        private long value;

        #endregion variables

        #region constructor

        public AtomicLong(long value)
        {
            this.Set(value);
        }

        public AtomicLong()
            : this(0)
        {
        }

        #endregion constructor

        #region methods

        internal void Set(long value)
        {
            Interlocked.Exchange(ref this.value, value);
        }

        internal void Add(long value)
        {
            Interlocked.Add(ref this.value, value);
        }

        internal long Get()
        {
            return Interlocked.Read(ref this.value);
        }

        internal bool CompareAndSet(long expected, long updated)
        {
            if (this.Get() == expected)
            {
                this.Set(updated);
                return true;
            }
            return false;
        }

        internal long Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        internal long Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        #endregion methods
    }
}