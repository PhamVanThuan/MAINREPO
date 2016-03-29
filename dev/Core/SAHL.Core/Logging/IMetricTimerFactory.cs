namespace SAHL.Core.Logging
{
    public interface IMetricTimerFactory
    {
        IMetricTimer NewTimer();
    }
}