using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.DomainProcessManager.Models;
using System.IO;
using System.Linq;

namespace SAHL.Services.PollingManager.Helpers
{
    public interface ILossControlMailHelper
    {
        IMailMessage PreparePositiveMailResponse(IMailMessage requestMessage);

        IMailMessage PrepareNegativeMailResponse(IMailMessage requestMessage);

        string GetAttachmentsAsString(IMailMessage mailMessage);

        IMailAttachment GetAttachedInvoice(IMailMessage emailMessage);

        void GetAttachmentFileDetails(string fullFileName, out string fileName, out string fileExtenstion);

        ReceiveAttorneyInvoiceProcessModel GetPreProcessedThirdPartyInvoice(int loanNumber, IMailMessage emailMessage);
    }

    public class LossControlMailHelper : ILossControlMailHelper
    {
        public IMailMessage PreparePositiveMailResponse(IMailMessage requestMessage)
        {
            IMailMessage responseMessage = new MailMessage();
            responseMessage.To = requestMessage.From;
            responseMessage.From = requestMessage.To;
            responseMessage.Subject = "Received - " + requestMessage.Subject;

            return responseMessage;
        }

        public IMailMessage PrepareNegativeMailResponse(IMailMessage requestMessage)
        {
            IMailMessage responseMessage = new MailMessage();
            responseMessage.To = requestMessage.From;
            responseMessage.From = requestMessage.To;
            responseMessage.Subject = "Failed - " + requestMessage.Subject;

            return responseMessage;
        }

        public IMailAttachment GetAttachedInvoice(IMailMessage emailMessage)
        {
            return emailMessage.Attachments.FirstOrDefault(x => x.AttachmentType == ".pdf" | x.AttachmentType == ".tiff");
        }

        public string GetAttachmentsAsString(IMailMessage mailMessage)
        {
            string response = "";
            foreach (IMailAttachment mailAttachment in mailMessage.Attachments)
            {
                if (response.Length == 0) { response += mailAttachment.AttachmentName; }
                else { response += " \n\r " + mailAttachment.AttachmentName; }
            }
            return response;
        }

        public void GetAttachmentFileDetails(string fullFileName, out string fileName, out string fileExtension)
        {
            fileName = Path.GetFileNameWithoutExtension(fullFileName);
            fileExtension = Path.GetExtension(fullFileName);
            if (fileExtension.StartsWith("."))
            {
                fileExtension = fileExtension.Substring(1, fileExtension.Length - 1);
            }
        }


        public ReceiveAttorneyInvoiceProcessModel GetPreProcessedThirdPartyInvoice(int loanNumber, IMailMessage emailMessage)
        {
            string fileName;
            string fileExtension;

            var attachedInvoice = GetAttachedInvoice(emailMessage);
            GetAttachmentFileDetails(attachedInvoice.AttachmentName, out fileName, out fileExtension);

            var dataModel = new ReceiveAttorneyInvoiceProcessModel(loanNumber, emailMessage.DateRecieved, emailMessage.From, 
                emailMessage.Subject, fileName, fileExtension, string.Empty, attachedInvoice.ContentAsBase64);

            return dataModel;
        }
    }
}