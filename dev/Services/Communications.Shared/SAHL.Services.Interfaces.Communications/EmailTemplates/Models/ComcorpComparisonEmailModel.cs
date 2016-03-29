using SAHL.Core.Exchange;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class ComcorpComparisonEmailModel : IEmailModel
    {
        public string To { get; protected set; }

        public string Subject { get; protected set; }

        public MailPriority MailPriority { get; protected set; }

        public Dictionary<string, string> DifferingRecords { get; set; }

        public string ClientIdNumber { get; protected set; }

        public string ClientName { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }
        public bool DateOfBirthSetToComcorpProvided { get; protected set; }

        public ComcorpComparisonEmailModel(string to, string subject, Dictionary<string, string> differingRecords, MailPriority mailPriority, string clientIdNumber, string clientName, bool dateOfBirthSetToComcorpProvided)
        {
            this.To = to;
            this.Subject = subject;
            this.MailPriority = mailPriority;
            this.DifferingRecords = differingRecords;
            this.ClientIdNumber = clientIdNumber;
            this.ClientName = clientName;
            this.DateOfBirthSetToComcorpProvided = dateOfBirthSetToComcorpProvided;
        }

    }
}