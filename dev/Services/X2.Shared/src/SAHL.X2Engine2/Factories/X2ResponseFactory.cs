using System;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Factories
{
    public class X2ResponseFactory : IX2ResponseFactory
    {
        public X2Response CreateErrorResponse(IX2Request request, string errorMessage, long? instanceId, SystemMessageCollection messages)
        {
            X2Response response = new X2ErrorResponse(request.CorrelationId, errorMessage, instanceId ?? 0, messages);
            return response;
        }

        public X2Response CreateSuccessResponse(IX2Request request, long instanceId, ISystemMessageCollection messages)
        {
            var response = new X2Response(request.CorrelationId, String.Empty, instanceId);

            return response;
        }
    }
}