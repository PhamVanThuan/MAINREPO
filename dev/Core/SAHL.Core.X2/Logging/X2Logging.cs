using SAHL.Core.Data.Models.X2;
using SAHL.Core.Logging;
using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace SAHL.Core.X2.Logging
{
    public class X2Logging : IX2Logging
    {
        public const string InstanceId = "InstanceId";
        public const string AdUsername = "ADUserName";
        public const string WorkflowName = "WorkflowName";
        public const string WorkflowState = "WorkflowState";
        public const string WorkflowActivity = "WorkflowActivity";
        public const string Request = "Request";
        public const string Response = "Response";
        private readonly IRawLogger logger;
        private readonly ILoggerAppSource loggerAppSource;
        private readonly ILoggerSource loggerSource;

        public X2Logging(IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public void LogOnEnterX2Request(string methodName, Int64 instanceId, X2Request request, string adUser)
        {
            LogX2DebugInternal(LogLocationEnum.OnEnter, methodName, adUser, instanceId, new Dictionary<string, object> { { Request, request } });
        }

        public void LogOnX2RequestSuccess(string methodName, Int64 instanceId, X2Response response)
        {
            LogX2DebugInternal(LogLocationEnum.OnComplete, methodName, GetCurrentUser(), instanceId, new Dictionary<string, object> { { Response, response } });
        }

        public void LogOnExitX2Request(string methodName, Int64 instanceId)
        {
            LogX2DebugInternal(LogLocationEnum.OnExit, methodName, GetCurrentUser(), instanceId, CopyThreadParameters());
        }

        public void LogOnX2RequestException(string methodName, Int64 instanceId, Exception exception, X2ErrorResponse errorResponse)
        {
            LogX2ErrorInternal(LogLocationEnum.OnException, methodName, GetCurrentUser(), instanceId, exception, new Dictionary<string, object> { { Response, errorResponse } });
        }

        public void LogOnEnterWorkflow(string methodName, object workflowData, InstanceDataModel instanceData, IX2Params x2Params)
        {
            LogOnEnterWorkflow(methodName, workflowData, instanceData, x2Params, CopyThreadParameters());
        }

        public void LogOnEnterWorkflow(string methodName, object workflowData, InstanceDataModel instanceData, IX2Params x2Params, IDictionary<string, object> parameters)
        {
            AddToThreadParameters(WorkflowName, x2Params.WorkflowName);
            AddToThreadParameters(WorkflowState, x2Params.StateName);
            AddToThreadParameters(WorkflowActivity, x2Params.ActivityName);
            LogX2DebugInternal(LogLocationEnum.OnEnter, methodName, GetCurrentUser(), instanceData.ID, parameters);
        }

        public void LogOnWorkflowSuccess(string methodName, object workflowData, InstanceDataModel instanceData, IX2Params x2Params)
        {
            LogX2DebugInternal(LogLocationEnum.OnComplete, methodName, GetCurrentUser(), instanceData.ID, CopyThreadParameters());
        }

        public void LogOnExitWorkflow(string methodName, Int64 instanceId)
        {
            LogX2DebugInternal(LogLocationEnum.OnExit, methodName, GetCurrentUser(), instanceId, CopyThreadParameters());
            Logger.ThreadContext.Remove(WorkflowName);
            Logger.ThreadContext.Remove(WorkflowState);
            Logger.ThreadContext.Remove(WorkflowActivity);
        }

        public void LogOnWorkflowException(string methodName, Int64 instanceId, Exception exception)
        {
            LogX2ErrorInternal(LogLocationEnum.OnException, methodName, GetCurrentUser(), instanceId, exception, CopyThreadParameters());
        }

        private void LogX2DebugInternal(LogLocationEnum logLocation, string methodName, string adUser, Int64 instanceId, IDictionary<string, object> parameters)
        {
            var nonNullParameters = parameters ?? new Dictionary<string, object>();

            string user = adUser;
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

            AddToThreadParameters(InstanceId, instanceId);
            AddToThreadParameters(AdUsername, user);
            AddToThreadParameters(Logger.LogLocation, logLocation);

            logger.LogDebug(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, user, methodName, "", nonNullParameters);
        }

        private void LogX2ErrorInternal(LogLocationEnum logLocation, string methodName, string adUser, Int64 instanceId, Exception exception, IDictionary<string, object> parameters)
        {
            string user = adUser;
            if (string.IsNullOrEmpty(user))
            {
                user = GetCurrentUser();
            }

            AddToThreadParameters(InstanceId, instanceId);
            AddToThreadParameters(AdUsername, user);
            AddToThreadParameters(Logger.LogLocation, logLocation);

            logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, user, methodName, "", exception, parameters);
        }

        private void AddToThreadParameters(string key, object value)
        {
            IDictionary<string, object> parameters = Logger.ThreadContext;
            if (parameters == null)
            {
                return;
            }
            if (parameters.ContainsKey(key))
            {
                parameters[key] = value;
            }
            else
            {
                parameters.Add(key, value);
            }
        }

        private IDictionary<string, object> CopyThreadParameters()
        {
            return Logger.ThreadContext.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private string GetCurrentUser()
        {
            string user = String.Empty;
            IPrincipal principal = Thread.CurrentPrincipal;

            if (principal == null)
            {
                return user;
            }
            if (principal.Identity != null)
            {
                user = principal.Identity.Name;
            }
            return user;
        }
    }
}