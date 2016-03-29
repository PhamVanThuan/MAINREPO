using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System.Net.Mail;
using System.Collections.Generic;
using SAHL.Core.Exchange;

namespace SAHL.Services.Communications.Specs.Models
{
    public class TestEmailModel : IEmailModel
    {
        public string To { get; protected set; }

        public string Subject { get; protected set; }

        public MailPriority MailPriority { get; protected set; }

        public string Body { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public TestEmailModel(string to, string subject, string body, MailPriority mailPriority, IEnumerable<IMailAttachment> attachments)
        {
            this.To = to;
            this.Subject = subject;
            this.Body = body;
            this.MailPriority = mailPriority;
            this.Attachments = attachments;
        }
    }
}