using System;

namespace SAHL.Core.Metrics
{
    public interface IUpTime : IMetric
    {
        DateTime StartDateTime { get; }

        TimeSpan UpTime { get; }

        void Update(DateTime startDateTime);
    }
}