namespace SAHL.Core.Metrics
{
    public class TimeValue
    {
        public TimeValue(long duration, TimeUnit timeUnit)
        {
            this.Duration = duration;
            this.TimeUnit = timeUnit;
        }

        public long Duration { get; protected set; }

        public TimeUnit TimeUnit { get; protected set; }
    }
}