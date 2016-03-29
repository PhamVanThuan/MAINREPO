namespace SAHL.Core.Exchange.Provider
{
    public class MailAttachment : IMailAttachment
    {
        public string AttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public string ContentAsBase64 { get; set; }
    }
}