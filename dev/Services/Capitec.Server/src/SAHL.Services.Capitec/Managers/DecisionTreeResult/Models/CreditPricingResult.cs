using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult.Models
{
    public class CreditPricingResult
    {
        public ISystemMessageCollection Messages { get; set; }

        public bool EligibleApplication { get; set; }
    }
}
