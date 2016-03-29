using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Exchange.Specs.Fakes;
using System.Linq;

namespace SAHL.Core.Exchange.Specs
{
    public class when_querying_for_new_mail_and_has_single_mail_messages : WithFakes
    {
        public static IExchangeMailboxHelper helper;
        public static IExchangeProvider exchangeProvider;
        private static int expectedNumberOfEmails;

        private Establish that = () =>
        {
            exchangeProvider = An<IExchangeProvider>();
            exchangeProvider.WhenToldTo(x => x.OpenMailboxConnection()).Return(true);
            exchangeProvider.WhenToldTo(x => x.FetchAllMessagesFromInbox()).Return(FakeMailMessageFactory.ReturnSingleInValidMessage);
            expectedNumberOfEmails = FakeMailMessageFactory.ReturnSingleInValidMessage().Count;
        };

        private Because of = () =>
        {
            helper = new ExchangeMailboxHelper(exchangeProvider);
            helper.UpdateMessages();
        };

        private It should_have_the_correct_number_of_message_available = () =>
        {
            helper.EmailMessages.Count().ShouldEqual(expectedNumberOfEmails);
        };
    }
}