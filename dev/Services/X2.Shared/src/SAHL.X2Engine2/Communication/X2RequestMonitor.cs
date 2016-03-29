using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Communication
{
    public class X2RequestMonitor : IX2RequestMonitor
    {
        private IX2RequestMonitorCallback requestCallback;
        private IResponseThreadWaiter threadWaiter;
        private ITimeoutServiceFactory timeoutServiceFactory;
        private ITimeoutService timeoutService;

        public X2RequestMonitor(IResponseThreadWaiter threadWaiter, ITimeoutServiceFactory timeoutServiceFactory)
        {
            this.threadWaiter = threadWaiter;
            this.timeoutServiceFactory = timeoutServiceFactory;
        }

        public void Initialise(IX2RequestMonitorCallback requestCallback)
        {
            this.requestCallback = requestCallback;
        }

        public void MonitorRequest(IX2Request request)
        {
            timeoutService = timeoutServiceFactory.Create(request, requestCallback, threadWaiter);
            timeoutService.Start();
        }

        public void HandleResponse(X2Response response)
        {
            requestCallback.RequestCompleted(response);
        }

        public void Reset(IX2Request request)
        {
            if (timeoutService != null)
            {
                timeoutService.Stop();
            }
            timeoutService = timeoutServiceFactory.Create(request, requestCallback, threadWaiter);
            timeoutService.Start();
        }

        public void Stop()
        {
            if (timeoutService != null)
                timeoutService.Stop();
        }
    }
}