using SAHL.Core.Services;
using System;

namespace SAHL.Core.X2.Messages
{
    public class X2RequestForAutoForward : X2Request, IX2SystemRequest, IX2Request, IX2Message
    {
        public X2RequestForAutoForward(Guid corrolationId, IServiceRequestMetadata serviceRequestMetadata, X2RequestType requestType, long instanceId)
            : base(corrolationId, requestType, instanceId, true, serviceRequestMetadata)
        {
            this.ServiceRequestMetadata = serviceRequestMetadata;
            this.InstanceId = instanceId;
            this.ActivityTime = DateTime.Now;// fire now
        }

        public int WorkflowActivityId { get; protected set; }

        public DateTime ActivityTime
        {
            get;
            protected set;
        }
    }
}