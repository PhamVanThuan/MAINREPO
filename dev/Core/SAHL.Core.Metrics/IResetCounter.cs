namespace SAHL.Core.Metrics
{
    public interface IResetCounter : IMetric
    {
        long Count { get; }

        void Increment();

        void IncrementBy(long amount);

        void Decrement();

        void DecrementBy(long amount);

        void Clear();

        TimeUnit RateUnit { get; }
    }
}