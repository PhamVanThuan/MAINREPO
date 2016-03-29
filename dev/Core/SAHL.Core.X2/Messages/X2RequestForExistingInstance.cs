using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2RequestForExistingInstance : X2Request, IX2RequestForExistingInstance
    {
        public string ActivityName { get; protected set; }

        public X2RequestForExistingInstance(Guid correlationID, Int64 instanceId, X2RequestType action,
                                            IServiceRequestMetadata serviceRequestMetadata, string activityName, 
                                            bool ignoreWarnings, Dictionary<string, string> mapVariables = null, object data = null)
            : base(correlationID, action, instanceId, ignoreWarnings, serviceRequestMetadata, mapVariables, data)
        {
            this.InstanceId = instanceId;
            this.ActivityName = activityName;
            this.ServiceRequestMetadata = serviceRequestMetadata;
        }
    }
}