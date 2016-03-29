using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers.Mailbox;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class SendEmailCommandHandler : IServiceCommandHandler<SendEmailCommand>
    {
        private IMailboxManager mailboxManager;

        public SendEmailCommandHandler(IMailboxManager mailboxManager)
        {
            this.mailboxManager = mailboxManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(SendEmailCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            var open = this.mailboxManager.OpenMailboxConnection();
            if (open)
            {
                this.mailboxManager.SendMessage(command.MailMessage);
            }
            else
            {
                messages.AddMessage(new SystemMessage("Unable to open mailbox connection", SystemMessageSeverityEnum.Error));
            }
            return messages;
        }
    }
}