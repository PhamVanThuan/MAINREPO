using System;

namespace SAHL.Core.Logging
{
    public class MetricTimerResult : IMetricTimerResult
    {
        public MetricTimerResult(DateTime startTime, TimeSpan duration)
        {
            this.StartTime = startTime;
            this.Duration = duration;
        }

        public DateTime StartTime
        {
            get;
            protected set;
        }

        public TimeSpan Duration
        {
            get;
            protected set;
        }
    }
}