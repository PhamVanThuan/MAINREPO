using System;
using System.Collections.Generic;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.PollingManager;

namespace SAHL.Services.PollingManager.Helpers
{
    public class WorkFlowHelper : IWorkFlowHelper
    {

        public X2CreateInstanceRequest CreateInstanceRequest(Guid correlationId, Dictionary<string, string> inputs, 
            string workFlowActivity, string workflowProcess, string workFlowName, string userName)
        {
            return new X2CreateInstanceRequest(correlationId,
                workFlowActivity,   // "Create Instance"
                workflowProcess,          // "Origination",
                workFlowName,       // "Application Capture",
                userName, // Dont set a consultant to assign to.
                false,
                null,
                null,
                inputs,
                null);
        }

        public X2RequestForExistingInstance RequestForExistingInstance(Guid correlationId, long instanceId, Dictionary<string, string> inputs, 
            X2RequestType x2RequestType, string userName, string workFlowActivity)
        {
            return new X2RequestForExistingInstance(correlationId,
                instanceId,
                x2RequestType,
                userName,
                workFlowActivity,
                false,
                inputs);
        }


    }

}