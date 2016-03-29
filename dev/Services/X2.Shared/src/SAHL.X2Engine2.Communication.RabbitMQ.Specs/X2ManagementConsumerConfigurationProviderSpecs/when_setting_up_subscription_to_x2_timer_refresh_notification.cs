using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.MessageHandlers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.RabbitMQ.Specs.X2ManagementConsumerConfigurationProviderSpecs
{
    public class when_setting_up_subscription_to_x2_response : WithFakes
    {
        private static AutoMocker<X2ManagementConsumerConfigurationProvider> automocker = new NSubstituteAutoMocker<X2ManagementConsumerConfigurationProvider>();
        private static IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest> handler;
        private static X2RouteEndpoint endpoint = new X2RouteEndpoint("exchange", "queue");

        Establish context = () =>
        {
            handler = An<IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest>>();
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.Subscribe(endpoint, handler);
        };

        It should_set_up_the_subscriptions = () =>
        {
            automocker.ClassUnderTest.GetConsumers().Count.ShouldEqual(1);
        };
    }
}
