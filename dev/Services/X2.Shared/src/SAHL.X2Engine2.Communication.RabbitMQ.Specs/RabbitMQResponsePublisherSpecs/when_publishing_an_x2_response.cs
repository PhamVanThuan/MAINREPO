using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.X2.Messages;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.RabbitMQResponsePublisherSpecs
{
    public class when_publishing_an_x2_response : WithFakes
    {
        private static AutoMocker<RabbitMQResponsePublisher> automocker = new NSubstituteAutoMocker<RabbitMQResponsePublisher>();
        private static X2RouteEndpoint endpoint;
        private static X2Response response;

        Establish context = () =>
        {
            endpoint = new X2RouteEndpoint("process","workflow");
            response = new X2Response(Guid.NewGuid(),"message",1);
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.Publish(endpoint, response);
        };

        It should_publish_the_x2_response = () =>
        {
            automocker.Get<IQueuePublisher>().WasToldTo(x => x.Publish(endpoint.ExchangeName, "#", response, true, "direct"));
        };
    }
}
