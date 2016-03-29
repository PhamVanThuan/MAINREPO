using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetAffordabilityAssessmentStressFactorsQuery 
        : ServiceQuery<AffordabilityAssessmentStressFactorModel>, ISqlServiceQuery<AffordabilityAssessmentStressFactorModel>, IApplicationDomainQuery
    {
        public GetAffordabilityAssessmentStressFactorsQuery()
        {
        }
    }
}