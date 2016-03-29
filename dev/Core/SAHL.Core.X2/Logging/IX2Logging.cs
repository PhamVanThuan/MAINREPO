using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Logging
{
    public interface IX2Logging
    {
        void LogOnEnterWorkflow(string methodName, object workflowData, Data.Models.X2.InstanceDataModel instanceData, IX2Params x2Params);

        void LogOnEnterWorkflow(string methodName, object workflowData, Data.Models.X2.InstanceDataModel instanceData, IX2Params x2Params,
                                IDictionary<string, object> parameters);

        void LogOnEnterX2Request(string methodName, long instanceId, SAHL.Core.X2.Messages.X2Request request, string ADUser);

        void LogOnExitWorkflow(string methodName, long instanceId);

        void LogOnExitX2Request(string methodName, long instanceId);

        void LogOnWorkflowException(string methodName, long instanceId, Exception exception);

        void LogOnWorkflowSuccess(string methodName, object workflowData, Data.Models.X2.InstanceDataModel instanceData, IX2Params x2Params);

        void LogOnX2RequestException(string methodName, long instanceId, Exception exception, Messages.X2ErrorResponse errorResponse);

        void LogOnX2RequestSuccess(string methodName, long instanceId, Messages.X2Response response);
    }
}