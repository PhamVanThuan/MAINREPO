using System;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class FakeTimeoutService : ITimeoutService
    {
        private int requestTimeoutInMilliseconds;
        private X2Request request;
        private IX2RequestMonitorCallback requestTimeoutCallback;
        private IResponseThreadWaiter responseThreadWaiter;

        public FakeTimeoutService(X2Request request,IX2RequestMonitorCallback requestTimeoutCallback, int requestTimeoutInMilliseconds, IResponseThreadWaiter responseThreadWaiter)
        {
            this.requestTimeoutCallback = requestTimeoutCallback;
            this.request = request;
            this.requestTimeoutInMilliseconds = requestTimeoutInMilliseconds;
            this.responseThreadWaiter = responseThreadWaiter;
        }

        public void Start()
        {
            requestTimeoutCallback.RequestTimedout(request, responseThreadWaiter);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}