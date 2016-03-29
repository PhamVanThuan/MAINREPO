using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.QueryHandlers
{
    public class GetAffordabilityAssessmentByKeyQueryHandler : IServiceQueryHandler<GetAffordabilityAssessmentByKeyQuery>
    {
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;

        public GetAffordabilityAssessmentByKeyQueryHandler(IAffordabilityAssessmentManager affordabilityAssessmentManager)
        {
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
        }

        public ISystemMessageCollection HandleQuery(GetAffordabilityAssessmentByKeyQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var result = this.affordabilityAssessmentManager.GetAffordabilityAssessment(query.AffordabilityAssessmentKey);
            query.Result = new ServiceQueryResult<AffordabilityAssessmentModel>(new List<AffordabilityAssessmentModel> { result });
            
            return messages; 

        }
    }
}
