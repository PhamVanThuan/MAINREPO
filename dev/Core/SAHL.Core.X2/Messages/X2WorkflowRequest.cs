using SAHL.Core.Services;
using System;

namespace SAHL.Core.X2.Messages
{
    public class X2WorkflowRequest : X2Request, IX2SystemRequest, IX2Request, IX2Message
    {
        public X2WorkflowRequest(Guid corrolationId, IServiceRequestMetadata serviceRequestMetadata, X2RequestType requestType, long instanceId, int workflowActivityId, bool ignoreWarnings)
            : base(corrolationId, requestType, instanceId, ignoreWarnings, serviceRequestMetadata)
        {
            this.InstanceId = instanceId;
            this.ActivityTime = DateTime.Now;// fire now
            this.WorkflowActivityId = workflowActivityId;
        }

        public int WorkflowActivityId { get; protected set; }

        public DateTime ActivityTime
        {
            get;
            protected set;
        }
    }
}