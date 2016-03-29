using System;

namespace SAHL.Core.Metrics
{
    [Serializable]
    public enum TimeUnit : int
    {
        Nanoseconds = 0,
        Microseconds = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6
    }
}