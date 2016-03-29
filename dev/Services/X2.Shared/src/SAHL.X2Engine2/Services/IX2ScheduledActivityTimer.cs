using System;

namespace SAHL.X2Engine2.Services
{
    public interface IX2ScheduledActivityTimer
    {
        void Start(int timeUntilTimerInMilliseconds, Action callback);

        void Stop();
    }
}