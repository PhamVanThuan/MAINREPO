using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SAHL.Core.Exchange.Specs.ExchangeProviderSpecs
{
    public class when_sending_an_email : WithExchangeProviderFakes
    {
        private static IMailMessage mailMessage;
        private static bool messageSent;
        private static IEnumerable<IMailMessage> mailMessages;

        private Establish context = () =>
            {

                exchangeProvider.OpenMailboxConnection();
                mailMessage = Fakes.FakeMailMessageFactory.ReturnSingleMessage().First();
                mailMessage.To = "halouser@sahomeloans.com";
                mailMessage.Subject = Guid.NewGuid().ToString();
            };

        private Because of = () =>
            {
                messageSent = exchangeProvider.SendMessage(mailMessage);
            };

        private It should_send_the_message = () =>
            {
                messageSent.ShouldBeTrue();
            };

        private It should_be_in_the_users_inbox = () =>
            {
                mailMessages = WaitForMessageToBeDelivered(mailMessage);
                mailMessages.Count().ShouldEqual(1);
            };

        private Cleanup move_all_messages_to_archive = () =>
            {
                MoveMessageToArchive(mailMessages.First());
            };
    }
}