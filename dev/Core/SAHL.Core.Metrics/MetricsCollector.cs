using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Metrics
{
    public class MetricsCollector : IMetricsCollector
    {
        private static readonly ConcurrentDictionary<IMetricKey, IMetric> internalMetrics = new ConcurrentDictionary<IMetricKey, IMetric>();

        public void AddOrUpdateCounter(IMetricName name)
        {
            GetOrAdd<ICounterMetric>(name, name, () =>
            {
                return new CounterMetric(name);
            }).Increment();
        }

        public void AddOrUpdateCounterSet(string metricSetName, IMetricName name)
        {
            AddOrUpdateCounter(new MetricSetName(metricSetName, null));
            AddOrUpdateCounter(new MetricSetName(metricSetName, name));
        }

        public void AddOrUpdateResetCounter(IMetricName name, TimeUnit rateUnit)
        {
            GetOrAdd<IResetCounter>(new TimeUnitName(rateUnit, name), name, () =>
            {
                return ResetCounter.New(name, rateUnit);
            }).Increment();
        }

        public void AddOrUpdateResetCounterSet(string metricSetName, IMetricName name, TimeUnit rateUnit)
        {
            AddOrUpdateResetCounter(new MetricSetName(metricSetName, null), rateUnit);
            AddOrUpdateResetCounter(new MetricSetName(metricSetName, name), rateUnit);
        }

        public void AddOrUpdateGauge<T>(IMetricName name, Func<T> expression)
        {
            GetOrAdd<IGaugeMetric<T>>(name, name, () =>
            {
                return new GaugeMetric<T>(name, expression);
            });
        }

        public void AddOrUpdateGaugeSet<T>(string metricSetName, IMetricName name, Func<T> expression)
        {
            AddOrUpdateGauge(new MetricSetName(metricSetName, null), expression);
            AddOrUpdateGauge(new MetricSetName(metricSetName, name), expression);
        }

        public void AddOrUpdateHistogram(IMetricName name, SampleType sampleType, long value)
        {
            GetOrAdd<IHistogramMetric>(name, name, () =>
            {
                return new HistogramMetric(name, sampleType);
            }).Update(value);
        }

        public void AddOrUpdateHistogramSet(string metricSetName, IMetricName name, SampleType sampleType, long value)
        {
            AddOrUpdateHistogram(new MetricSetName(metricSetName, null), sampleType, value);
            AddOrUpdateHistogram(new MetricSetName(metricSetName, name), sampleType, value);
        }

        public void AddOrUpdateMeter(IMetricName name, TimeUnit unit)
        {
            GetOrAdd<IMeterMetric>(new TimeUnitName(unit, name), name, () =>
            {
                return MeterMetric.New(name, unit);
            }).Mark();
        }

        public void AddOrUpdateMeterSet(string metricSetName, IMetricName name, TimeUnit unit)
        {
            AddOrUpdateMeter(new MetricSetName(metricSetName, null), unit);
            AddOrUpdateMeter(new MetricSetName(metricSetName, name), unit);
        }

        public void AddOrUpdateTimer(IMetricName name, TimeUnit durationUnit, TimeUnit rateUnit, TimeValue timeValue)
        {
            TimeUnitName rateName = new TimeUnitName(rateUnit, name);
            IHistogramMetric metricHistogram = GetOrAdd<IHistogramMetric>(rateName, rateName, () =>
            {
                return new HistogramMetric(name, SampleType.Biased);
            });

            IMeterMetric metricMeter = GetOrAdd<IMeterMetric>(rateName, rateName, () =>
            {
                return MeterMetric.New(name, rateUnit);
            });

            GetOrAdd<ITimerMetric>(rateName, name, () =>
            {
                return new TimerMetric(name, durationUnit, rateUnit, metricHistogram, metricMeter);
            }).Update(timeValue);
        }

        public void AddOrUpdateTimerSet(string metricSetName, IMetricName name, TimeUnit durationUnit, TimeUnit rateUnit, TimeValue timeValue)
        {
            AddOrUpdateTimer(new MetricSetName(metricSetName, null), durationUnit, rateUnit, timeValue);
            AddOrUpdateTimer(new MetricSetName(metricSetName, name), durationUnit, rateUnit, timeValue);
        }

        public void AddOrUpdateUptime(IMetricName name, DateTime startDateTime)
        {
            GetOrAdd<IUpTime>(name, name, () =>
            {
                return new UpTime(name, startDateTime);
            }).Update(startDateTime);
        }

        public IEnumerable<IMetric> All
        {
            get
            {
                return this.PrivateAll.Values;
            }
        }

        public IEnumerable<IMetric> GetSetMetrics(string setName)
        {
            return this.PrivateAll.Where(
                kvp => kvp.Key.MetricName is IMetricSetName && ((IMetricSetName)kvp.Key.MetricName).InternalMetric == null && ((IMetricSetName)kvp.Key.MetricName).Name == setName
            ).Select(
                kvp => kvp.Value
            );
        }

        IEnumerable<IMetric> IMetricsCollector.GetMetricsNameForSet(string setName)
        {
            return this.PrivateAll.Where(
                kvp => kvp.Key.MetricName is IMetricSetName && ((IMetricSetName)kvp.Key.MetricName).InternalMetric != null && ((IMetricSetName)kvp.Key.MetricName).Name == setName
            ).Select(
                kvp => kvp.Value
            );
        }

        public IEnumerable<T> GetMetricsNameForSet<T>(string setName) where T : IMetric
        {
            return this.PrivateAll.Where(
                kvp => kvp.Key.MetricName is IMetricSetName && ((IMetricSetName)kvp.Key.MetricName).InternalMetric != null && ((IMetricSetName)kvp.Key.MetricName).Name == setName
            ).Select(
                kvp => kvp.Value
            ).OfType<T>();
        }

        private T GetOrAdd<T>(IMetricName uniqueKey, IMetricName name, Func<T> newMetric) where T : IMetric
        {
            IMetricKey newName = new MetricKey<T>(uniqueKey, name);
            IMetric metric;
            if (MetricsCollector.internalMetrics.TryGetValue(newName, out metric))
            {
                return (T)metric;
            }
            metric = newMetric();
            IMetric orAdd = MetricsCollector.internalMetrics.GetOrAdd(newName, metric);
            if (orAdd != null)
            {
                return (T)orAdd;
            }
            return (T)metric;
        }

        public IDictionary<IMetricKey, IMetric> PrivateAll
        {
            get
            {
                return new Dictionary<IMetricKey, IMetric>(MetricsCollector.internalMetrics);
            }
        }
    }
}