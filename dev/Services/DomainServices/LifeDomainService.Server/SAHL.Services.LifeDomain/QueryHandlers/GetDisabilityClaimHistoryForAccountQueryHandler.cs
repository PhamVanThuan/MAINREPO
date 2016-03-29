﻿using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.QueryHandlers
{
    public class GetDisabilityClaimHistoryForAccountQueryHandler : IServiceQueryHandler<GetDisabilityClaimHistoryForAccountQuery>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public GetDisabilityClaimHistoryForAccountQueryHandler(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetDisabilityClaimHistoryForAccountQuery query)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            IList<DisabilityClaimDetailModel> disabilityClaims = lifeDomainDataManager.GetDisabilityClaimHistory(query.AccountKey).ToList();

            query.Result = new ServiceQueryResult<DisabilityClaimDetailModel>(disabilityClaims);

            return systemMessageCollection;
        }
    }
}