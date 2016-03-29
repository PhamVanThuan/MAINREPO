using System;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    public class ResponseThreadWaiterManager : IResponseThreadWaiterManager
    {
        private System.Collections.Concurrent.ConcurrentDictionary<Guid, IResponseThreadWaiter> requestWaiters;

        public ResponseThreadWaiterManager()
        {
            requestWaiters = new System.Collections.Concurrent.ConcurrentDictionary<Guid, IResponseThreadWaiter>();
        }

        public IResponseThreadWaiter Create(IX2Request request)
        {
            var threadWaiter = new ResponseThreadWaiter(request.CorrelationId);
            requestWaiters.TryAdd(request.CorrelationId, threadWaiter);
            return threadWaiter;
        }

        public void Release(Guid correlationId)
        {
            IResponseThreadWaiter threadWaiter = null;
            requestWaiters.TryRemove(correlationId, out threadWaiter);
        }

        public IResponseThreadWaiter GetThreadWaiter(Guid requestID)
        {
            IResponseThreadWaiter threadWaiterToReturn = null;
            requestWaiters.TryGetValue(requestID, out threadWaiterToReturn);
            return threadWaiterToReturn;
        }
    }
}