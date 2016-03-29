using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using System;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.RabbitMQRequestPublisherSpecs
{
    public class when_publishing_an_invalid_x2_request_type : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RabbitMQRequestPublisher> autoMocker;
        private static IX2Request request;
        private static Exception exception;

        private Establish context = () =>
        {
            request = new X2Request(Guid.NewGuid(), X2RequestType.UserCreateWithComplete, null, false, null);
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RabbitMQRequestPublisher>();
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                autoMocker.ClassUnderTest.Publish(Param.IsAny<IX2RouteEndpoint>(), request);
            });
        };

        private It should_thow_a_x2_publish_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(X2PublishException));
            exception.Message.ShouldEqual("You may not publish base x2 requests");
        };
    }
}