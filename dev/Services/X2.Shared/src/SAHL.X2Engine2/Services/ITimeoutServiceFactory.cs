using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Services
{
    public interface ITimeoutServiceFactory
    {
        ITimeoutService Create(IX2Request request, IX2RequestMonitorCallback requestCallback, IResponseThreadWaiter threadWaiter);
    }
}