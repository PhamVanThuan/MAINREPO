namespace SAHL.Core.Metrics
{
    public interface ITimerMetric : IMetric
    {
        IHistogramMetric Histogram { get; }

        IMeterMetric Meter { get; }

        TimeUnit DurationUnit { get; }

        TimeUnit RateUnit { get; }

        long Count { get; }

        double MeanRate { get; }

        double Max { get; }

        double Min { get; }

        double Mean { get; }

        double StdDeviation { get; }

        void Clear();

        void Update(TimeValue duration);
    }
}