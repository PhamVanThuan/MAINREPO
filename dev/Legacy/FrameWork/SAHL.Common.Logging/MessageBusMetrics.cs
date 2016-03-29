using System;
using System.Collections.Generic;
using SAHL.Communication;
using SAHL.Shared.Messages;
using SAHL.Shared.Messages.Metrics;

namespace SAHL.Common.Logging
{
    public class MessageBusMetrics : IMetrics
    {
        private IMessageBus messageBus = null;
        private string applicationName = String.Empty;
        private MessageBusMetricsConfiguration messageBusMetricsConfiguration;

        public MessageBusMetrics(IMessageBus messageBus, MessageBusMetricsConfiguration messageBusMetricsConfiguration)
        {
            this.messageBus = messageBus;
            this.applicationName = messageBusMetricsConfiguration.ApplicationName;
            this.messageBusMetricsConfiguration = messageBusMetricsConfiguration;
        }

        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source)
        {
            this.PublishLatencyMetric(startTime, duration, source, null);
        }

        public void PublishLatencyMetric(DateTime startTime, TimeSpan duration, string source, Dictionary<string, object> parameters)
        {
            if (null == parameters)
            {
                parameters = new Dictionary<string, object>();
            }

            ProcessThreadLocalParameters(parameters);

            LatencyMetricMessage message = new LatencyMetricMessage(this.applicationName, source, startTime, duration, "", parameters);
            this.Publish(message);
        }

        public void PublishInstantaneousValueMetric(int value, string source)
        {
            this.PublishInstantaneousValueMetric(value, source, null);
        }

        public void PublishInstantaneousValueMetric(int value, string source, Dictionary<string, object> parameters)
        {
            if (null == parameters)
            {
                parameters = new Dictionary<string, object>();
            }

            ProcessThreadLocalParameters(parameters);

            InstantaneousValueMetricMessage message = new InstantaneousValueMetricMessage(this.applicationName, source, value, "", parameters);
            this.Publish(message);
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates)
        {
            this.PublishThroughputMetric(source, throughputRates, null);
        }

        public void PublishThroughputMetric(string source, IEnumerable<Shared.Messages.Metrics.TimeUnit> throughputRates, Dictionary<string, object> parameters)
        {
            if (null == parameters)
            {
                parameters = new Dictionary<string, object>();
            }

            ProcessThreadLocalParameters(parameters);

            ThroughputMetricMessage message = new ThroughputMetricMessage(this.applicationName, source, throughputRates, "", parameters);
            this.Publish(message);
        }

        private void Publish<T>(T message) where T : class, IMetricMessage
        {
            if (this.messageBusMetricsConfiguration.PublishMetrics)
            {
                this.messageBus.Publish(message);
            }
        }

        private void ProcessThreadLocalParameters(Dictionary<string, object> parameters)
        {
            Dictionary<string, object> threadParameters = Metrics.ThreadContext;
            if (threadParameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in threadParameters)
                {
                    string parameterKey = kvp.Key;
                    if (parameters.ContainsKey(parameterKey))
                    {
                        parameterKey = string.Format("{0}_{1}", parameterKey, Guid.NewGuid());
                    }

                    parameters.Add(parameterKey, kvp.Value);
                }
            }
        }
    }
}