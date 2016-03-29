using SAHL.Core.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.Metrics
{
    public class CommandServiceRequestMetrics : ServiceMetricBase, ICommandServiceRequestMetrics
    {
        private IHostedService hostedService;

        public CommandServiceRequestMetrics(string serviceName, IMetricsCollector collector, IHostedService hostedService)
            : base(serviceName, collector)
        {
            this.hostedService = hostedService;
        }

        public IDictionary<string, ITimerMetric> CommandLatencyForLastDay
        {
            get
            {
                return this.collector.GetMetricsNameForSet<ITimerMetric>(this.Name).Where(x => x.RateUnit == TimeUnit.Days).ToDictionary(x => x.Name.Value);
            }
        }

        public IDictionary<string, ITimerMetric> CommandLatencyForLastHour
        {
            get
            {
                return this.collector.GetMetricsNameForSet<ITimerMetric>(this.Name).Where(x => x.RateUnit == TimeUnit.Hours).ToDictionary(x => x.Name.Value);
            }
        }

        public IDictionary<string, ITimerMetric> CommandLatencyForLastMinute
        {
            get
            {
                return this.collector.GetMetricsNameForSet<ITimerMetric>(this.Name).Where(x => x.RateUnit == TimeUnit.Minutes).ToDictionary(x => x.Name.Value);
            }
        }

        public IDictionary<string, long> CommandRequestsCount
        {
            get
            {
                return this.collector.GetMetricsNameForSet(this.Name).OfType<ICounterMetric>().ToDictionary(k => k.Name.Value, v => v.Count);
            }
        }

        public ITimerMetric TotalLatencyForLastDay
        {
            get
            {
                return this.collector.GetSetMetrics(this.Name).OfType<ITimerMetric>().SingleOrDefault(x => x.RateUnit == TimeUnit.Days);
            }
        }

        public ITimerMetric TotalLatencyForLastHour
        {
            get
            {
                return this.collector.GetSetMetrics(this.Name).OfType<ITimerMetric>().SingleOrDefault(x => x.RateUnit == TimeUnit.Hours);
            }
        }

        public ITimerMetric TotalLatencyForLastMinute
        {
            get
            {
                return this.collector.GetSetMetrics(this.Name).OfType<ITimerMetric>().SingleOrDefault(x => x.RateUnit == TimeUnit.Minutes);
            }
        }

        public long TotalRequestsCount
        {
            get
            {
                ICounterMetric counter = this.collector.GetSetMetrics(this.Name).OfType<ICounterMetric>().SingleOrDefault();
                if (counter != null)
                {
                    return counter.Count;
                }
                return 0;
            }
        }

        public long TotalRequestsForLastDay
        {
            get
            {
                IResetCounter counter = this.collector.GetSetMetrics(this.Name).OfType<IResetCounter>().SingleOrDefault(x => x.RateUnit == TimeUnit.Days);
                if (counter != null)
                {
                    return counter.Count;
                }
                return 0;
            }
        }

        public long TotalRequestsForLastHour
        {
            get
            {
                IResetCounter counter = this.collector.GetSetMetrics(this.Name).OfType<IResetCounter>().SingleOrDefault(x => x.RateUnit == TimeUnit.Hours);
                if (counter != null)
                {
                    return counter.Count;
                }
                return 0;
            }
        }

        public long TotalRequestsForLastMinute
        {
            get
            {
                IResetCounter counter = this.collector.GetSetMetrics(this.Name).OfType<IResetCounter>().SingleOrDefault(x => x.RateUnit == TimeUnit.Minutes);
                if (counter != null)
                {
                    return counter.Count;
                }
                return 0;
            }
        }

        public TimeSpan ServiceUptime
        {
            get
            {
                var uptimes = this.collector.All.OfType<IUpTime>().ToArray();
                var requiredMatrixName = new TypeOwnedMetricName<IUpTime>(this.Name);
                return uptimes.FirstOrDefault(ut => ut.Name.Value.Equals(requiredMatrixName.Value)).UpTime;
            }
        }

        public void IncrementRequestForCommand<T>(string name)
        {
            collector.AddOrUpdateCounterSet(this.Name, new TypeOwnedMetricName<T>(name));

            collector.AddOrUpdateResetCounterSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Minutes);
            collector.AddOrUpdateResetCounterSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Hours);
            collector.AddOrUpdateResetCounterSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Days);
        }

        public void UpdateRequestLatencyForCommand<T>(string name, long durationInMilliseconds)
        {
            collector.AddOrUpdateTimerSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Milliseconds, TimeUnit.Minutes, new TimeValue(durationInMilliseconds, TimeUnit.Milliseconds));
            collector.AddOrUpdateTimerSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Milliseconds, TimeUnit.Hours, new TimeValue(durationInMilliseconds, TimeUnit.Milliseconds));
            collector.AddOrUpdateTimerSet(this.Name, new TypeOwnedMetricName<T>(name), TimeUnit.Milliseconds, TimeUnit.Days, new TimeValue(durationInMilliseconds, TimeUnit.Milliseconds));
        }

        public string ServiceName
        {
            get { return this.hostedService.Name; }
        }

        public string ServiceVersion
        {
            get { return this.hostedService.Version; }
        }
    }
}