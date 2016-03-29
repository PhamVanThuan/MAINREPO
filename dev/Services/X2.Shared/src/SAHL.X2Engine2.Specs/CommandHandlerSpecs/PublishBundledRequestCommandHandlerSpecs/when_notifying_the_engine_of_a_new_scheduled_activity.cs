using System.Linq;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.X2Engine2.Node.Providers;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Communication;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.PublishBundledRequestCommandHandlerSpecs
{
    public class when_notifying_the_engine_of_a_new_scheduled_activity : WithFakes
    {
        private static PublishBundledRequestCommand command;
        static NotificationOfNewScheduledActivityCommand innerCommand;
        static List<IX2BundledNotificationCommand> commands = new List<IX2BundledNotificationCommand>();
        private static AutoMocker<PublishBundledRequestCommandHandler> automocker;
        private static long instanceId = 10;
        private static int activityId = 12;
        private static IX2RouteEndpoint engineRoute;
        private static X2NotificationOfNewScheduledActivityRequest x2NotificationOfNewScheduledActivityRequest;
        private static X2Workflow workflow;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                workflow = new X2Workflow("process", "workflow");
                engineRoute = new X2RouteEndpoint("exchange", "queue");

                innerCommand = new NotificationOfNewScheduledActivityCommand(instanceId, activityId);
                commands.Add(innerCommand);
                command = new PublishBundledRequestCommand(commands);
                automocker = new NSubstituteAutoMocker<PublishBundledRequestCommandHandler>();
                x2NotificationOfNewScheduledActivityRequest = new X2NotificationOfNewScheduledActivityRequest(instanceId, activityId, null);
                automocker.Get<IRequestFactory>().WhenToldTo(x => x.CreateRequest(Arg.Is<NotificationOfNewScheduledActivityCommand>(c => c.InstanceId == instanceId && c.ActivityId == activityId))).Return(x2NotificationOfNewScheduledActivityRequest);

                automocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(Param.IsAny<IX2Request>())).Return(workflow);
                automocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetSystemQueue(workflow)).Return(engineRoute);
            };

        private Because of = () =>
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