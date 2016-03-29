namespace SAHL.Core.Metrics
{
    public class TimerMetric : ITimerMetric
    {
        #region properties

        public IHistogramMetric Histogram
        {
            get;
            protected set;
        }

        public IMeterMetric Meter
        {
            get;
            protected set;
        }

        public TimeUnit DurationUnit
        {
            get;
            private set;
        }

        public TimeUnit RateUnit
        {
            get;
            private set;
        }

        public long Count
        {
            get
            {
                return this.Histogram.Count;
            }
        }

        public double MeanRate
        {
            get { return this.Meter.MeanRate; }
        }

        public double Max
        {
            get { return this.Histogram.Max; }
        }

        public double Min
        {
            get { return this.Histogram.Min; }
        }

        public double Mean
        {
            get { return this.Histogram.Mean; }
        }

        public double StdDeviation
        {
            get { return this.ConvertFromNanos(this.Histogram.StdDeviation); }
        }

        public IMetricName Name
        {
            get;
            protected set;
        }

        #endregion properties

        #region constructor

        public TimerMetric(IMetricName name, TimeUnit durationUnit, TimeUnit rateUnit, IHistogramMetric histogram, IMeterMetric meter)
        {
            this.Name = name;
            this.DurationUnit = durationUnit;
            this.RateUnit = rateUnit;
            this.Histogram = histogram;
            this.Meter = meter;
        }

        #endregion constructor

        #region methods

        public void Clear()
        {
            this.Histogram.Clear();
        }

        public void Update(TimeValue duration)
        {
            if (duration.Duration < 0L)
            {
                return;
            }
            this.Histogram.Update(duration.Duration);
            this.Meter.Mark();
        }

        #endregion methods

        #region private methods

        private double ConvertFromNanos(double nanos)
        {
            return nanos / (double)this.DurationUnit.Convert(1L, TimeUnit.Nanoseconds);
        }

        #endregion private methods
    }
}