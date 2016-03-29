using SAHL.Core.Services;
using System;

namespace SAHL.Core.X2
{
    [Serializable]
    public class X2Params : IX2Params
    {
        public X2Params(string activityName, string stateName, string workflowName, bool ignoreWarning, string adUserName, IServiceRequestMetadata serviceRequestMetadata, object data = null)
        {
            this.ActivityName = activityName;
            this.StateName = stateName;
            this.WorkflowName = workflowName;
            this.IgnoreWarning = ignoreWarning;
            this.ADUserName = adUserName;
            this.Data = data;
            this.ServiceRequestMetadata = serviceRequestMetadata;
        }

        public string ActivityName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public string StateName { get; protected set; }

        public string ADUserName { get; protected set; }

        public bool IgnoreWarning { get; protected set; }

        public object Data { get; protected set; }

        public IServiceRequestMetadata ServiceRequestMetadata { get; protected set; }
    }
}