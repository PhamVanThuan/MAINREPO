using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange.Provider;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Exchange.Specs
{
    public class when_querying_for_new_mail_and_has_no_mail : WithFakes
    {
        private static IExchangeMailboxHelper helper;
        private static IExchangeProvider exchangeProvider;

        private Establish that = () =>
        {
            exchangeProvider = An<IExchangeProvider>();
            exchangeProvider.WhenToldTo(x => x.OpenMailboxConnection()).Return(true);
            exchangeProvider.WhenToldTo(x => x.FetchAllMessagesFromInbox()).Return(new List<IMailMessage>());
        };

        private Because of = () =>
        {
            helper = new ExchangeMailboxHelper(exchangeProvider);
            helper.UpdateMessages();
        };

        private It should_read_from_the_mail_box = () =>
        {
            exchangeProvider.WasToldTo(x => x.FetchAllMessagesFromInbox());
        };

        private It should_not_have_any_message_available = () =>
        {
            helper.EmailMessages.Any().ShouldBeFalse();
        };
    }
}