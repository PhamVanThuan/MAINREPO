using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.NotificationOfNewScheduledActivityCommandHandlerSpecs
{
    public class when_notifying_the_engine_of_a_new_scheduled_activity : WithFakes
    {
        static NotificationOfNewFutureScheduledActivityCommand command;
        private static AutoMocker<NotificationOfNewFutureScheduledActivityCommandHandler> automocker;
        private static long instanceId = 10;
        private static int activityId = 12;
        private static IX2RouteEndpoint engineRoute;
        private static X2Workflow workflow;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            workflow = new X2Workflow("process", "workflow");
            engineRoute = new X2RouteEndpoint("exchange", "queue");

            command = new NotificationOfNewFutureScheduledActivityCommand(instanceId, activityId);
            automocker = new NSubstituteAutoMocker<NotificationOfNewFutureScheduledActivityCommandHandler>();


            automocker.Get<IX2RequestInterrogator>().WhenToldTo(x => x.GetRequestWorkflow(Param.IsAny<IX2Request>())).Return(workflow);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };


        It should_publish_a_bundled_request_to_the_engine = () =>
        {
            automocker.Get<IX2RequestPublisher>().WasToldTo(x => x.Publish<X2NotificationOfNewScheduledActivityRequest>(Arg.Is<X2RouteEndpoint>(t => t.ExchangeName == X2QueueManager.X2EngineTimersRefreshExchange && t.QueueName == X2QueueManager.X2EngineTimersRefreshQueue)
                , Arg.Is<X2NotificationOfNewScheduledActivityRequest>(y =>
                y.InstanceId == y.InstanceId)));
        };
    }
}
