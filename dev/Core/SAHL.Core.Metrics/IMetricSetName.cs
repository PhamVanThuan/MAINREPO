namespace SAHL.Core.Metrics
{
    public interface IMetricSetName : IMetricName
    {
        string Name { get; }

        IMetricName InternalMetric { get; }
    }
}