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
    public class when_message_handler_starts : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>> mock;
        private static int timeOutIntervalToReloadFailedMessages;
        private static Action action;

        Establish context = () =>
            {
                mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageQueueHandler<CapitecApplicationMessage>>();
                timeOutIntervalToReloadFailedMessages = 10;
                mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.TimeOutIntervalToReloadFailedMessages).Return(timeOutIntervalToReloadFailedMessages);
                action = () => {};
            };

        Because of = () =>
            {
                mock.ClassUnderTest.Start();
            };

        It should_start_the_timer = () =>
            {
                mock.Get<IMessageRetryService<CapitecApplicationMessage>>().WasToldTo(x => x.Start());
            };
    }
}
