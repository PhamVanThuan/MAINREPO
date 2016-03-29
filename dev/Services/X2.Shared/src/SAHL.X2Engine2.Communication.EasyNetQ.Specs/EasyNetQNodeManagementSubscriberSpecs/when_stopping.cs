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
    public class when_stopping : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementSubscriber> autoMocker;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<EasyNetQNodeManagementSubscriber>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.Teardown();
        };

        private It should_ask_the_message_bus_to_subscribe = () =>
        {
            autoMocker.Get<IMessageBusAdvanced>().WasToldTo(x => x.Dispose());
        };
    }
}
