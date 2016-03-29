using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.PublishBundledRequestCommandHandlerSpecs
{
    public class when_notifying_the_node_of_a_bundled_request_given_a_collection_of_commands : WithFakes
    {

        private static PublishBundledRequestCommand command;
        private static NotificationOfNewScheduledActivityCommand notifyEngineOfNewScheduledActivityCommand;
        private static List<IX2BundledNotificationCommand> commands;
        private static AutoMocker<PublishBundledRequestCommandHandler> automocker;
        private static ServiceRequestMetadata metadata;
        private static long instanceId = 10;
        private static int activityId = 12;
        private static IX2RouteEndpoint engineRoute;
        private static X2NotificationOfNewScheduledActivityRequest  x2NotificationOfNewScheduledActivityRequest;
        private static X2Workflow workflow;

        Establish context = () =>
            {
                workflow = new X2Workflow("process", "workflow");
                engineRoute = new X2RouteEndpoint("exchange", "queue");

                notifyEngineOfNewScheduledActivityCommand = new NotificationOfNewScheduledActivityCommand(instanceId, activityId);
                commands = new List<IX2BundledNotificationCommand>();
                commands.Add(notifyEngineOfNewScheduledActivityCommand);
                command = new PublishBundledRequestCommand(commands);
                automocker = new NSubstituteAutoMocker<PublishBundledRequestCommandHandler>();
                x2NotificationOfNewScheduledActivityRequest = new X2NotificationOfNewScheduledActivityRequest(instanceId,activityId,null);
                automocker.Get<IRequestFactory>().WhenToldTo(x => x.CreateRequest(Arg.Is<NotificationOfNewScheduledActivityCommand>(c => c.InstanceId == instanceId && c.ActivityId == activityId))).Return(x2NotificationOfNewScheduledActivityRequest);

                automocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(Param.IsAny<IX2Request>())).Return(workflow);
                automocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetSystemQueue(workflow)).Return(engineRoute);

            };

        Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        It should_get_request_for_the_command = () =>
            {
                automocker.Get<IRequestFactory>().WasToldTo(x => x.CreateRequest(Arg.Is<NotificationOfNewScheduledActivityCommand>(c => c.InstanceId == instanceId && c.ActivityId == activityId)));
            };

        It should_publish_a_bundled_request_to_the_engine = () =>
        {
            automocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish<X2BundledRequest>(engineRoute, Arg.Is<X2BundledRequest>(request => 
                request.Requests.First() == x2NotificationOfNewScheduledActivityRequest)));
        };
    }
}
