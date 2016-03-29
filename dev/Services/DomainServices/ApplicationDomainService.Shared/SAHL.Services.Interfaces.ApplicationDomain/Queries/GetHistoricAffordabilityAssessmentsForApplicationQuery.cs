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
    public class GetHistoricalAffordabilityAssessmentsForApplicationQuery : ServiceQuery<AffordabilityAssessmentSummaryModel>, ISqlServiceQuery<AffordabilityAssessmentSummaryModel>, IApplicationDomainQuery
    {
        public int ApplicationKey { get; set; }

        public GetHistoricalAffordabilityAssessmentsForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}
