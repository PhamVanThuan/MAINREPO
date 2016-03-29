using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Queries
{
    public class GetAffordabilityAssessmentByKeyQuery : ServiceQuery<AffordabilityAssessmentModel>, IApplicationDomainQuery
    {
        public GetAffordabilityAssessmentByKeyQuery(int affordabilityAssessmentKey)
        {
            this.AffordabilityAssessmentKey = affordabilityAssessmentKey;
        }

        [Required]
        public int AffordabilityAssessmentKey { get; set; }
    }
}
