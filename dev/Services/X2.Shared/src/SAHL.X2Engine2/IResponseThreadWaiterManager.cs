using System;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    public interface IResponseThreadWaiterManager
    {
        IResponseThreadWaiter Create(IX2Request request);

        void Release(Guid requestID);

        IResponseThreadWaiter GetThreadWaiter(Guid requestID);
    }
}