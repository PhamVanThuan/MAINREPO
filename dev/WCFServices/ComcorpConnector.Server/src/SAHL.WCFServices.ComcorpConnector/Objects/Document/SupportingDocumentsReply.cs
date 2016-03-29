using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [SerializableAttribute()]
    [XmlType(AnonymousType = true, Namespace = "http://localhost/ImagingReplyStatus.xsd")]
    [XmlRoot("Supporting.Documents.Reply", Namespace = "http://localhost/ImagingReplyStatus.xsd", IsNullable = false)]
    public partial class SupportingDocumentsReply
    {
        private ReplyHeaderType replyHeaderField;

        private errorType[] requestErrorsField;

        [XmlElement("Reply.Header")]
        public ReplyHeaderType ReplyHeader
        {
            get
            {
                return this.replyHeaderField;
            }
            set
            {
                this.replyHeaderField = value;
            }
        }

        [XmlElement("Request.Errors")]
        public errorType[] RequestErrors
        {
            get
            {
                return this.requestErrorsField;
            }
            set
            {
                this.requestErrorsField = value;
            }
        }
    }
}