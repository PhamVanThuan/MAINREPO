using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Public.Models
{
    public class ProposalViewModel
    {
        public int ProposalKey { get; set; }

        public string ProposalType { get; set; }

        public string ProposalStatus { get; set; }

        public int DebtCounsellingKey { get; set; }

        public bool? HOCInclusive { get; set; }

        public bool? LifeInclusive { get; set; }

        public string LegalEntityDisplayName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ReviewDate { get; set; }

        public bool? Accepted { get; set; }
    }
}