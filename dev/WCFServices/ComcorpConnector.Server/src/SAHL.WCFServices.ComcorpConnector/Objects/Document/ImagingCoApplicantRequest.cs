using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="ImagingCoApplicantRequest")]
    public class ImagingCoApplicantRequest
    {
        private coApplicantHeaderType requestHeaderField;

        private CoApplicantSupportingDocuments supportingDocumentsField;

        [XmlElement("Request.Header", Namespace = "http://localhost/ImagingCoApplicantRequest.xsd")]
        public coApplicantHeaderType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
            }
        }

        [XmlElement("Supporting.Documents", Namespace = "http://localhost/ImagingCoApplicantRequest.xsd")]
        public CoApplicantSupportingDocuments SupportingDocuments
        {
            get
            {
                return this.supportingDocumentsField;
            }
            set
            {
                this.supportingDocumentsField = value;
            }
        }
    }
}