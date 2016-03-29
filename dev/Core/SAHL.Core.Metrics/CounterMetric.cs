namespace SAHL.Core.Metrics
{
    public class CounterMetric : ICounterMetric
    {
        #region variables

        private readonly AtomicLong count = new AtomicLong();

        #endregion variables

        #region properties

        public long Count
        {
            get { return this.count.Get(); }
        }

        public IMetricName Name
        {
            get;
            protected set;
        }

        #endregion properties

        #region constructor

        public CounterMetric(IMetricName name)
        {
            this.Name = name;
        }

        #endregion constructor

        #region methods

        public void Increment()
        {
            this.IncrementBy(1);
        }

        public void IncrementBy(long amount)
        {
            this.count.Add(amount);
        }

        public void Decrement()
        {
            this.DecrementBy(1);
        }

        public void DecrementBy(long amount)
        {
            this.count.Add(-amount);
        }

        public void Clear()
        {
            this.count.Set(0);
        }

        #endregion methods
    }
}