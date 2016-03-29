using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.QueryHandlers
{
    public class GetDisabilityClaimStatusDescriptionQueryHandler : IServiceQueryHandler<GetDisabilityClaimStatusDescriptionQuery>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public GetDisabilityClaimStatusDescriptionQueryHandler(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetDisabilityClaimStatusDescriptionQuery query)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            string disabilityClaimStatus = lifeDomainDataManager.GetDisabilityClaimStatusDescription(query.DisabilityClaimKey);

            query.Result = new ServiceQueryResult<string>(new List<string>() { disabilityClaimStatus });

            return systemMessageCollection;
        }
    }
}