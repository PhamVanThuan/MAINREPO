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
    public class when_a_message_has_been_received : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>> mock;
        private static CapitecApplicationMessage capitecApplicationMessage;

        Establish context = () =>
            {
                capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());
                mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>>();
                mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WhenToldTo(x => x.Process(capitecApplicationMessage)).Return(true);
                mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.NumberOfTimesToRetryToProcessTheMessage).Return(1);
            };

        Because of = () =>
            {   
                mock.ClassUnderTest.HandleMessage(new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp()));
            };

        It should_store_message = () =>
            {
                mock.Get<IRepository>().WasToldTo(x => x.Save(capitecApplicationMessage));
            };

        It should_process_the_message = () =>
            {
                mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WasToldTo(x => x.Process(capitecApplicationMessage));
            };

        It should_acknowlegde_the_message_as_sucessfully_processed = () =>
            {
                mock.Get<IRepository>().WasToldTo(x => x.Update(capitecApplicationMessage,GenericStatuses.Complete));
            };

        It should_reset_the_timer = () =>
            {
                mock.Get<IMessageRetryService<CapitecApplicationMessage>>().WasToldTo(x => x.Reset());
            };
    }
}
