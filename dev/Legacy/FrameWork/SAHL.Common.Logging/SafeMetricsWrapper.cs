using System;
using System.Collections.Generic;

namespace SAHL.Common.Logging
{
    public class SafeMetricsWrapper : IMetrics
    {
        public SafeMetricsWrapper(IMetrics innerMetrics)
        {
            this.InnerMetrics = innerMetrics;
        }

        public IMetrics InnerMetrics { get; protected set; }

        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishLatencyMetric(startTime, duration, source);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source, Dictionary<string, object> parameters)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishLatencyMetric(startTime, duration, source, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void PublishInstantaneousValueMetric(int value, string source)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishInstantaneousValueMetric(value, source);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void PublishInstantaneousValueMetric(int value, string source, Dictionary<string, object> parameters)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishInstantaneousValueMetric(value, source, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishThroughputMetric(source, throughputRates);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates, Dictionary<string, object> parameters)
        {
            try
            {
                if (this.InnerMetrics != null)
                {
                    this.InnerMetrics.PublishThroughputMetric(source, throughputRates, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }
    }
}