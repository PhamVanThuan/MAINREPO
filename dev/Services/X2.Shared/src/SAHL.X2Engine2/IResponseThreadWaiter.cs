using SAHL.Core.X2.Messages;
using System;

namespace SAHL.X2Engine2
{
    public interface IResponseThreadWaiter
    {
        X2Response Response { get; }

        void Wait();

        void Continue(X2Response response);

        Guid CorrelationId { get;}
    }
}