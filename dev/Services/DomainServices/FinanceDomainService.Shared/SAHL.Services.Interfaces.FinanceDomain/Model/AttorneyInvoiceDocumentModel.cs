using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class AttorneyInvoiceDocumentModel : ValidatableModel
    {
        [Required(ErrorMessage = "Loan Number is required.")]
        public string LoanNumber { get; protected set; }

        [Required(ErrorMessage = "Date received is required")]
        public DateTime DateReceived { get; protected set; }

        [Required(ErrorMessage = "Date processed is required")]
        public DateTime DateProcessed { get; protected set; }

        [Required(ErrorMessage = "From email address is required.")]
        public string FromEmailAddress { get; protected set; }

        [Required(ErrorMessage = "From email subject is required.")]
        public string EmailSubject { get; protected set; }

        [Required(ErrorMessage = "Attachment file name subject is required.")]
        public string InvoiceFileName { get; protected set; }

        [Required(ErrorMessage = "A file extension is required.")]
        public string InvoiceFileExtension { get; protected set; }

        public string Category { get; protected set; }

        [Required(ErrorMessage = "A file content is required.")]
        public string FileContentAsBase64 { get; protected set; }

        public AttorneyInvoiceDocumentModel(string loanNumber, DateTime dateReceived, DateTime dateProcessed, string fromEmailAddress, string emailSubject, string invoiceFileName, 
            string invoiceFileExtension, string category, string fileContentAsBase64)
        {
            this.LoanNumber = loanNumber;
            this.DateReceived = dateReceived;
            this.DateProcessed = dateProcessed;
            this.FromEmailAddress = fromEmailAddress;
            this.EmailSubject = emailSubject;
            this.InvoiceFileName = invoiceFileName;
            this.InvoiceFileExtension = invoiceFileExtension;
            this.Category = category;
            this.FileContentAsBase64 = fileContentAsBase64;
            this.Validate();
        }
    }
}