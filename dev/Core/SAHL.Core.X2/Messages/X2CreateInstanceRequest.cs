using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Messages
{
    public class X2CreateInstanceRequest : X2Request, IX2CreateRequest
    {
        public X2CreateInstanceRequest(Guid correlationID, string activityName, string processName, string workflowName,
                                       IServiceRequestMetadata serviceRequestMetadata, bool ignoreWarnings, 
                                       int? ReturnActivityId = null, long? SourceInstanceId = null, 
                                       Dictionary<string, string> mapVariables = null, object data = null)
            : base(correlationID, X2RequestType.UserCreate, null, ignoreWarnings, serviceRequestMetadata, mapVariables, data)
        {
            this.ProcessName = processName;
            this.WorkflowName = workflowName;
            this.ActivityName = activityName;
            this.ReturnActivityId = ReturnActivityId;
            this.SourceInstanceId = SourceInstanceId;
        }

        public string ProcessName
        {
            get;
            protected set;
        }

        public string WorkflowName
        {
            get;
            protected set;
        }

        public int? ReturnActivityId
        {
            get;
            protected set;
        }

        public long? SourceInstanceId
        {
            get;
            protected set;
        }

        public string ActivityName
        {
            get;
            protected set;
        }
    }
}