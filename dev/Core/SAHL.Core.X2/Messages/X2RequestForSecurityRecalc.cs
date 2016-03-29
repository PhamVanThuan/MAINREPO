using SAHL.Core.Services;
using System;

namespace SAHL.Core.X2.Messages
{
    public class X2RequestForSecurityRecalc : X2Request, IX2RequestForSecurityRecalc
    {
        public X2RequestForSecurityRecalc(Guid correlationID, long? instanceId, IServiceRequestMetadata serviceRequestMetadata)
            : base(correlationID, X2RequestType.SecurityRecalc, instanceId, true, serviceRequestMetadata)
        {
        }
    }
}