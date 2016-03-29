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
    public class when_the_timer_expires : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageRetryService<CapitecApplicationMessage>> mock;
        private static int timeOutIntervalToReloadFailedMessages;
        private static int numberOfAttemptsToRetryToProcessTheMessage;
        private static List<CapitecApplicationMessage> messages;
        private static CapitecApplicationMessage message;
        private static Action action;

        Establish context = () =>
            {
                mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<MessageRetryService<CapitecApplicationMessage>>();
                timeOutIntervalToReloadFailedMessages = 1;
                numberOfAttemptsToRetryToProcessTheMessage = 3;
                mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.TimeOutIntervalToReloadFailedMessages).Return(timeOutIntervalToReloadFailedMessages);
                mock.Get<IBatchServiceConfiguration>().WhenToldTo(x => x.NumberOfAttemptsToRetryToProcessTheMessage).Return(numberOfAttemptsToRetryToProcessTheMessage);
                mock.Get<ITimer>().WhenToldTo(x => x.Start(Param.IsAny<int>(),Param.IsAny<Action>())).Callback<int,Action>((a,b) => {
                    mock.ClassUnderTest.RetryFailedMessages();
                });
                action = () => { };
                messages = new List<CapitecApplicationMessage>();
                message = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummySwitchLoanApp());
                messages.Add(message);
                mock.Get<IRepository>().WhenToldTo(x => x.Load<CapitecApplicationMessage>(GenericStatuses.Failed,numberOfAttemptsToRetryToProcessTheMessage)).Return(messages);
            };

        Because of = () =>
            {
                mock.ClassUnderTest.Start();
            };

        It should_retrieve_all_the_failed_messages = () =>
            {
                mock.Get<IRepository>().WasToldTo(x => x.Load<CapitecApplicationMessage>(GenericStatuses.Failed, numberOfAttemptsToRetryToProcessTheMessage));
            };

        It should_publish_all_the_failed_messages = () =>
            {
                mock.Get<IDiposableMessageBus>().WasToldTo(x => x.Publish(message)).Times(messages.Count);
            };

    }
}
