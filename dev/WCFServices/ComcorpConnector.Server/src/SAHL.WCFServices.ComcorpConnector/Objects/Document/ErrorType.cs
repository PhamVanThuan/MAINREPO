using System.Xml.Serialization;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(Namespace = "http://localhost/ImagingReplyStatus.xsd")]
    public partial class errorType
    {
        private string errorDescriptionField;

        [XmlElement("Error.Description")]
        public string ErrorDescription
        {
            get
            {
                return this.errorDescriptionField;
            }
            set
            {
                this.errorDescriptionField = value;
            }
        }
    }
}
