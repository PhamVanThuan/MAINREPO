using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetUnconfirmedAffordabilityAssessmentsForApplicationQuery 
        : ServiceQuery<AffordabilityAssessmentSummaryModel>, ISqlServiceQuery<AffordabilityAssessmentSummaryModel>, 
          IServiceQuery<IServiceQueryResult<AffordabilityAssessmentSummaryModel>>, 
          IApplicationDomainQuery, 
          IServiceQuery, 
          IServiceCommand
    {
        public GetUnconfirmedAffordabilityAssessmentsForApplicationQuery(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        [Required]
        public int ApplicationKey { get; set; }
    }
}