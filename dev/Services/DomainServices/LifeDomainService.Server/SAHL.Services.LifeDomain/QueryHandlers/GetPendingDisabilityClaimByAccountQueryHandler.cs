using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.QueryHandlers
{
    public class GetPendingDisabilityClaimByAccountQueryHandler : IServiceQueryHandler<GetPendingDisabilityClaimByAccountQuery>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public GetPendingDisabilityClaimByAccountQueryHandler(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetPendingDisabilityClaimByAccountQuery query)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            DisabilityClaimModel disabilityClaimDataModel = lifeDomainDataManager.GetDisabilityClaimsByAccount(query.AccountKey)
                .Where(x => x.DisabilityClaimStatusKey == (int)DisabilityClaimStatus.Pending).FirstOrDefault();
            query.Result = new ServiceQueryResult<DisabilityClaimModel>(new List<DisabilityClaimModel>() { disabilityClaimDataModel });

            return systemMessageCollection;
        }
    }
}