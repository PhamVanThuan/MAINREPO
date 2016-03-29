using SAHL.Core.Exchange;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates.Models
{
    public class InvoiceScheduledForPaymentEmailModel : IEmailModel
    {
        public InvoiceScheduledForPaymentEmailModel(string recipientName, string to, MailPriority mailPriority, IEnumerable<InvoicePaymentModel> invoicePayments)
        {
            To = to;
            MailPriority = mailPriority;
            Attachments = new List<IMailAttachment>();
            InvoicePayments = invoicePayments;
            RecipientName = recipientName;
        }

        public string To { get; protected set; }

        public string Subject
        {
            get
            {
                return "Invoice(s) scheduled for payment";
            }
        }

        public System.Net.Mail.MailPriority MailPriority { get; protected set; }

        public IEnumerable<IMailAttachment> Attachments { get; protected set; }

        public IEnumerable<InvoicePaymentModel> InvoicePayments { get; protected set; }

        public string RecipientName { get; protected set; }

        public decimal TotalScheduledForPayment
        {
            get
            {
                return this.InvoicePayments.Sum(y => y.InvoiceTotalAmount);
            }
        }
    }

    public class InvoicePaymentModel
    {
        public int AccountNumber { get; protected set; }

        public string InvoiceReferenceNumber { get; protected set; }

        public decimal InvoiceTotalAmount { get; protected set; }

        public InvoicePaymentModel(int accountNumber, string invoiceReferenceNumber, decimal invoiceTotalAmount)
        {
            AccountNumber = accountNumber;
            InvoiceReferenceNumber = invoiceReferenceNumber;
            InvoiceTotalAmount = invoiceTotalAmount;
        }
    }
}