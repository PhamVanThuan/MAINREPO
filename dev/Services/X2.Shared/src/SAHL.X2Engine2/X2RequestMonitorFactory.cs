using System.Collections.Concurrent;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Services;
using System;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    public class X2RequestMonitorFactory : IX2RequestMonitorFactory
    {
        private IX2ResponseSubscriber responseSubscriber;
        private ITimeoutServiceFactory timeoutServiceFactory;
        private ConcurrentDictionary<IResponseThreadWaiter, IX2RequestMonitor> requestMonitors;
        private ConcurrentDictionary<Guid, Action<X2Response>> requestResponseHandlers;

        public X2RequestMonitorFactory(IX2ResponseSubscriber responseSubscriber, ITimeoutServiceFactory timeoutServiceFactory)
        {
            this.responseSubscriber = responseSubscriber;
            this.timeoutServiceFactory = timeoutServiceFactory;
            this.requestMonitors = new ConcurrentDictionary<IResponseThreadWaiter, IX2RequestMonitor>();
            this.requestResponseHandlers = new ConcurrentDictionary<Guid, Action<X2Response>>();
        }

        private void HandleResponse(X2Response x2Response)
        {
            if (this.requestResponseHandlers.ContainsKey(x2Response.RequestID))
            {
                var handler = this.requestResponseHandlers[x2Response.RequestID];
                handler(x2Response);
            }
        }

        public IX2RequestMonitor GetRequestMonitor(IResponseThreadWaiter threadWaiter)
        {
            if (requestMonitors.ContainsKey(threadWaiter))
                return requestMonitors[threadWaiter];
            return null;
        }

        public IX2RequestMonitor GetOrCreateRequestMonitor(IResponseThreadWaiter threadWaiter, IX2RequestMonitorCallback callback)
        {
            IX2RequestMonitor requestMonitor = GetRequestMonitor(threadWaiter);

            if (requestMonitor != null)
                return requestMonitor;

            requestMonitor = new X2RequestMonitor(threadWaiter, timeoutServiceFactory);
            requestMonitor.Initialise(callback);
            this.requestResponseHandlers.TryAdd(threadWaiter.CorrelationId, requestMonitor.HandleResponse);
            requestMonitors.TryAdd(threadWaiter, requestMonitor);
            return requestMonitor;
        }

        public void RemoveRequestMonitor(IResponseThreadWaiter threadWaiter)
        {
            IX2RequestMonitor requestMonitor = null;
            requestMonitors.TryRemove(threadWaiter, out requestMonitor);
            Action<X2Response> handler = null;
            requestResponseHandlers.TryRemove(threadWaiter.CorrelationId, out handler);
        }


        public void Initialise()
        {
            this.responseSubscriber.Subscribe<X2Response>(HandleResponse);
        }
    }
}