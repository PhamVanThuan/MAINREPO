using System;
using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Factories
{
    /// <summary>
    ///
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        private IWorkflowDataProvider workflowDataProvider = null;
        public string workflowProviderName;
        private Dictionary<X2RequestType, Func<IX2Request, IEnumerable<IServiceCommand>>> internalCommandFactory;

        public CommandFactory(IWorkflowDataProvider workflowDataProvider, IX2NodeConfigurationProvider nodeConfigurationProvider)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.workflowProviderName = nodeConfigurationProvider.GetExchangeName();
            internalCommandFactory = new Dictionary<X2RequestType, Func<IX2Request, IEnumerable<IServiceCommand>>>
            {
                { X2RequestType.UserCreate, GetUserRequestCreate },
                { X2RequestType.CreateComplete, GetUserCreateComplete },
                { X2RequestType.UserCreateWithComplete, GetUserRequestCreateWithComplete },
                { X2RequestType.UserStart, GetUserStart },
                { X2RequestType.UserCancel, GetUserCancel },
                { X2RequestType.UserComplete, GetUserComplete },
                { X2RequestType.SystemRequestGroup, GetSystemRequestGroup },
                { X2RequestType.Timer, GetTimerRequest },
                { X2RequestType.WorkflowActivity, GetWorkflowActivityRequest },
                { X2RequestType.External, GetExternalActivityRequest },
                { X2RequestType.AutoForward, GetAutoForwardRequest },
                { X2RequestType.SecurityRecalc, GetSecurityRecalc }
            };
        }

        public IEnumerable<IServiceCommand> GetSecurityRecalc(IX2Request request)
        {
            X2RequestForSecurityRecalc recalcRequest = request as X2RequestForSecurityRecalc;
            List<IServiceCommand> commands = new List<IServiceCommand>
            {
                new RebuildInstanceCommand(recalcRequest.InstanceId)
            };
            return commands;
        }

        public IEnumerable<IServiceCommand> GetTimerRequest(IX2Request request)
        {
            return GetSystemRequestGroup(request);
        }

        public IEnumerable<IServiceCommand> GetAutoForwardRequest(IX2Request request)
        {
            X2RequestForAutoForward autoFwd = request as X2RequestForAutoForward;
            List<IServiceCommand> commands = new List<IServiceCommand>
            {
                new HandleSystemRequestThatIsAnAutoForwardCommand(request.InstanceId, autoFwd.ServiceRequestMetadata.UserName)
            };
            return commands;
        }

        public IEnumerable<IServiceCommand> GetWorkflowActivityRequest(IX2Request request)
        {
            List<IServiceCommand> commands = new List<IServiceCommand>();
            X2WorkflowRequest workflowRequest = request as X2WorkflowRequest;
            WorkFlowActivityDataModel workflowActivityDataModel = workflowDataProvider.GetWorkflowActivityDataModelById(workflowRequest.WorkflowActivityId);
            WorkFlowDataModel currentflowDataModel = workflowDataProvider.GetWorkflowById(workflowActivityDataModel.WorkFlowID);
            WorkFlowDataModel nextWorkflowDataModel = workflowDataProvider.GetWorkflowById(workflowActivityDataModel.NextWorkFlowID);
            WorkflowActivity workflowActivity = new WorkflowActivity(workflowActivityDataModel.ID, currentflowDataModel.Name, nextWorkflowDataModel.Name, workflowActivityDataModel.NextActivityID, workflowActivityDataModel.ReturnActivityID, workflowActivityDataModel.Name);
            HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand command = new HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand(workflowRequest.InstanceId, workflowActivity, workflowRequest.ServiceRequestMetadata.UserName, workflowProviderName);
            commands.Add(command);
            return commands;
        }

        public IEnumerable<IServiceCommand> GetExternalActivityRequest(IX2Request request)
        {
            List<IServiceCommand> commands = new List<IServiceCommand>();
            X2ExternalActivityRequest extRequest = request as X2ExternalActivityRequest;
            ExternalActivityCommand externalActivityCommand = new ExternalActivityCommand(extRequest.ExternalActivityId, extRequest.WorkflowId, extRequest.ActivatingInstanceId, extRequest.ActivityTime, extRequest.MapVariables);
            DeleteActiveExternalActivityCommand deleteCommand = new DeleteActiveExternalActivityCommand(extRequest.ActiveExternalActivityId);
            commands.Add(externalActivityCommand);
            commands.Add(deleteCommand);
            return commands;
        }

        public IEnumerable<IServiceCommand> GetUserRequestCreate(IX2Request request)
        {
            // its a create
            X2CreateInstanceRequest createRequest = request as X2CreateInstanceRequest;
            if (createRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2CreateInstanceRequest message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityByNameAndWorkflowName(createRequest.ActivityName, createRequest.WorkflowName);

            UserRequestCreateInstanceCommand command = new UserRequestCreateInstanceCommand(createRequest.ProcessName, createRequest.WorkflowName, createRequest.ServiceRequestMetadata.UserName, activity, createRequest.MapVariables, workflowProviderName, request.IgnoreWarnings);

            return new List<IServiceCommand> { command };
        }

        public IEnumerable<IServiceCommand> GetUserRequestCreateWithComplete(IX2Request request)
        {
            // its a create
            X2CreateInstanceWithCompleteRequest createWithCompleteRequest = request as X2CreateInstanceWithCompleteRequest;
            if (createWithCompleteRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2CreateInstanceWithCompleteRequest message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityByNameAndWorkflowName(createWithCompleteRequest.ActivityName, createWithCompleteRequest.WorkflowName);

            UserRequestCreateInstanceWithCompleteCommand command = new UserRequestCreateInstanceWithCompleteCommand(createWithCompleteRequest.ProcessName, createWithCompleteRequest.WorkflowName, createWithCompleteRequest.ServiceRequestMetadata.UserName, activity, createWithCompleteRequest.MapVariables, workflowProviderName, request.IgnoreWarnings);

            return new List<IServiceCommand> { command };
        }

        public IEnumerable<IServiceCommand> GetUserStart(IX2Request request)
        {
            X2RequestForExistingInstance existingInstanceRequest = request as X2RequestForExistingInstance;
            if (existingInstanceRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2RequestForExistingInstance message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityForInstanceAndName(existingInstanceRequest.InstanceId, existingInstanceRequest.ActivityName);
            if (activity.SplitWorkflow)
            {
                IServiceCommand command = new UserRequestStartActivityWithSplitCommand(existingInstanceRequest.InstanceId, existingInstanceRequest.ServiceRequestMetadata.UserName, activity, existingInstanceRequest.MapVariables, workflowProviderName, request.IgnoreWarnings, request.Data);
                return new List<IServiceCommand> { command };
            }
            else
            {
                IServiceCommand command = new UserRequestStartActivityWithoutSplitCommand(existingInstanceRequest.InstanceId, existingInstanceRequest.ServiceRequestMetadata.UserName, activity, request.IgnoreWarnings, existingInstanceRequest.MapVariables, request.Data);
                return new List<IServiceCommand> { command };
            }
        }

        public IEnumerable<IServiceCommand> GetUserCreateComplete(IX2Request request)
        {
            X2RequestForExistingInstance existingInstanceRequest = request as X2RequestForExistingInstance;
            if (existingInstanceRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2RequestForExistingInstance message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityForInstanceAndName(existingInstanceRequest.InstanceId, existingInstanceRequest.ActivityName);
            IServiceCommand command = new UserRequestCompleteCreateCommand(existingInstanceRequest.InstanceId, activity, existingInstanceRequest.ServiceRequestMetadata.UserName, request.IgnoreWarnings, existingInstanceRequest.MapVariables, request.Data);
            return new List<IServiceCommand> { command };
        }

        public IEnumerable<IServiceCommand> GetUserComplete(IX2Request request)
        {
            X2RequestForExistingInstance existingInstanceRequest = request as X2RequestForExistingInstance;
            if (existingInstanceRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2RequestForExistingInstance message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityForInstanceAndName(existingInstanceRequest.InstanceId, existingInstanceRequest.ActivityName);
            IServiceCommand command = new UserRequestCompleteActivityCommand(existingInstanceRequest.InstanceId, activity, existingInstanceRequest.ServiceRequestMetadata.UserName, request.IgnoreWarnings, existingInstanceRequest.MapVariables, request.Data);
            return new List<IServiceCommand> { command };
        }

        public IEnumerable<IServiceCommand> GetUserCancel(IX2Request request)
        {
            X2RequestForExistingInstance existingInstanceRequest = request as X2RequestForExistingInstance;
            if (existingInstanceRequest == null)
            {
                throw new Exception(string.Format("The Request Message is not an X2RequestForExistingInstance message. CorrelationID: {0}", request.CorrelationId));
            }
            Activity activity = workflowDataProvider.GetActivityForInstanceAndName(existingInstanceRequest.InstanceId, existingInstanceRequest.ActivityName);
            IServiceCommand command = new UserRequestCancelActivityCommand(existingInstanceRequest.InstanceId, activity, request.ServiceRequestMetadata.UserName);
            return new List<IServiceCommand> { command };
        }

        public IEnumerable<IServiceCommand> GetSystemRequestGroup(IX2Request request)
        {
            X2SystemRequestGroup systemRequestGroup = request as X2SystemRequestGroup;
            if (systemRequestGroup == null)
            {
                throw new Exception(string.Format("The Request Message is not a SystemRequestGroup message: CorrelationID: {0}", request.CorrelationId));
            }
            List<IServiceCommand> commands = new List<IServiceCommand>();

            foreach (string activityName in systemRequestGroup.ActivityNames)
            {
                Activity activity = workflowDataProvider.GetActivityForInstanceAndName(systemRequestGroup.InstanceId, activityName);
                if (activity != null && activity.SplitWorkflow)
                {
                    commands.Add(new HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand(systemRequestGroup.InstanceId, activity, systemRequestGroup.ServiceRequestMetadata.UserName, workflowProviderName));
                }
                else if (activity != null && !activity.SplitWorkflow)
                {
                    commands.Add(new HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(systemRequestGroup.InstanceId, activity, systemRequestGroup.ServiceRequestMetadata.UserName));
                }
            }

            return commands;
        }

        public IEnumerable<IServiceCommand> CreateCommands(IX2Request request)
        {
            var commandCreator = internalCommandFactory[request.RequestType];
            return commandCreator(request);
        }
    }
}