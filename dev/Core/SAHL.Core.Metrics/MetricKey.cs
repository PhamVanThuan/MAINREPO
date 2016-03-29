using System;

namespace SAHL.Core.Metrics
{
    internal class MetricKey<T> : IMetricKey
    {
        IMetricName UniqueKey;

        public IMetricName MetricName { get; protected set; }

        public Type Owner { get { return typeof(T); } }

        public string Value
        {
            get;
            protected set;
        }

        internal MetricKey(IMetricName uniqueKey, IMetricName metricName)
        {
            this.UniqueKey = uniqueKey;
            this.MetricName = metricName;
            this.Value = string.Format("{0}_{1}", Owner, this.UniqueKey.Value);
        }

        public override bool Equals(object obj)
        {
            var temp = (IMetricKey)obj;
            return temp != null && temp.Value == this.Value;
        }

        public override int GetHashCode()
        {
            return (this.Value.GetHashCode());
        }
    }
}