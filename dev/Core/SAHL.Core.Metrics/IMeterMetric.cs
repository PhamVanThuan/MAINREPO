namespace SAHL.Core.Metrics
{
    public interface IMeterMetric : IMetric
    {
        long Count { get; }

        double Rate { get; }

        double MeanRate { get; }

        TimeUnit RateUnit { get; }

        void Mark();

        void Mark(long value);
    }
}