using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetOpenNewBusinessApplicationQuery : ServiceQuery<GetOpenNewBusinessApplicationQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetOpenNewBusinessApplicationQueryResult>
    {
        public bool HasDebitOrder { get; protected set; }
        public bool HasMailingAddress { get; protected set; }
        public bool HasProperty { get; protected set; }
        public bool IsAccepted { get; protected set; }
        public double HouseholdIncome { get; protected set; }

        public GetOpenNewBusinessApplicationQuery(bool hasDebitOrder, bool hasMailingAddress, bool hasProperty, bool isAccepted, double householdIncome)
        {
            this.HasDebitOrder = hasDebitOrder;
            this.HasMailingAddress = hasMailingAddress;
            this.HasProperty = hasProperty;
            this.IsAccepted = isAccepted;
            this.HouseholdIncome = householdIncome;
        }
    }
}
