using SAHL.Core.Services;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;

namespace SAHL.Services.Interfaces.Communications.Commands
{
    public class SendInternalEmailCommand : ServiceCommand, ICommunicationsServiceCommand
    {
        public IEmailTemplate<IEmailModel> EmailTemplate { get; protected set; }

        public SendInternalEmailCommand(Guid id, IEmailTemplate<IEmailModel> emailTemplate)
            : base(id)
        {
            this.EmailTemplate = emailTemplate;
        }
    }
}