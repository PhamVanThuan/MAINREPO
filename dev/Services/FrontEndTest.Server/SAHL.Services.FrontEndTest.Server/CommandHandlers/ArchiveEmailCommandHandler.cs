using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.Managers.Mailbox;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class ArchiveEmailCommandHandler : IServiceCommandHandler<ArchiveEmailCommand>
    {
        private IMailboxManager mailboxManager;

        public ArchiveEmailCommandHandler(IMailboxManager mailboxManager)
        {
            this.mailboxManager = mailboxManager;
        }

        public ISystemMessageCollection HandleCommand(ArchiveEmailCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            if (string.IsNullOrEmpty(command.MailMessage.UniqueExchangeId))
            {
                messages.AddMessage(new SystemMessage("MailMessage.UniqueExchangeId is mandatory", SystemMessageSeverityEnum.Error));
                return messages;
            }
            var open = this.mailboxManager.OpenMailboxConnection();
            if (open)
            {
                this.mailboxManager.MoveMessage(command.MailMessage);
            }
            else
            {
                messages.AddMessage(new SystemMessage("Unable to open mailbox connection", SystemMessageSeverityEnum.Error));
            }
            return messages;
        }
    }
}