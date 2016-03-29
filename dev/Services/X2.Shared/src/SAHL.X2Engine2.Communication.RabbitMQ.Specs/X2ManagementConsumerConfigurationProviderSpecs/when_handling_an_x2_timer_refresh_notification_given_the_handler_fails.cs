using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.MessageHandlers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Linq;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2ManagementConsumerConfigurationProviderSpecs
{
    public class when_handling_an_x2_response_given_the_handler_fails : WithFakes
    {
        private static AutoMocker<X2ManagementConsumerConfigurationProvider> automocker = new NSubstituteAutoMocker<X2ManagementConsumerConfigurationProvider>();
        private static IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest> handler;
        private static X2RouteEndpoint endpoint = new X2RouteEndpoint("exchange", "queue");

        private Establish context = () =>
        {
            handler = An<IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest>>();
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.Subscribe(endpoint, handler);
            automocker.ClassUnderTest.GetConsumers().First().WorkAction("{'name'/'bob'}");
        };

        private It should_log_the_error = () =>
        {
            automocker.Get<IRawLogger>().WasToldTo(x => x.LogErrorWithException(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Exception>(), null));
        };
    }
}