using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class ExpenditureItem
    {
        [DataMember]
        public decimal ExpenditureAmount { get; set; }

        [DataMember]
        public string ExpenditureDesc { get; set; }

        [DataMember]
        public decimal ExpenditureType { get; set; }

        [DataMember]
        public string CapturedDescription { get; set; }
    }
}