using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication.EasyNetQ.Specs.EasyNetQNodeManagementPublisherSpecs
{
    public class when_publishing : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementPublisher> autoMocker;
        private static IX2NodeManagementMessage message;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementPublisher>();
            message = An<IX2NodeManagementMessage>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Publish(message);
        };

        private It should_ask_message_bus_to_publish_message = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.Publish(message));
        };
    }
}