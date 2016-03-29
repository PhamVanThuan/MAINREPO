using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Address;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Collections.Generic;

namespace SAHL.Services.DomainQuery.QueryHandlers
{
    public class IsAddressAClientAddressQueryHandler : IServiceQueryHandler<IsAddressAClientAddressQuery>
    {
        private IAddressDataManager addressDataManager;

        public IsAddressAClientAddressQueryHandler(IAddressDataManager addressDataManager)
        {
            this.addressDataManager = addressDataManager;
        }

        public ISystemMessageCollection HandleQuery(IsAddressAClientAddressQuery query)
        {
            var systemMessages = SystemMessageCollection.Empty();
            bool addressIsAClientAddress = addressDataManager.IsAddressAClientAddress(query.AddressKey, query.ClientKey);
            query.Result = new ServiceQueryResult<IsAddressAClientAddressQueryResult>(new List<IsAddressAClientAddressQueryResult>() { new IsAddressAClientAddressQueryResult { AddressIsAClientAddress = addressIsAClientAddress } });

            return systemMessages;
        }
    }
}
