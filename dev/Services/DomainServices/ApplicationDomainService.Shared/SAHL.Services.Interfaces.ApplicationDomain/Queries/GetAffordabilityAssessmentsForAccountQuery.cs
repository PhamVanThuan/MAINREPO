using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetAffordabilityAssessmentsForAccountQuery : ServiceQuery<AffordabilityAssessmentSummaryModel>, ISqlServiceQuery<AffordabilityAssessmentSummaryModel>, IApplicationDomainQuery
    {
        public int AccountKey { get; set; }

        public GetAffordabilityAssessmentsForAccountQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}
