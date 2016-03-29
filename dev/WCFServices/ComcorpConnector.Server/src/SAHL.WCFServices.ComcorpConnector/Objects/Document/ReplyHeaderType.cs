using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://localhost/ImagingReplyStatus.xsd")]
    public class ReplyHeaderType
    {
        private string applicationReferenceField;

        private string imagingReferenceField;

        private System.DateTime replyDateTimeField;

        private decimal serviceVersionField;

        private decimal requestStatusField;

        [XmlElementAttribute("Application.Reference")]
        public string ApplicationReference
        {
            get
            {
                return this.applicationReferenceField;
            }
            set
            {
                this.applicationReferenceField = value;
            }
        }

        [XmlElementAttribute("Imaging.Reference")]
        public string ImagingReference
        {
            get
            {
                return this.imagingReferenceField;
            }
            set
            {
                this.imagingReferenceField = value;
            }
        }

        [XmlElementAttribute("Reply.DateTime")]
        public System.DateTime ReplyDateTime
        {
            get
            {
                return this.replyDateTimeField;
            }
            set
            {
                this.replyDateTimeField = value;
            }
        }

        [XmlElementAttribute("Service.Version")]
        public decimal ServiceVersion
        {
            get
            {
                return this.serviceVersionField;
            }
            set
            {
                this.serviceVersionField = value;
            }
        }

        [XmlElementAttribute("Request.Status")]
        public decimal RequestStatus
        {
            get
            {
                return this.requestStatusField;
            }
            set
            {
                this.requestStatusField = value;
            }
        }
    }
}