using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2SystemRequestGroup : X2Request, IX2SystemRequest
    {
        public X2SystemRequestGroup(Guid corrolationId, IServiceRequestMetadata serviceRequestMetadata, X2RequestType requestType, 
            long instanceId, List<string> activityNames, DateTime? activityTime = null) :
            base(corrolationId, requestType, instanceId, true, serviceRequestMetadata)
        {
            this.ServiceRequestMetadata = serviceRequestMetadata;
            this.InstanceId = instanceId;
            this.ActivityNames = activityNames;
            if (activityTime == null)
            {
                activityTime = DateTime.Now;
            }
            this.ActivityTime = (DateTime) activityTime;
        }

        public List<string> ActivityNames { get; set; }

        public DateTime ActivityTime { get; protected set; }
    }
}