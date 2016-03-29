using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using SAHL.DecisionTree.Shared;
using SAHL.DecisionTree.Shared.Core;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign
{
    public class CatchAllHubErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            dynamic debugSessionId = GetDebugSessionIdFromContext(invokerContext.Args[0]);
            if (exceptionContext.Error.InnerException != null)
            {
                invokerContext.Hub.Clients.Caller.onDebugError(debugSessionId, "=> Inner Exception " + exceptionContext.Error.InnerException.Message);
            }
            else
            {
                invokerContext.Hub.Clients.Caller.onDebugError(debugSessionId, "=> Exception " + exceptionContext.Error.Message);
            }
            TreeProcessingContext treeDebugContext;
            DecisionTreeDebugHub.connectedSessions.TryRemove(Guid.Parse(debugSessionId), out treeDebugContext);
            treeDebugContext = null;

            base.OnIncomingError(exceptionContext, invokerContext);
        }

        private dynamic GetDebugSessionIdFromContext(dynamic contextArgs)
        {
            IDictionary<string, JToken> contextArgsDictionary = JObject.Parse(contextArgs.ToString());
            dynamic result = null;
            if (contextArgsDictionary.ContainsKey("debugSessionId"))
            {
                result = contextArgs.debugSessionId;
            }
            else
            {
                result = contextArgs;
            }
            return result;
        }
    }
}