namespace SAHL.Core.Metrics
{
    internal class MetricSetName : IMetricSetName
    {
        internal MetricSetName(string name, IMetricName internalMetric)
        {
            this.Name = name;
            this.InternalMetric = internalMetric;
        }

        public string Value
        {
            get
            {
                return string.Format("{0}_{1}", this.Name, InternalMetric == null ? "Base" : InternalMetric.Value);
            }
        }

        public IMetricName InternalMetric
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }
    }
}