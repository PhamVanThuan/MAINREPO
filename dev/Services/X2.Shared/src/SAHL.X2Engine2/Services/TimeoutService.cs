using System.Timers;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Services
{
    public class TimeoutService : ITimeoutService
    {
        private IX2Request request;
        private IX2RequestMonitorCallback requestTimeoutCallback;
        private IResponseThreadWaiter responseThreadWaiter;
        private int RequestTimeoutInMilliseconds;
        private System.Timers.Timer timer;

        public TimeoutService(IX2Request request,IX2RequestMonitorCallback requestTimeoutCallback, int requestTimeoutInMilliseconds, IResponseThreadWaiter responseThreadWaiter)
        {
            this.request = request;
            this.requestTimeoutCallback = requestTimeoutCallback;
            this.RequestTimeoutInMilliseconds = requestTimeoutInMilliseconds;
            this.responseThreadWaiter = responseThreadWaiter;
        }

        public void Start()
        {
            timer = new System.Timers.Timer(RequestTimeoutInMilliseconds);
            timer.Start();
            timer.Elapsed += t_Elapsed;
        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timer != null)
            {
                requestTimeoutCallback.RequestTimedout(request, responseThreadWaiter);
            }
        }

        public void Stop()
        {
            if (null != timer)
            {
                timer.Enabled = false;
                timer.Stop();
                timer = null;
            }
        }
    }
}