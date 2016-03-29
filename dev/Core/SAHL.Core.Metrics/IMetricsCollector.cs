using System;
using System.Collections.Generic;

namespace SAHL.Core.Metrics
{
    public interface IMetricsCollector
    {
        void AddOrUpdateCounter(IMetricName name);

        void AddOrUpdateCounterSet(string metricSetName, IMetricName name);

        void AddOrUpdateResetCounter(IMetricName name, TimeUnit rateUnit);

        void AddOrUpdateResetCounterSet(string metricSetName, IMetricName name, TimeUnit rateUnit);

        void AddOrUpdateGauge<T>(IMetricName name, Func<T> expression);

        void AddOrUpdateGaugeSet<T>(string metricSetName, IMetricName name, Func<T> expression);

        void AddOrUpdateHistogram(IMetricName name, SampleType sampleType, long value);

        void AddOrUpdateHistogramSet(string metricSetName, IMetricName name, SampleType sampleType, long value);

        void AddOrUpdateMeter(IMetricName name, TimeUnit unit);

        void AddOrUpdateMeterSet(string metricSetName, IMetricName name, TimeUnit unit);

        void AddOrUpdateTimer(IMetricName name, TimeUnit durationUnit, TimeUnit rateUnit, TimeValue timeValue);

        void AddOrUpdateTimerSet(string metricSetName, IMetricName name, TimeUnit durationUnit, TimeUnit rateUnit, TimeValue timeValue);

        void AddOrUpdateUptime(IMetricName name, DateTime startDateTime);

        IEnumerable<IMetric> GetSetMetrics(string setName);

        IEnumerable<IMetric> GetMetricsNameForSet(string setName);

        IEnumerable<T> GetMetricsNameForSet<T>(string setName) where T : IMetric;

        IEnumerable<IMetric> All { get; }
    }
}