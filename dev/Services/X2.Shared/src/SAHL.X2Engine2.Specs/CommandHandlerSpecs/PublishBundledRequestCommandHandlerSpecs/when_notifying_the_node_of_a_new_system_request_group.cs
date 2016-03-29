using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Communication;
using System.Linq;
using System;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.PublishBundledRequestCommandHandlerSpecs
{
    internal class when_notifying_the_node_of_a_new_system_request_group : WithFakes
    {
        static PublishBundledRequestCommand command;
        static NotificationOfNewSystemRequestGroupCommand innerCommand;
        static List<IX2BundledNotificationCommand> commands = new List<IX2BundledNotificationCommand>();
        static AutoMocker<PublishBundledRequestCommandHandler> automocker;
        static long instanceId = 10;
        static object obj;
        private static IX2RouteEndpoint engineRoute;
        static ServiceRequestMetadata metadata;
        private static X2SystemRequestGroup x2SystemRequestGroup;
        private static List<string> activityNames;
        private static X2Workflow workflow;

        Establish context = () =>
        {
            workflow = new X2Workflow("process", "workflow");
            engineRoute = new X2RouteEndpoint("exchange", "queue");
            activityNames = new List<string>(new string[] { "Activity1", "Activity2" });
            innerCommand = new NotificationOfNewSystemRequestGroupCommand(activityNames, instanceId);
            x2SystemRequestGroup = new X2SystemRequestGroup(Guid.NewGuid(), metadata, X2RequestType.SystemRequestGroup, innerCommand.InstanceId, innerCommand.ActivityNames, DateTime.MinValue);
            commands.Add(innerCommand);
            command = new PublishBundledRequestCommand(commands);
            automocker = new NSubstituteAutoMocker<PublishBundledRequestCommandHandler>();
            automocker.Get<IRequestFactory>().WhenToldTo(x => x.CreateRequest(Arg.Is<NotificationOfNewSystemRequestGroupCommand>(c => c.InstanceId == instanceId && c.ActivityNames == activityNames))).Return(x2SystemRequestGroup);
            obj = automocker.Get<IX2ServiceCommandRouter>();

            automocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(Param.IsAny<IX2Request>())).Return(workflow);
            automocker.Get<IX2QueueNameBuilder>().WhenToldTo(x => x.GetSystemQueue(workflow)).Return(engineRoute);
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_get_request_for_the_command = () =>
        {
            automocker.Get<IRequestFactory>().WasToldTo(x => x.CreateRequest(Arg.Is<NotificationOfNewSystemRequestGroupCommand>(c => c.InstanceId == instanceId && c.ActivityNames == activityNames)));
        };

        It should_publish_a_bundled_request_to_the_engine = () =>
        {
            automocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish<X2BundledRequest>(engineRoute, Arg.Is<X2BundledRequest>(request =>
                request.Requests.First() == x2SystemRequestGroup)));
        };
    }
}