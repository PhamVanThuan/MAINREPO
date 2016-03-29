using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Exchange.Specs.Fakes;
using System;
using System.Linq;

namespace SAHL.Core.Exchange.Specs
{
    public class when_archiving_a_single_mail : WithFakes
    {
        private static IExchangeMailboxHelper helper;
        private static IExchangeProvider exchangeProvider;
        private static IMailMessage mailMessage;
        private static string archiveFolder;

        private Establish that = () =>
        {
            archiveFolder = "Archive";
            mailMessage = FakeMailMessageFactory.ReturnSingleMessage().FirstOrDefault();
            mailMessage.From = "test@sahomeloans.com";
            mailMessage.To = "attorney@practice.co.za";
            mailMessage.Body = "Test body";

            exchangeProvider = An<IExchangeProvider>();
            exchangeProvider.WhenToldTo(x => x.OpenMailboxConnection()).Return(true);
            exchangeProvider.WhenToldTo(x => x.FetchAllMessagesFromInbox()).Return(FakeMailMessageFactory.ReturnSingleMessage);

            helper = new ExchangeMailboxHelper(exchangeProvider);
        };

        private Because of = () =>
        {
            helper.MoveMessage(archiveFolder, mailMessage);
        };

        private It should_move_given_email_to_given_folder = () =>
        {
            exchangeProvider.WasToldTo(e => e.MoveMailMessageToFolder(Param<IMailMessage>.Matches(
                  m => m.From.Equals(mailMessage.From, StringComparison.Ordinal)
                    && m.To.Equals(mailMessage.To, StringComparison.Ordinal)
                    && m.Subject.Equals(mailMessage.Subject, StringComparison.Ordinal)
                    && m.Body.Equals(mailMessage.Body, StringComparison.Ordinal)
                  )
                , Param<string>.Matches(m => m.Equals(archiveFolder, StringComparison.Ordinal))
                ));
        };
    }
}