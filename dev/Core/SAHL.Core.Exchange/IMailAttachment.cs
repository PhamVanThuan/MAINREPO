namespace SAHL.Core.Exchange
{
    public interface IMailAttachment
    {
        string AttachmentName { get; set; }
        string AttachmentType { get; set; }
        string ContentAsBase64 { get; set; }
    }
}