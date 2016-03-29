using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="MainApplicantSupportingDocuments", Namespace = "http://localhost/ImagingMainApplicantRequest.xsd")]
    public class MainApplicantSupportingDocuments
    {
        private mainApplicantType mainApplicantDocumentsField;

        [XmlElement("Main.Applicant.Documents")]
        public mainApplicantType MainApplicantDocuments
        {
            get
            {
                return this.mainApplicantDocumentsField;
            }
            set
            {
                this.mainApplicantDocumentsField = value;
            }
        }
    }
}