namespace SAHL.Core.Logging
{
    public interface IMetricTimer
    {
        void Start();

        IMetricTimerResult Stop();
    }
}