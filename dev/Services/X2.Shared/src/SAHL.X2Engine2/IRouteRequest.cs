using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2
{
    public interface IRouteRequest
    {
        IResponseThreadWaiter RouteRequest<T>(T request) where T : class, IX2Request;
    }
}