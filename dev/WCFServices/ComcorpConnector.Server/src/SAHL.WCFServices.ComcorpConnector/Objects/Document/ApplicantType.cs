using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingCoApplicantRequest.xsd", TypeName = "CoApplicantType")]
    public class coApplicantType : applicantType
    {
        protected coApplicantDocumentType[] supportingDocumentField;

        protected coApplicantSpouseType spouseDocumentsField;

        [XmlElement("Supporting.Document")]
        public coApplicantDocumentType[] SupportingDocument
        {
            get
            {
                return this.supportingDocumentField;
            }
            set
            {
                this.supportingDocumentField = value;
            }
        }

        [XmlElement("Spouse.Documents")]
        public coApplicantSpouseType SpouseDocuments
        {
            get
            {
                return this.spouseDocumentsField;
            }
            set
            {
                this.spouseDocumentsField = value;
            }
        }
    }

    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingMainApplicantRequest.xsd", TypeName = "MainApplicantType")]
    public class mainApplicantType : applicantType
    {
        protected mainApplicantDocumentType[] supportingDocumentField;

        protected mainApplicantSpouseType spouseDocumentsField;

        [XmlElement("Supporting.Document")]
        public mainApplicantDocumentType[] SupportingDocument
        {
            get
            {
                return this.supportingDocumentField;
            }
            set
            {
                this.supportingDocumentField = value;
            }
        }

        [XmlElement("Spouse.Documents")]
        public mainApplicantSpouseType SpouseDocuments
        {
            get
            {
                return this.spouseDocumentsField;
            }
            set
            {
                this.spouseDocumentsField = value;
            }
        }
    }

    public class applicantType
    {
        protected decimal applicantReferenceField;

        protected string applicantFirstNameField;

        protected string applicantSurnameField;

        protected string applicantIdentityNumberField;

        protected string applicantPassportNumberField;

        protected string bankCustomField;

        [XmlElement("Applicant.Reference")]
        public decimal ApplicantReference
        {
            get
            {
                return this.applicantReferenceField;
            }
            set
            {
                this.applicantReferenceField = value;
            }
        }

        [XmlElement("Applicant.FirstName")]
        public string ApplicantFirstName
        {
            get
            {
                return this.applicantFirstNameField;
            }
            set
            {
                this.applicantFirstNameField = value;
            }
        }

        [XmlElement("Applicant.Surname")]
        public string ApplicantSurname
        {
            get
            {
                return this.applicantSurnameField;
            }
            set
            {
                this.applicantSurnameField = value;
            }
        }

        [XmlElement("Applicant.IdentityNumber")]
        public string ApplicantIdentityNumber
        {
            get
            {
                return this.applicantIdentityNumberField;
            }
            set
            {
                this.applicantIdentityNumberField = value;
            }
        }

        [XmlElement("Applicant.PassportNumber")]
        public string ApplicantPassportNumber
        {
            get
            {
                return this.applicantPassportNumberField;
            }
            set
            {
                this.applicantPassportNumberField = value;
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