using System;
using System.Collections.Generic;
using SAHL.Core.Exchange.Provider;

namespace SAHL.Core.Exchange.Specs.Fakes
{
    public static class FakeMailMessageFactory
    {
        public static List<IMailMessage> ReturnMailMessages()
        {
            List<IMailMessage> items = new List<IMailMessage>();
            items.Add(GetNewFirstMessage());
            items.Add(GetNewSecondMessage());
            return items;
        }
        
        public static List<IMailMessage> ReturnSingleMessage()
        {
            List<IMailMessage> items = new List<IMailMessage>();
            items.Add(GetNewFirstMessage());
            return items;
        }

        public static List<IMailMessage> ReturnSingleValidMessage()
        {
            List<IMailMessage> items = new List<IMailMessage>();
            items.Add(GetNewFirstValidMailMessage());
            return items;
        }

        public static List<IMailMessage> ReturnSingleInValidMessage()
        {
            List<IMailMessage> items = new List<IMailMessage>();
            items.Add(GetNewInValidFormatMailMessage());
            return items;
        }

        public static IMailMessage GetNewFirstMessage()
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Test Message 1";
            return mailMessage;
        }

        public static IMailMessage GetNewSecondMessage()
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Test Message 2";
            return mailMessage;
        }

        public static IMailMessage GetNewFirstValidMailMessage()
        {
            MailMessage mailMessage = CreateMailMessage();
            AddPdfDocument(mailMessage);
            return mailMessage;
        }

        public static IMailMessage GetNewInValidFormatMailMessage()
        {
            MailMessage mailMessage = CreateMailMessage();
            mailMessage.Subject = "12345-";
            AddPdfDocument(mailMessage);
            return mailMessage;
        }

        private static MailMessage CreateMailMessage()
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "12345 - 56666 - My Invoice 1";
            mailMessage.From = "test@test.com";
            mailMessage.To = "losscontrol@sahl.com";
            return mailMessage;
        }

        private static void AddImage(MailMessage mailMessage)
        {
            MailAttachment mailAttachment = new MailAttachment();
            mailAttachment.AttachmentType = "jpg";
            mailAttachment.AttachmentName = "Some Image";
            mailAttachment.ContentAsBase64 = "dGVzdHN0cmluZw==";
            mailMessage.Attachments.Add(mailAttachment);
        }

        private static void AddPdfDocument(MailMessage mailMessage)
        {
            MailAttachment mailAttachment = new MailAttachment();
            mailAttachment.AttachmentType = "pdf";
            mailAttachment.AttachmentName = "Some PDF Document";
            mailAttachment.ContentAsBase64 = "dGVzdHN0cmluZw==";
            mailMessage.Attachments.Add(mailAttachment);
        }

        private static void AddTiffDocument(MailMessage mailMessage)
        {
            MailAttachment mailAttachment = new MailAttachment();
            mailAttachment.AttachmentType = "tiff";
            mailAttachment.AttachmentName = "Some Tiff Document";
            mailAttachment.ContentAsBase64 = "dGVzdHN0cmluZw==";
            mailMessage.Attachments.Add(mailAttachment);
        }

    }
}