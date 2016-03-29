using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers.Mailbox;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_sending_a_mail : WithFakes
    {
        private static IMailboxManager MailboxManager;
        private static MailMessage mailMessage;
        private static SendEmailCommand command;
        private static SendEmailCommandHandler commandhandler;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metaData;
        private static bool openConnection;

        private Establish context = () =>
        {
            MailboxManager = An<IMailboxManager>();
            messages = An<ISystemMessageCollection>();
            metaData = An<IServiceRequestMetadata>();
            mailMessage = new MailMessage();
            command = new SendEmailCommand(mailMessage);
            commandhandler = new SendEmailCommandHandler(MailboxManager);
            openConnection = false;
        };

        private Because of = () =>
        {
            messages = commandhandler.HandleCommand(command, metaData);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_send_mails_if_the_connection_is_closed = () =>
        {
            MailboxManager.WasNotToldTo(x => x.SendMessage(command.MailMessage));
        };

        private It should_return_error_messages_if_the_connection_isnt_open = () =>
        {
            messages.AllMessages.Contains(new SystemMessage("Unable to open mailbox connection", SystemMessageSeverityEnum.Error));
        };
    }
}