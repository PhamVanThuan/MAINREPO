using System;

namespace SAHL.Core.Logging
{
    public interface IMetricTimerResult
    {
        DateTime StartTime { get; }

        TimeSpan Duration { get; }
    }
}