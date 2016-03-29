﻿using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.QueryHandlers
{
    public class GetDisabilityClaimByKeyQueryHandler : IServiceQueryHandler<GetDisabilityClaimByKeyQuery>
    {
        private ILifeDomainDataManager lifeDomainDataManager;

        public GetDisabilityClaimByKeyQueryHandler(ILifeDomainDataManager lifeDomainDataManager)
        {
            this.lifeDomainDataManager = lifeDomainDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetDisabilityClaimByKeyQuery query)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            DisabilityClaimDetailModel disabilityClaim = lifeDomainDataManager.GetDisabilityClaimDetailByKey(query.DisabilityClaimKey);

            query.Result = new ServiceQueryResult<DisabilityClaimDetailModel>(new List<DisabilityClaimDetailModel>() { disabilityClaim });

            return systemMessageCollection;
        }
    }
}