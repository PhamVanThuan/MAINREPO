using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://localhost/ImagingCoApplicantRequest.xsd", TypeName = "CoApplicantHeaderType")]
    public class coApplicantHeaderType : headerType
    {
    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://localhost/ImagingMainApplicantRequest.xsd", TypeName = "MainApplicantHeaderType")]
    public class mainApplicantHeaderType : headerType
    {
    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://localhost/ImagingApplicationRequest.xsd", TypeName = "ApplicationHeaderType")]
    public class applicationHeaderType : headerType
    {
    }

    [SerializableAttribute()]
    [XmlType]
    public class headerType
    {
        private headerTypeRequestAction requestActionField;

        private string requestMacField;

        private string applicationReferenceField;

        private string bankReferenceField;

        private string imagingReferenceField;

        private decimal requestMessagesField;

        private DateTime requestDateTimeField;

        private decimal serviceVersionField;

        private string bankCustomField;

        [XmlElement("Request.Action")]
        public headerTypeRequestAction RequestAction
        {
            get
            {
                return this.requestActionField;
            }
            set
            {
                this.requestActionField = value;
            }
        }

        [XmlElement("Request.Mac")]
        public string RequestMac
        {
            get
            {
                return this.requestMacField;
            }
            set
            {
                this.requestMacField = value;
            }
        }

        [XmlElement("Application.Reference")]
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

        [XmlElement("Bank.Reference")]
        public string BankReference
        {
            get
            {
                return this.bankReferenceField;
            }
            set
            {
                this.bankReferenceField = value;
            }
        }

        [XmlElement("Imaging.Reference")]
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

        [XmlElement("Request.Messages")]
        public decimal RequestMessages
        {
            get
            {
                return this.requestMessagesField;
            }
            set
            {
                this.requestMessagesField = value;
            }
        }

        [XmlElement("Request.DateTime")]
        public DateTime RequestDateTime
        {
            get
            {
                return this.requestDateTimeField;
            }
            set
            {
                this.requestDateTimeField = value;
            }
        }

        [XmlElement("Service.Version")]
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

        [XmlElement("Bank.Custom")]
        public string BankCustom
        {
            get
            {
                return this.bankCustomField;
            }
            set
            {
                this.bankCustomField = value;
            }
        }
    }
}