using System.Net.Mail;
using System.Collections.Generic;
using SAHL.Core.Exchange;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public interface IEmailModel
    {
        string To { get; }

        string Subject { get; }

        MailPriority MailPriority { get; }

        IEnumerable<IMailAttachment> Attachments { get; }
    }
}