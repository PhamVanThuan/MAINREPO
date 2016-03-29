namespace SAHL.Core.Metrics
{
    public interface IMetricKey : IMetricName
    {
        IMetricName MetricName { get; }
    }
}