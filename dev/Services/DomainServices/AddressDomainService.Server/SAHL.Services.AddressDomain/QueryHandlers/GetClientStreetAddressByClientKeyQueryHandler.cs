using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.AddressDomain.QueryHandlers
{
    public class GetClientStreetAddressByClientKeyQueryHandler : IServiceQueryHandler<GetClientStreetAddressByClientKeyQuery>
    {
        public IAddressDataManager addressDatamanager { protected set; get; }
        public GetClientStreetAddressByClientKeyQueryHandler(IAddressDataManager addressDatamanager)
        {
            this.addressDatamanager = addressDatamanager;
        }

        public ISystemMessageCollection HandleQuery(GetClientStreetAddressByClientKeyQuery query)
        {
            List<GetClientStreetAddressByClientKeyQueryResult> result = new List<GetClientStreetAddressByClientKeyQueryResult>();

            IEnumerable<AddressDataModel> clientAddressCollection = null;
            var systemMessages = SystemMessageCollection.Empty();
            clientAddressCollection = addressDatamanager.GetExistingActiveClientStreetAddressByClientKey(query.ClientKey);

            if (clientAddressCollection != null)
            {
                result = clientAddressCollection.Select(x => new GetClientStreetAddressByClientKeyQueryResult
                {
                    StreetName = x.StreetName,
                    StreetNumber = x.StreetNumber,
                    City = x.RRR_CityDescription,
                    Suburb = x.RRR_SuburbDescription,
                    PostalCode = x.RRR_PostalCode
                }).ToList();
            }

            query.Result = new ServiceQueryResult<GetClientStreetAddressByClientKeyQueryResult>(result);
            return systemMessages;
        }
    }
}
