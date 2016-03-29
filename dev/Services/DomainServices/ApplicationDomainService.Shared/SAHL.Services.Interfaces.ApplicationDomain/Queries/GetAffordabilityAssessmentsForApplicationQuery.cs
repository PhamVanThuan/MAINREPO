using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetAffordabilityAssessmentsForApplicationQuery : ServiceQuery<AffordabilityAssessmentSummaryModel>, ISqlServiceQuery<AffordabilityAssessmentSummaryModel>, IApplicationDomainQuery
    {
        public GetAffordabilityAssessmentsForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; set; }
    }
}