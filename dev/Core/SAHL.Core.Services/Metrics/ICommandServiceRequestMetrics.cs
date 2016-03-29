using SAHL.Core.Metrics;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Services.Metrics
{
    public interface ICommandServiceRequestMetrics : IServiceMetric
    {
        string ServiceName { get; }

        string ServiceVersion { get; }

        long TotalRequestsCount { get; }

        IDictionary<string, long> CommandRequestsCount { get; }

        long TotalRequestsForLastMinute { get; }

        long TotalRequestsForLastHour { get; }

        long TotalRequestsForLastDay { get; }

        ITimerMetric TotalLatencyForLastMinute { get; }

        IDictionary<string, ITimerMetric> CommandLatencyForLastMinute { get; }

        ITimerMetric TotalLatencyForLastHour { get; }

        IDictionary<string, ITimerMetric> CommandLatencyForLastHour { get; }

        ITimerMetric TotalLatencyForLastDay { get; }

        IDictionary<string, ITimerMetric> CommandLatencyForLastDay { get; }

        TimeSpan ServiceUptime { get; }

        void SetServiceStartDateTime(string name, DateTime startDateTime);

        void IncrementRequestForCommand<T>(string name);

        void UpdateRequestLatencyForCommand<T>(string name, long durationInMilliseconds);
    }
}