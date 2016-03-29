using System.Xml.Serialization;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(TypeName ="ApplicationSupportingDocuments", Namespace = "http://localhost/ImagingApplicationRequest.xsd")]
    public class ApplicationSupportingDocuments {

        private documentType[] applicationDocumentsField;

        [XmlArray("Application.Documents")]
        [XmlArrayItem("Supporting.Document", IsNullable = false)]
        public documentType[] ApplicationDocuments {
            get {
                return this.applicationDocumentsField;
            }
            set {
                this.applicationDocumentsField = value;
            }
        }
    }
}