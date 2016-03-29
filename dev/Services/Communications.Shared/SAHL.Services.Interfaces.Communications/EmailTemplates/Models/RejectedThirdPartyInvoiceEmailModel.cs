using SAHL.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class RejectedThirdPartyInvoiceEmailModel : IEmailModel
    {
        public RejectedThirdPartyInvoiceEmailModel(
              string sahlReference
            , int accountNumber
            , int thirdPartyInvoiceKey
            , string to
            , string invoiceNumber
            , string rejectedBy
            , string rejectionComment
            , DateTime eventDate
            , string subject
            , MailPriority mailPriority
        )
        {
            this.AccountNumber = accountNumber;
            this.To = to;
            this.EventDate = eventDate;
            this.InvoiceNumber = invoiceNumber;
            this.RejectedBy = rejectedBy;
            this.RejectionComment = rejectionComment;
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.SAHLReference = sahlReference;
            this.Subject = subject;
            this.MailPriority = mailPriority;
            this.Attachments = new List<IMailAttachment>();
        }

        public string To { get; protected set; }

        public string Subject { get; protected set; }

        public MailPriority MailPriority { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public int AccountNumber { get; protected set; }

        public DateTime EventDate { get; protected set; }

        public string InvoiceNumber { get; protected set; }

        public string RejectedBy { get; protected set; }

        public string RejectionComment { get; protected set; }

        public string SAHLReference { get; protected set; }

        public int ThirdPartyInvoiceKey { get; protected set; }
    }
}