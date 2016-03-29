using System.Threading;
using SAHL.Core.X2.Messages;
using System;

namespace SAHL.X2Engine2
{
    public class ResponseThreadWaiter : IResponseThreadWaiter
    {
        private ManualResetEvent resetEvent;

        public Guid CorrelationId { get; protected set; }

        public X2Response Response
        {
            get;
            private set;
        }

        public ResponseThreadWaiter(Guid correlationId)
        {
            this.CorrelationId = correlationId;
            resetEvent = new ManualResetEvent(false);
        }

        public void Wait()
        {
            resetEvent.WaitOne();
        }

        public void Continue(X2Response response)
        {
            this.Response = response;
            resetEvent.Set();
        }
    }
}