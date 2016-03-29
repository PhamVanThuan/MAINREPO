using System.Xml.Serialization;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://localhost/ImagingMainApplicantRequest.xsd")]
    public enum headerTypeRequestAction
    {
        New,
        Update,
    }
}
