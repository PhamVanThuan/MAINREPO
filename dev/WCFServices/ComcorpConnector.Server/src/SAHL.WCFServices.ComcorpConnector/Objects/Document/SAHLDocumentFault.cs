using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    [DataContract]
    public class SAHLDocumentFault
    {
        [DataMember]
        public int FaultCode { get; set; }

        [DataMember]
        public string FaultDescription { get; set; }
    }
}