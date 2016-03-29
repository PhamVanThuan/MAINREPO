using System;
using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class LiabilityItem
    {
        [DataMember]
        public DateTime LiabilityDateRepayable { get; set; }

        [DataMember]
        public decimal LiabilityOutstandingLiability { get; set; }

        [DataMember]
        public string SAHLLiabilityDesc { get; set; }

        [DataMember]
        public decimal SAHLLiabilityValue { get; set; }

        [DataMember]
        public decimal SAHLLiabilityCost { get; set; }

        [DataMember]
        public string LiabilityCompanyName { get; set; }

        [DataMember]
        public string LiabilityLoanType { get; set; }

        [DataMember]
        public string LiabilityDescription { get; set; }

        [DataMember]
        public decimal LiabilityAssetValue { get; set; }

    }
}