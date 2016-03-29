using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Factories
{
    public class X2ScheduledActivityTimerFactory : IX2ScheduledActivityTimerFactory
    {
        public Services.IX2ScheduledActivityTimer CreateTimer()
        {
            return new X2ScheduledActivityTimer();
        }
    }
}