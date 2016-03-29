using System;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="ImagingApplicationRequest")]
    public class ImagingApplicationRequest
    {
        private applicationHeaderType requestHeaderField;

        private ApplicationSupportingDocuments supportingDocumentsField;

        [XmlElement(ElementName ="Request.Header", Namespace ="http://localhost/ImagingApplicationRequest.xsd")]
        public applicationHeaderType RequestHeader
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

        [XmlElement(ElementName = "Supporting.Documents", Namespace = "http://localhost/ImagingApplicationRequest.xsd")]
        public ApplicationSupportingDocuments SupportingDocuments
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