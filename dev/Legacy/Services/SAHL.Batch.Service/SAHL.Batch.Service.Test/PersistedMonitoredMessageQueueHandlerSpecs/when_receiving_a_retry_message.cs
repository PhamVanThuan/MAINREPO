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
    public class when_receiving_a_retry_message : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>> mock;
        private static CapitecApplicationMessage capitecApplicationMessage;

        Establish context = () =>
        {
            capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());
            capitecApplicationMessage.Id = 1;
            mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>>();
            mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WhenToldTo(x => x.Process(capitecApplicationMessage)).Return(true);
            mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.NumberOfTimesToRetryToProcessTheMessage).Return(1);
        };

        Because of = () =>
        {
            mock.ClassUnderTest.HandleMessage(capitecApplicationMessage);
        };

        It should_not_save_the_message = () =>
        {
            mock.Get<IRepository>().WasNotToldTo(x => x.Save(capitecApplicationMessage));
        };

    }
}
