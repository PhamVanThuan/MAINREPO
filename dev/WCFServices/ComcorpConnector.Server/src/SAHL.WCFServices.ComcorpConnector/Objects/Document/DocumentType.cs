using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingCoApplicantRequest.xsd", TypeName = "CoApplicantDocumentType")]
    public class coApplicantDocumentType : documentType
    {
    }

    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingMainApplicantRequest.xsd", TypeName = "MainApplicantDocumentType")]
    public class mainApplicantDocumentType : documentType
    {
    }

    [Serializable()]
    [XmlType]
    public class documentType
    {
        private decimal documentReferenceField;

        private string documentTypeField;

        private string documentDescriptionField;

        private string documentCommentsField;

        private string documentImageField;

        [XmlElement("Document.Reference")]
        public decimal DocumentReference
        {
            get
            {
                return this.documentReferenceField;
            }
            set
            {
                this.documentReferenceField = value;
            }
        }

        [XmlElement("Document.Type")]
        public string DocumentType
        {
            get
            {
                return this.documentTypeField;
            }
            set
            {
                this.documentTypeField = value;
            }
        }

        [XmlElement("Document.Description")]
        public string DocumentDescription
        {
            get
            {
                return this.documentDescriptionField;
            }
            set
            {
                this.documentDescriptionField = value;
            }
        }

        [XmlElement("Document.Comments")]
        public string DocumentComments
        {
            get
            {
                return this.documentCommentsField;
            }
            set
            {
                this.documentCommentsField = value;
            }
        }

        [XmlElement("Document.Image")]
        public string DocumentImage
        {
            get
            {
                return this.documentImageField;
            }
            set
            {
                this.documentImageField = value;
            }
        }
    }
}