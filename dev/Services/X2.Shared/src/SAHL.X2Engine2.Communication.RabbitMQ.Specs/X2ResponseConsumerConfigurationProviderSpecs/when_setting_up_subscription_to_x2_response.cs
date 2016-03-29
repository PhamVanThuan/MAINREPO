using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2ResponseConsumerConfigurationProviderSpecs
{
    public class when_setting_up_subscription_to_x2_response : WithFakes
    {
        private static AutoMocker<X2ResponseConsumerConfigurationProvider> automocker = new NSubstituteAutoMocker<X2ResponseConsumerConfigurationProvider>();
        private static Action<X2Response> responseHandler;

        Establish context = () =>
        {
            responseHandler = (x) => 
            {
               var messageReceived = x;
            };
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.Subscribe(responseHandler);
        };

        It should_set_up_the_subscriptions = () =>
        {
            automocker.ClassUnderTest.GetConsumers().Count.ShouldEqual(1);
        };
    }
}
