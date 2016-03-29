using System.Timers;
using SAHL.Services.Interfaces.PollingManager;

namespace SAHL.Services.PollingManager.Handlers
{
    public class TimerHandler : ITimerHandler
    {

        private Timer HandlerTimer { get; set; }

        public void Initialise(int timerInterval)
        {
            HandlerTimer = new Timer(timerInterval);
            
        }

        public void Start()
        {
            HandlerTimer.Start();
        }

        public void Stop()
        {
            HandlerTimer.Stop();
        }
    }
}