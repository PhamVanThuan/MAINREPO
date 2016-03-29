namespace SAHL.Core.Metrics
{
    internal class EWMA
    {
        private readonly AtomicLong uncounted = new AtomicLong(0L);
        private readonly double alpha;
        private readonly double interval;
        private volatile bool initialized;
        private VolatileDouble rate;

        public EWMA(double alpha, long interval, TimeUnit intervalUnit)
        {
            this.interval = (double)intervalUnit.ToNanos(interval);
            this.alpha = alpha;
        }

        public void Update(long n)
        {
            this.uncounted.Add(n);
        }

        public void Tick()
        {
            long count = this.uncounted.Get();
            this.uncounted.Set(0L);
            double instantRate = (double)count / this.interval;
            if (this.initialized)
            {
                this.rate += this.alpha * (instantRate - this.rate);
                return;
            }
            this.rate.Set(instantRate);
            this.initialized = true;
        }

        public double Rate(TimeUnit rateUnit)
        {
            long nanos = rateUnit.ToNanos(1L);
            return this.rate * (double)nanos;
        }
    }
}