using System.Net.Mail;
using System.Collections.Generic;
using SAHL.Core.Exchange;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class StandardEmailModel : IEmailModel
    {
        public string To { get; protected set; }

        public string Subject { get; protected set; }

        public string Body { get; protected set; }

        public MailPriority MailPriority { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public StandardEmailModel(string to, string subject, string body, MailPriority mailPriority, IEnumerable<IMailAttachment> attachments = null)
        {
            this.To = to;
            this.Subject = subject;
            this.Body = body;
            this.MailPriority = mailPriority;
            this.Attachments = attachments;
        }

    }
}