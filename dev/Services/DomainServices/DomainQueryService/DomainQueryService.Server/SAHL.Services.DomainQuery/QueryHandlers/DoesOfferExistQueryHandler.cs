using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Application;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Collections.Generic;

namespace SAHL.Services.DomainQuery.QueryHandlers
{
    public class DoesOfferExistQueryHandler : IServiceQueryHandler<DoesOfferExistQuery>
    {
        private IApplicationDataManager applicationDataManager;

        public DoesOfferExistQueryHandler(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public ISystemMessageCollection HandleQuery(DoesOfferExistQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            bool offerExists = applicationDataManager.DoesOfferExist(query.OfferKey);
            var queryResult = new DoesOfferExistQueryResult { OfferExist = offerExists };
            query.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { queryResult });

            return messages;
        }
    }
}