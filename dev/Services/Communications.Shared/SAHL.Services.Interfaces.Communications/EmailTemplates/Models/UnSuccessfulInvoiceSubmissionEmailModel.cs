using SAHL.Core.Exchange;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class UnSuccessfulInvoiceSubmissionEmailModel : IEmailModel
    {
        public string To { get; protected set; }

        private const string SUBJECT_PREFIX = "INVOICE SUBMISSION REJECTED: ";

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

        public MailPriority MailPriority { get; protected set; }

        public IEnumerable<string> RejectionReasons { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public UnSuccessfulInvoiceSubmissionEmailModel(string to, string subject, MailPriority mailPriority, IEnumerable<string> rejectionReasons
                                           , IEnumerable<IMailAttachment> attachments = null)
        {
            To = to;
            Subject = subject;
            MailPriority = mailPriority;
            RejectionReasons = rejectionReasons;
            Attachments = attachments;
        }
    }
}