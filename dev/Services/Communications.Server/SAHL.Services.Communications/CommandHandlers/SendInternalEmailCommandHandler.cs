using ActionMailerNext.Standalone;
using ActionMailerNext.Utils;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Interfaces.Communications.Commands;

namespace SAHL.Services.Communications.CommandHandlers
{
    public class SendInternalEmailCommandHandler : IServiceCommandHandler<SendInternalEmailCommand>
    {
        private IEmailManager emailManager;

        public SendInternalEmailCommandHandler(IEmailManager emailManager)
        {
            this.emailManager = emailManager;
        }

        public ISystemMessageCollection HandleCommand(SendInternalEmailCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            
            RazorEmailResult result = null;
            try
            {
                result = emailManager.GenerateEmail(command.EmailTemplate.TemplateName, command.EmailTemplate.EmailModel);
            }
            catch (NoViewsFoundException)
            {
                messages.AddMessage(new SystemMessage(string.Format("No email template found for: {0}.", command.EmailTemplate.TemplateName),
                    SystemMessageSeverityEnum.Error));
                return messages;
            }

            if (result == null)
            {
                messages.AddMessage(new SystemMessage(string.Format("An error occurred when generating an email for template: {0}.", command.EmailTemplate.TemplateName),
                    SystemMessageSeverityEnum.Error));
                return messages;
            }
            
            emailManager.DeliverEmail(result);

            return messages;
        }
    }
}