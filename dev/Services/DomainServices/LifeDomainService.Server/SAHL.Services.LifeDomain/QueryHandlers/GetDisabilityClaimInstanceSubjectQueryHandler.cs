using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.QueryHandlers
{
    public class GetDisabilityClaimInstanceSubjectQueryHandler : IServiceQueryHandler<GetDisabilityClaimInstanceSubjectQuery>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public GetDisabilityClaimInstanceSubjectQueryHandler(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetDisabilityClaimInstanceSubjectQuery query)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            string subject = lifeDomainDataManager.GetDisabilityClaimInstanceSubject(query.DisabilityClaimKey);

            query.Result = new ServiceQueryResult<string>(new List<string>() { subject });

            return systemMessageCollection;
        }
    }
}