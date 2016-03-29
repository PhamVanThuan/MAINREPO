using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="ImagingMainApplicantRequest")]
    public class ImagingMainApplicantRequest
    {
        private mainApplicantHeaderType requestHeaderField;

        private MainApplicantSupportingDocuments supportingDocumentsField;

        [XmlElement("Request.Header", Namespace = "http://localhost/ImagingMainApplicantRequest.xsd")]
        public mainApplicantHeaderType RequestHeader
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

        [XmlElement("Supporting.Documents", Namespace = "http://localhost/ImagingMainApplicantRequest.xsd")]
        public MainApplicantSupportingDocuments SupportingDocuments
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