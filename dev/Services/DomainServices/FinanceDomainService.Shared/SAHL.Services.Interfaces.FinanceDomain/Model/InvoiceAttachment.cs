using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class InvoiceAttachment
    {
        [Required]
        public string FileName { get; protected set; }

        [Required]
        public string FileExtension { get; protected set; }

        [Required]
        public string FileContents { get; protected set; }

        public InvoiceAttachment(string fileName, string fileExtension, string fileContents)
        {
            this.FileName = fileName;
            this.FileExtension = fileExtension;
            this.FileContents = fileContents;
        }
    }
}