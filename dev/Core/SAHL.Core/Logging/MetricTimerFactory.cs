namespace SAHL.Core.Logging
{
    public class MetricTimerFactory : IMetricTimerFactory
    {
        public IMetricTimer NewTimer()
        {
            return new MetricTimer();
        }
    }
}