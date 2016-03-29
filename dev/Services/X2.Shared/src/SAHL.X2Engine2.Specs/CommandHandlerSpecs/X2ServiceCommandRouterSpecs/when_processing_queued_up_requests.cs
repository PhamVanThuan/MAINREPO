using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using System.Collections.Generic;
namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs
{
    public class when_processing_queued_up_requests : WithFakes
    {
        private static X2ServiceCommandRouter typeUnderTest;
        private static NotifyEngineOfNewScheduledActivityCommand notifyEngineOfNewScheduledActivityCommand;
        private static NotifyEngineOfNewWorkflowActivityCommand notifyEngineOfNewWorkflowActivityCommand;
        private static IX2ServiceCommandRouter commandHandler;
        private static IServiceCommandHandlerProvider commandHandlerProvider;
        private static long instanceId = 12;
        private static int activityId = 9;
        private static int workflowActivity = 10;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            commandHandler = An<IX2ServiceCommandRouter>();
            commandHandlerProvider = An<IServiceCommandHandlerProvider>();
            typeUnderTest = new X2ServiceCommandRouter(commandHandlerProvider, commandHandler);
            serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "Username" }
                            });
            notifyEngineOfNewScheduledActivityCommand = new NotifyEngineOfNewScheduledActivityCommand(instanceId, activityId, false);
            notifyEngineOfNewWorkflowActivityCommand = new NotifyEngineOfNewWorkflowActivityCommand(instanceId, workflowActivity);
            typeUnderTest.QueueUpCommandToBeProcessed(notifyEngineOfNewScheduledActivityCommand);
            typeUnderTest.QueueUpCommandToBeProcessed(notifyEngineOfNewWorkflowActivityCommand);
        };

        private Because of = () =>
        {
            typeUnderTest.ProcessQueuedCommands(serviceRequestMetadata);
        };

        private It should_ = () =>
        {
            commandHandler.WasToldTo(x => x.HandleCommand(Arg.Any<NotifyX2EngineOfNewCommandsCommand>(), serviceRequestMetadata));
        };
    }

}
