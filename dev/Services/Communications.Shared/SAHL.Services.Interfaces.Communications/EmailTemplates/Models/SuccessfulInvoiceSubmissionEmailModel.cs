
using System.Net.Mail;
using System.Collections.Generic;
using SAHL.Core.Exchange;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class SuccessfulInvoiceSubmissionEmailModel : IEmailModel
    {

        public string To { get; protected set; }

        private const string SUBJECT_PREFIX = "INVOICE SUBMISSION RECEIVED: ";

        private string subject;

        public string Subject
        {
            get
            {
                return subject;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    subject = SUBJECT_PREFIX;
                }
                else
                {
                    string subjectTemp = value.Replace(SUBJECT_PREFIX, string.Empty);
                    subject = string.Format("{0}{1}", SUBJECT_PREFIX, subjectTemp);
                }
            }
        }

        public string SAHLRefrence { get; protected set; }

        public MailPriority MailPriority { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public SuccessfulInvoiceSubmissionEmailModel(string to, string subject, MailPriority mailPriority, string sahlRefrence)
        {
            To = to;
            Subject = subject;
            MailPriority = mailPriority;
            Attachments = new List<IMailAttachment>();
            this.SAHLRefrence = sahlRefrence;
        }

    }
}
