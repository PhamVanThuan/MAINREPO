using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using SAHL.Common.Logging;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2.Framework.Logging
{
    public static class X2Logging
    {
        public const string INSTANCEID = "instanceid";
        public const string ADUSERNAME = "adusername";
        public const string WORKFLOWNAME = "workflowname";
        public const string WORKFLOWSTATE = "workflowstate";
        public const string WORKFLOWACTIVITY = "workflowactivity";

        public static void LogOnEnterX2Request(string methodName, Int64 instanceId, X2RequestBase request, string ADUser)
        {
            LogX2DebugInternal(LogLocationEnum.OnEnter, methodName, ADUser, instanceId, new Dictionary<string, object> { { "Request", request } });
        }

        public static void LogOnX2RequestSuccess(string methodName, Int64 instanceId, X2ResponseBase response)
        {
            LogX2DebugInternal(LogLocationEnum.OnComplete, methodName, "", instanceId, new Dictionary<string, object> { { "Reponse", response } });
        }

        public static void LogOnExitX2Request(string methodName, Int64 instanceId)
        {
            LogX2DebugInternal(LogLocationEnum.OnExit, methodName, "", instanceId, null);
        }

        public static void LogOnX2RequestException(string methodName, Int64 instanceId, Exception exception, X2ErrorResponse errorResponse)
        {
            LogX2ErrorInternal(LogLocationEnum.OnException, methodName, "", instanceId, exception, new Dictionary<string, object> { { "Response", errorResponse } });
        }

        public static void LogOnEnterWorkflow(string methodName, object workflowData, IX2InstanceData instanceData, IX2Params X2params)
        {
            LogOnEnterWorkflow(methodName, workflowData, instanceData, X2params, null);
        }

        public static void LogOnEnterWorkflow(string methodName, object workflowData, IX2InstanceData instanceData, IX2Params X2params, Dictionary<string, object> parameters)
        {
            AddToThreadParameters(WORKFLOWNAME, instanceData.WorkFlowName);
            AddToThreadParameters(WORKFLOWSTATE, instanceData.StateName);
            AddToThreadParameters(WORKFLOWACTIVITY, instanceData.ActivityName);
            LogX2DebugInternal(LogLocationEnum.OnEnter, methodName, "", instanceData.InstanceID, parameters);
        }

        public static void LogOnWorkflowSuccess(string methodName, object workflowData, IX2InstanceData instanceData, IX2Params X2params)
        {
            LogX2DebugInternal(LogLocationEnum.OnComplete, methodName, "", instanceData.InstanceID, null);
        }

        public static void LogOnExitWorkflow(string methodName, Int64 instanceId)
        {
            LogX2DebugInternal(LogLocationEnum.OnExit, methodName, "", instanceId, null);
            Logger.ThreadContext.Remove(WORKFLOWNAME);
            Logger.ThreadContext.Remove(WORKFLOWSTATE);
            Logger.ThreadContext.Remove(WORKFLOWACTIVITY);
        }

        public static void LogOnWorkflowException(string methodName, Int64 instanceId, Exception exception)
        {
            LogX2ErrorInternal(LogLocationEnum.OnException, methodName, "", instanceId, exception, null);
        }

        private static void LogX2DebugInternal(LogLocationEnum logLocation, string methodName, string ADUser, Int64 instanceId, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            string user = ADUser;
            if (string.IsNullOrEmpty(user))
            {
                IPrincipal principal = Thread.CurrentPrincipal;

                if (principal != null)
                {
                    if (principal.Identity != null)
                    {
                        user = principal.Identity.Name;
                    }
                }
            }

            AddToThreadParameters(INSTANCEID, instanceId);
            AddToThreadParameters(ADUSERNAME, user);
            AddToThreadParameters(Logger.LOGLOCATION, logLocation);

            LogPlugin.Logger.LogDebugMessage(methodName, "X2Debug", parameters);
        }

        private static void LogX2ErrorInternal(LogLocationEnum logLocation, string methodName, string ADUser, Int64 instanceId, Exception exception, Dictionary<string, object> parameters)
        {
            string user = ADUser;
            if (string.IsNullOrEmpty(user))
            {
                IPrincipal principal = Thread.CurrentPrincipal;

                if (principal != null)
                {
                    if (principal.Identity != null)
                    {
                        user = principal.Identity.Name;
                    }
                }
            }

            AddToThreadParameters(INSTANCEID, instanceId);
            AddToThreadParameters(ADUSERNAME, user);
            AddToThreadParameters(Logger.LOGLOCATION, logLocation);

            LogPlugin.Logger.LogErrorMessageWithException(methodName, "X2Error", exception, parameters);
        }

        private static void AddToParameters(Dictionary<string, object> parameters, string key, object value)
        {
            if (parameters.ContainsKey(key))
            {
                parameters[key] = value;
            }
            else
            {
                parameters.Add(key, value);
            }
        }

        private static void AddToThreadParameters(string key, object value)
        {
            Dictionary<string, object> parameters = Logger.ThreadContext;
            if (parameters != null)
            {
                if (parameters.ContainsKey(key))
                {
                    parameters[key] = value;
                }
                else
                {
                    parameters.Add(key, value);
                }
            }
        }
    }
}