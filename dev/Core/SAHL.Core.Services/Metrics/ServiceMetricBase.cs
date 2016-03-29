using SAHL.Core.Metrics;
using System;

namespace SAHL.Core.Services.Metrics
{
    public abstract class ServiceMetricBase : IServiceMetric
    {
        protected IMetricsCollector collector;

        public string Name
        {
            get;
            protected set;
        }

        public ServiceMetricBase(string name, IMetricsCollector metricCollector)
        {
            this.Name = name;
            this.collector = metricCollector;
        }

        public void SetServiceStartDateTime(string serviceMetricName, DateTime startDateTime)
        {
            collector.AddOrUpdateUptime(new TypeOwnedMetricName<IUpTime>(serviceMetricName), startDateTime);
        }
    }
}