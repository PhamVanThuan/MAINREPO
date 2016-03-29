using System.Collections.Generic;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;

namespace SAHL.Services.PollingManager.Server.Tests
{
    public static class FakeMailMessageFactory
    {
        public static List<IMailMessage> ReturnSingleValidMessage()
        {
            List<IMailMessage> items = new List<IMailMessage>();
            items.Add(GetMailMessageWithSingleAttachment());
            return items;
        }

        public static IMailMessage GetMailMessageWithSingleAttachment()
        {
            var mailMessage = CreateMailMessage();
            AddPdfDocument(mailMessage, false);
            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithTwoAttachments()
        {
            var mailMessage = CreateMailMessage();
            AddPdfDocument(mailMessage, false);
            AddPdfDocument(mailMessage, false);
            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithThreeAttachments()
        {
            var mailMessage = CreateMailMessage();

            AddPdfDocument(mailMessage, false);
            AddPdfDocument(mailMessage, false);
            AddPdfDocument(mailMessage, false);

            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithInValidAttachments()
        {
            var mailMessage = CreateMailMessage();
            AddImage(mailMessage);

            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithTwoValidAttachments()
        {
            var mailMessage = CreateMailMessage();
            AddImage(mailMessage);
            AddPdfDocument(mailMessage, false);
            AddTiffDocument(mailMessage, false);

            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithSingleValidAttachment(bool pdf = true, bool upperCaseAttachmentType = false)
        {
            var mailMessage = CreateMailMessage();
            AddImage(mailMessage);
            if (pdf)
            {
                AddPdfDocument(mailMessage, upperCaseAttachmentType);
            }
            else
            {
                AddTiffDocument(mailMessage, upperCaseAttachmentType);
            }
            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithSingleValidAttachmentAndNoSubjectLine()
        {
            var mailMessage = GetMailMessageWithSingleValidAttachment();
            mailMessage.Subject = "";
            return mailMessage;
        }

        public static IMailMessage GetMailMessageWithSingleValidAttachmentAndIncorrectSubjectLineFormat()
        {
            var mailMessage = GetMailMessageWithSingleValidAttachment();
            mailMessage.Subject = "12345 - ";
            return mailMessage;
        }

        private static MailMessage CreateMailMessage()
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "12345 -  My Invoice 1";
            mailMessage.From = "test@test.com";
            mailMessage.To = "losscontrol@sahl.com";
            return mailMessage;
        }

        private static void AddImage(MailMessage mailMessage)
        {
            MailAttachment mailAttachment = new MailAttachment();
            mailAttachment.AttachmentType = ".jpg";
            mailAttachment.AttachmentName = "Some Image.jpg";
            mailAttachment.ContentAsBase64 = string.Empty;
            mailMessage.Attachments.Add(mailAttachment);
        }

        private static void AddPdfDocument(MailMessage mailMessage, bool toUpper)
        {
            MailAttachment mailAttachment = new MailAttachment();
            string attachmentType = ".pdf";
            attachmentType = toUpper ? attachmentType.ToUpper() : attachmentType.ToLower();
            mailAttachment.AttachmentType = attachmentType;
            mailAttachment.AttachmentName = "document.pdf";
            mailAttachment.ContentAsBase64 = "=45897DJAFHJK4384";
            mailMessage.Attachments.Add(mailAttachment);
        }

        private static void AddTiffDocument(MailMessage mailMessage, bool toUpper)
        {
            MailAttachment mailAttachment = new MailAttachment();
            string attachmentType = ".tiff";
            attachmentType = toUpper ? attachmentType.ToUpper() : attachmentType.ToLower();
            mailAttachment.AttachmentType = attachmentType;
            mailAttachment.AttachmentName = "document.tiff";
            mailAttachment.ContentAsBase64 = "=45897DJAFHJK4384";
            mailMessage.Attachments.Add(mailAttachment);
        }
    }
}
