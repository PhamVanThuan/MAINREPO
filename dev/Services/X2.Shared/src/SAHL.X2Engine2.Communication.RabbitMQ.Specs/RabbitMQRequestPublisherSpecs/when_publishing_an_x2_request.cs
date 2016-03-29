using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.X2.Messages;
using System;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.RabbitMQRequestPublisherSpecs
{
    public class when_publishing_an_x2_request : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RabbitMQRequestPublisher> autoMocker;
        private static IX2Request request;
        private static IX2RouteEndpoint routeEndpoint;

        private Establish context = () =>
        {
            routeEndpoint = new X2RouteEndpoint("ProcessName", "WorkflowName");
            request = new X2WorkflowRequest(Guid.NewGuid(), null, X2RequestType.UserCreateWithComplete, 1, 1, false);
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RabbitMQRequestPublisher>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Publish(routeEndpoint, request);
        };

        private It should_publish_a_x2_wrapped_request = () =>
        {
            autoMocker.Get<IQueuePublisher>().WasToldTo(x => x.Publish<X2WrappedRequest>(Param.IsAny<string>(), "#", Param.IsAny<X2WrappedRequest>(), true, ExchangeType.Direct));
        };
    }
}