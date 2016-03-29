using System;
using System.Threading;

namespace SAHL.X2Engine2.Services
{
    public class X2ScheduledActivityTimer : IX2ScheduledActivityTimer
    {
        private Timer timer;

        public void Start(int timeUntilTimerInMilliseconds, Action callback)
        {
            timer = new Timer((state) => { callback(); }, null, timeUntilTimerInMilliseconds, System.Threading.Timeout.Infinite);
        }

        public void Stop()
        {
            timer = null;
        }
    }
}