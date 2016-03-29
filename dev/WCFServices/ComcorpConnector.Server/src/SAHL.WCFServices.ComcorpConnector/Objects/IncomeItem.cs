using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class IncomeItem
    {
        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public string IncomeDesc { get; set; }

        [DataMember]
        public decimal IncomeType { get; set; }

        [DataMember]
        public string CapturedDescription { get; set; }

    }
}