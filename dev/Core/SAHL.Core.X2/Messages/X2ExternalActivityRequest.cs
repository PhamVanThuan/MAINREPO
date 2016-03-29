using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2ExternalActivityRequest : X2Request, IX2ExternalActivityRequest
    {
        public X2ExternalActivityRequest(Guid correlationId, long instanceId, int activeExternalActivityID, int externalActivityID, long? activatingInstanceID, int workflowId,
            IServiceRequestMetadata serviceRequestMetadata, Dictionary<string, string> mapVariables = null)
            : base(correlationId, X2RequestType.External, instanceId, false, serviceRequestMetadata, mapVariables, null)
        {
            this.ActiveExternalActivityId = activeExternalActivityID;
            this.ExternalActivityId = externalActivityID;
            this.ActivatingInstanceId = activatingInstanceID == null ? 0 : (long)activatingInstanceID;
            this.ActivityTime = DateTime.Now;
            this.WorkflowId = workflowId;
        }

        public int ActiveExternalActivityId { get; set; }

        public long? ActivatingInstanceId { get; set; }

        public int ExternalActivityId { get; set; }

        public DateTime ActivityTime { get; set; }

        public int WorkflowId { get; set; }
    }
}