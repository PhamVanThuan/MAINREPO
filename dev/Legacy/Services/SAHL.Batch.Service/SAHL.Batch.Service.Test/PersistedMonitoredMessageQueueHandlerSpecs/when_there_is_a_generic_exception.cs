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
    public class when_there_is_a_generic_exception : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>> mock;
        private static CapitecApplicationMessage capitecApplicationMessage;
        private static int numberOfTimesToRetryToProcessTheMessage;
        

        Establish context = () =>
        {
            capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());
            capitecApplicationMessage.FailureCount = 0;
            mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>>();
            mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WhenToldTo(x => x.Process(capitecApplicationMessage)).Return(() =>
            {
                throw new NotSupportedException("This method is not supported.");
                return true;
            });
            numberOfTimesToRetryToProcessTheMessage = 3;
            mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.NumberOfTimesToRetryToProcessTheMessage).Return(numberOfTimesToRetryToProcessTheMessage);
        };

        Because of = () =>
        {
            mock.ClassUnderTest.HandleMessage(capitecApplicationMessage);
        };

        private It should_retry_as_many_times_as_specified_in_configuration = () =>
        {
            mock.Get<IMessageProcessor<CapitecApplicationMessage>>().WasToldTo(x => x.Process(capitecApplicationMessage)).Times(numberOfTimesToRetryToProcessTheMessage);
        };

        private It should_increment_the_number_of_attempts = () =>
        {
            capitecApplicationMessage.FailureCount.ShouldEqual<int>(1);
        };

        private It should_store_the_failed_message = () =>
        {
            mock.Get<IRepository>().WasToldTo(x => x.Update(capitecApplicationMessage, GenericStatuses.Failed));
        };
    }
}
