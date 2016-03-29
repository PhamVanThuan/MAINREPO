using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Exchange.Specs.Fakes;
using System;
using System.Linq;

namespace SAHL.Core.Exchange.Specs
{
    public class when_sending_an_email : WithFakes
    {
        private static IExchangeMailboxHelper helper;
        private static IExchangeProvider exchangeProvider;
        private static IMailMessage mailMessage;

        private Establish that = () =>
        {
            mailMessage = FakeMailMessageFactory.ReturnSingleMessage().FirstOrDefault();
            mailMessage.From = "test@sahomeloans.com";
            mailMessage.To = "attorney@practice.co.za";
            mailMessage.Body = "Test body";

            exchangeProvider = An<IExchangeProvider>();
            exchangeProvider.WhenToldTo(x => x.OpenMailboxConnection()).Return(true);
            exchangeProvider.WhenToldTo(x => x.FetchAllMessagesFromInbox()).Return(FakeMailMessageFactory.ReturnSingleMessage);
            exchangeProvider.WhenToldTo(x => x.SendMessage(Param.IsAny<IMailMessage>())).Return(true);

            helper = new ExchangeMailboxHelper(exchangeProvider);
        };

        private Because of = () =>
        {
            helper.SendMessage(mailMessage);
        };

        private It should_send_given_email = () =>
        {
            exchangeProvider.WasToldTo(e => e.SendMessage(Param<IMailMessage>.Matches(
                  m => m.From.Equals(mailMessage.From, StringComparison.Ordinal)
                    && m.To.Equals(mailMessage.To, StringComparison.Ordinal)
                    && m.Subject.Equals(mailMessage.Subject, StringComparison.Ordinal)
                    && m.Body.Equals(mailMessage.Body, StringComparison.Ordinal)
                )));
        };
    }
}