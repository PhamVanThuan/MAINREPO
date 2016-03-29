using System.Collections.Generic;

namespace SAHL.Core.Metrics
{
    public interface IHistogramMetric : IMetric
    {
        long Count { get; }

        double Max { get; }

        double Min { get; }

        double Mean { get; }

        double StdDeviation { get; }

        ICollection<long> Values { get; }

        long SampleCount { get; }

        double SampleMax { get; }

        double SampleMin { get; }

        double SampleMean { get; }

        void Update(int value);

        void Update(long value);

        void Clear();
    }
}