namespace SAHL.Core.Metrics
{
    public class TimeUnitName : IMetricSetName
    {
        public string Name
        {
            get { return Rate.ToString(); }
        }

        public TimeUnit Rate
        {
            get;
            protected set;
        }

        public IMetricName InternalMetric
        {
            get;
            protected set;
        }

        public string Value
        {
            get;
            protected set;
        }

        public TimeUnitName(TimeUnit rate, IMetricName name)
        {
            this.Rate = rate;
            this.InternalMetric = name;
            this.Value = string.Format("{0}_{1}", this.Name, InternalMetric.Value);
        }
    }
}