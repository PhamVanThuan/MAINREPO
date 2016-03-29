using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingMainApplicantRequest.xsd", TypeName = "MainApplicantSpouseType")]
    public class mainApplicantSpouseType : spouseType
    {
        private mainApplicantDocumentType[] supportingDocumentField;

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
    }

    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingCoApplicantRequest.xsd", TypeName = "CoApplicantSpouseType")]
    public class coApplicantSpouseType : spouseType
    {
        private coApplicantDocumentType[] supportingDocumentField;

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
    }

    [Serializable()]
    [XmlType]
    public class spouseType
    {
        private decimal applicantReferenceField;

        private string applicantFirstNameField;

        private string applicantSurnameField;

        private string applicantIdentityNumberField;

        private string applicantPassportNumberField;

        private string bankCustomField;

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