using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Common;
using SAHL.Batch.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test.PersistedMonitoredMessageQueueHandlerSpecs
{
    public class when_there_is_domain_validation_exception : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>> mock;
        private static CapitecApplicationMessage capitecApplicationMessage;

        Establish context = () =>
        {
            capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());
            mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>>();
            mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WhenToldTo(x => x.Process(capitecApplicationMessage)).Return(false);
            mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.NumberOfTimesToRetryToProcessTheMessage).Return(1);
        };

        Because of = () =>
        {
            mock.ClassUnderTest.HandleMessage(new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp()));
        };

        It should_acknowlegde_the_message_as_unsuccessful = () =>
        {
            mock.Get<IRepository>().WasToldTo(x => x.Update(capitecApplicationMessage, GenericStatuses.Unsuccessful));
        };


    }
}
