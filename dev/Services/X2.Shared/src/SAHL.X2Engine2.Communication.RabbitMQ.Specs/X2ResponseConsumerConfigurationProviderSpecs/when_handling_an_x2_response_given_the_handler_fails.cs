using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.X2.Messages;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Linq;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2ResponseConsumerConfigurationProviderSpecs
{
    public class when_handling_an_x2_response_given_the_handler_fails : WithFakes
    {
        private static AutoMocker<X2ResponseConsumerConfigurationProvider> automocker = new NSubstituteAutoMocker<X2ResponseConsumerConfigurationProvider>();
        private static Action<X2Response> responseHandler;

        private Establish context = () =>
        {
            responseHandler = (x) =>
            {
                var messageReceived = x;
            };
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.Subscribe(responseHandler);
            automocker.ClassUnderTest.GetConsumers().First().WorkAction("{'name'/'bob'}");
        };

        private It should_log_the_error = () =>
        {
            automocker.Get<IRawLogger>().WasToldTo(x => x.LogErrorWithException(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Exception>(), null));
        };
    }
}