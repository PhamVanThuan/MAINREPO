namespace SAHL.Core.Metrics
{
    public interface ICounterMetric : IMetric
    {
        long Count { get; }

        void Increment();

        void IncrementBy(long amount);

        void Decrement();

        void DecrementBy(long amount);

        void Clear();
    }
}