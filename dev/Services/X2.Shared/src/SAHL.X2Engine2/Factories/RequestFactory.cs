using System;
using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Factories
{
    public class RequestFactory : IRequestFactory
    {
        private Dictionary<X2BundledNotificationCommandType, Func<IX2BundledNotificationCommand, IX2Request>> internalRequestFactory;
        private IWorkflowDataProvider workflowDataProvider;
        private IServiceRequestMetadata serviceRequestMetadata;

        public RequestFactory(IWorkflowDataProvider workflowDataProvider)
        {
            this.workflowDataProvider = workflowDataProvider;

            internalRequestFactory = new Dictionary<X2BundledNotificationCommandType, Func<IX2BundledNotificationCommand, IX2Request>>
            {
                {X2BundledNotificationCommandType.AutoForward,GetAutoForwardRequest},
                {X2BundledNotificationCommandType.WorkflowActivity,GetWorkflowActivityRequest},
                {X2BundledNotificationCommandType.SystemRequestGroup, GetSystemRequestGroupRequest},
                {X2BundledNotificationCommandType.ScheduledActivity, GetScheduledActivityRequest}
            };
            this.serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>()
                        {
                            { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                        });
        }

        private IX2Request GetAutoForwardRequest(IX2BundledNotificationCommand command)
        {
            var innerCommand = command as NotificationOfNewAutoForwardCommand;
            X2RequestForAutoForward request = new X2RequestForAutoForward(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.AutoForward, innerCommand.InstanceId);
            return request;
        }

        private IX2Request GetWorkflowActivityRequest(IX2BundledNotificationCommand command)
        {
            var innerCommand = command as NotificationOfNewWorkflowActivityCommand;
            X2WorkflowRequest request = new X2WorkflowRequest(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.WorkflowActivity, innerCommand.InstanceId, innerCommand.WorkflowActivityId, true);
            return request;
        }

        private IX2Request GetSystemRequestGroupRequest(IX2BundledNotificationCommand command)
        {
            var innerCommand = command as NotificationOfNewSystemRequestGroupCommand;
            X2SystemRequestGroup request = new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.SystemRequestGroup, innerCommand.InstanceId, innerCommand.ActivityNames, DateTime.MinValue);
            return request;
        }

        private IX2Request GetScheduledActivityRequest(IX2BundledNotificationCommand command)
        {
            var innerCommand = command as NotificationOfNewScheduledActivityCommand;
            ScheduledActivityDataModel scheduledActivityDataModel = workflowDataProvider.GetScheduledActivity(innerCommand.InstanceId, innerCommand.ActivityId);
            ActivityDataModel activityDataModel = workflowDataProvider.GetActivity(scheduledActivityDataModel.ActivityID);
            return new X2SystemRequestGroup(Guid.NewGuid(), serviceRequestMetadata, X2RequestType.Timer, scheduledActivityDataModel.InstanceID, new List<string> { activityDataModel.Name }, scheduledActivityDataModel.Time);
        }

        public IX2Request CreateRequest(IX2BundledNotificationCommand command)
        {
            if (command is IX2BundledNotificationCommand)
            {
                var x2EngineNotificationCommand = command as IX2BundledNotificationCommand;
                var requestCreator = internalRequestFactory[x2EngineNotificationCommand.CommandType];
                return requestCreator(command);
            }
            else
            {
                throw new NotImplementedException("invalid usage");
            }
        }
    }
}