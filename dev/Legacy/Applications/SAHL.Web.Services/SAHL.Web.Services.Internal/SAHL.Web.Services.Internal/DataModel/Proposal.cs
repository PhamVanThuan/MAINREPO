using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAHL.Web.Services.Internal.DataModel
{
    [DataContract]
    public class Proposal
    {
        [DataMember]
        public int ProposalKey { get; set; }

        [DataMember]
        public string ProposalType { get; set; }

        [DataMember]
        public string ProposalStatus { get; set; }

        [DataMember]
        public int DebtCounsellingKey { get; set; }

        [DataMember]
        public bool? HOCInclusive { get; set; }

        [DataMember]
        public bool? LifeInclusive { get; set; }

        [DataMember]
        public string LegalEntityDisplayName { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public DateTime? ReviewDate { get; set; }

        [DataMember]
        public bool? Accepted { get; set; }
    }
}