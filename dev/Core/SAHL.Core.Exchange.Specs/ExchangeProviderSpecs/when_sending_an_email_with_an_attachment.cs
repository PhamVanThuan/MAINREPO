using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Exchange.Specs.ExchangeProviderSpecs
{
    public class when_sending_an_email_with_an_attachment : WithExchangeProviderFakes
    {
        private static IMailMessage mailMessage;
        private static bool messageSent;
        private static string expectedAttachmentName;
        private static IEnumerable<IMailMessage> mailMessages;

        private Establish context = () =>
        {
            exchangeProvider.OpenMailboxConnection();
            mailMessage = Fakes.FakeMailMessageFactory.ReturnSingleValidMessage().First();
            mailMessage.To = "halouser@sahomeloans.com";
            mailMessage.Subject = Guid.NewGuid().ToString();
            expectedAttachmentName = string.Format("{0}.{1}", mailMessage.Attachments.First().AttachmentName, mailMessage.Attachments.First().AttachmentType);
        };

        private Because of = () =>
        {
            messageSent = exchangeProvider.SendMessage(mailMessage);
        };

        private It should_return_true = () =>
        {
            messageSent.ShouldBeTrue();
        };

        private It should_send_the_email_with_the_attachment = () =>
        {
            mailMessages = WaitForMessageToBeDelivered(mailMessage);
            mailMessages.First().Attachments.First().AttachmentName.ShouldEqual(expectedAttachmentName);
        };

        private Cleanup move_all_messages_to_archive = () =>
        {
            MoveMessageToArchive(mailMessages.First());
        };
    }
}