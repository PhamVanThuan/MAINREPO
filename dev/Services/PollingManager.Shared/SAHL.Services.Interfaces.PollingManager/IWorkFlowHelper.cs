using System;
using System.Collections.Generic;
using SAHL.Core.X2.Messages;

namespace SAHL.Services.Interfaces.PollingManager
{
    public interface IWorkFlowHelper
    {
        X2CreateInstanceRequest CreateInstanceRequest(Guid correlationId, Dictionary<string, string> inputs,
            string workFlowActivity, string workflowProcess, string workFlowName, string userName);

        X2RequestForExistingInstance RequestForExistingInstance(Guid correlationId, long instanceId,
            Dictionary<string, string> inputs, X2RequestType x2RequestType, string userName, string workFlowActivity);
    }
}