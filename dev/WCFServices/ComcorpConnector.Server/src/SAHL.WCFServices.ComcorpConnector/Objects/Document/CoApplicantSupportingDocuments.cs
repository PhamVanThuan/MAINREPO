using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="CoApplicantSupportingDocuments", Namespace = "http://localhost/ImagingCoApplicantRequest.xsd")]
    public class CoApplicantSupportingDocuments
    {
        private coApplicantType coApplicantDocumentsField;

        [XmlElement("Co.Applicant.Documents")]
        public coApplicantType CoApplicantDocuments
        {
            get
            {
                return this.coApplicantDocumentsField;
            }
            set
            {
                this.coApplicantDocumentsField = value;
            }
        }
    }
}