using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetAffordabilityInterestRateQuery: ServiceQuery<GetAffordabilityInterestRateQueryResult>
    {
        public bool InterestRateQuery { get; protected set; }

        public decimal CalcRate { get; protected set; }

        public decimal Deposit { get; protected set; }

        public decimal HouseholdIncome { get; protected set; }

        public new GetAffordabilityInterestRateQueryResult Result { get; set; }

        public GetAffordabilityInterestRateQuery(decimal householdIncome, decimal deposit, decimal calcRate, bool interestRateQuery)
        {
            this.HouseholdIncome = householdIncome;
            this.Deposit = deposit;
            this.CalcRate = calcRate;
            this.InterestRateQuery = interestRateQuery;
        }
    }
}
