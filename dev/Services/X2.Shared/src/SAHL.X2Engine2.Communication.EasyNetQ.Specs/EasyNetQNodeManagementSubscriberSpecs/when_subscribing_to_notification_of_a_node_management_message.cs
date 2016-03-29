using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.EasyNetQ.Specs.EasyNetQNodeManagementSubscriberSpecs
{
    public class when_subscribing_to_notification_of_a_node_management_message : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementSubscriber> autoMocker;
        private static InMemoryMessageBus messageBus;
        private static X2NodeManagementMessage message;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementSubscriber>();
            messageBus = new InMemoryMessageBus();
            autoMocker.Inject<IMessageBusAdvanced>(messageBus);
            message = new X2NodeManagementMessage(Core.X2.Messages.X2ManagementType.RefreshCache,null);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Initialise();
            messageBus.Publish(message);
        };

        private It should_process_the_message = () =>
        {
            autoMocker.Get<IX2NodeManagementMessageProcessor>().WasToldTo(x => x.ProcessMessage(message));
        };
    }
}
