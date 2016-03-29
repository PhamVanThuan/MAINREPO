using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FinanceDomain.Managers
{
    public class ApprovalMandateRanges
    {
        public decimal LowerBound { get; set; }

        public decimal UpperBound { get; set; }

        public string Capability { get; set; }
    }
}
