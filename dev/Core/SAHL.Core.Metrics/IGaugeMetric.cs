namespace SAHL.Core.Metrics
{
    public interface IGaugeMetric : IMetric
    {
        string ValueAsString { get; }
    }

    public interface IGaugeMetric<T> : IGaugeMetric
    {
        T Value { get; }
    }
}