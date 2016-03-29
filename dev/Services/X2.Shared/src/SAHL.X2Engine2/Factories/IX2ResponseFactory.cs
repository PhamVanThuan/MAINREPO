using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Factories
{
    public interface IX2ResponseFactory
    {
        X2Response CreateErrorResponse(IX2Request request, string errorMessage, long? instanceId, SystemMessageCollection messages);

        X2Response CreateSuccessResponse(IX2Request request, long instanceId, ISystemMessageCollection messages);
    }
}