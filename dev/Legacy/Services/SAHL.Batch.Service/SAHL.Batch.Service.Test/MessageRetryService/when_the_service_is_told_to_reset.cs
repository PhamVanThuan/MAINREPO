using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Common;
using SAHL.Batch.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test.MessageRetryService
{
    public class when_the_service_is_told_to_reset : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageRetryService<CapitecApplicationMessage>> mock;
        private static int timeOutIntervalToReloadFailedMessages;
        private static List<CapitecApplicationMessage> messages;
        private static CapitecApplicationMessage message;
        private static Action action;

        private Establish context = () =>
        {
            mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageRetryService<CapitecApplicationMessage>>();
            timeOutIntervalToReloadFailedMessages = 1;
            mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.TimeOutIntervalToReloadFailedMessages).Return(timeOutIntervalToReloadFailedMessages);
            mock.Get<ITimer>().WhenToldTo(x => x.Start(Param.IsAny<int>(), Param.IsAny<Action>())).Callback<int, Action>((a, b) =>
            {
                mock.ClassUnderTest.RetryFailedMessages();
            });
            action = () => { };
            messages = new List<CapitecApplicationMessage>();
            message = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummySwitchLoanApp());
            messages.Add(message);
        };

        private Because of = () =>
        {
            mock.ClassUnderTest.Reset();
        };

        private It should_reset_the_timer = () =>
        {
            mock.Get<ITimer>().WasToldTo(x => x.Reset());
        };
}
}