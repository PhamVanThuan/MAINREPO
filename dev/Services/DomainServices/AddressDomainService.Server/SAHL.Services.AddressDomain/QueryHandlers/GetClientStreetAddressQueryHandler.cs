using SAHL.Core.Services;
using SAHL.Services.AddressDomain.Managers;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.AddressDomain.QueryHandlers
{
    public class GetClientStreetAddressQueryHandler : IServiceQueryHandler<GetClientStreetAddressQuery>
    {
        public IAddressDataManager addressDatamanager { protected set; get; }
        public GetClientStreetAddressQueryHandler(IAddressDataManager addressDatamanager)
        {
            this.addressDatamanager = addressDatamanager;
        }

        public ISystemMessageCollection HandleQuery(GetClientStreetAddressQuery query)
        {
            GetClientStreetAddressQueryResult result = null;
            
            LegalEntityAddressDataModel clientAddress = null;
            var systemMessages = SystemMessageCollection.Empty();
            var streetAddress = addressDatamanager.FindAddressFromStreetAddress(query.Address).FirstOrDefault();
            if (streetAddress != null)
            {
                clientAddress = addressDatamanager.GetExistingActiveClientAddress(query.ClientKey, streetAddress.AddressKey, query.AddressTypeKey);
            }
            if (clientAddress != null)
            {
                result = new GetClientStreetAddressQueryResult()
                {
                    ClientAddressKey = clientAddress.LegalEntityAddressKey,
                    ClientKey = clientAddress.LegalEntityKey,
                    AddressKey = clientAddress.AddressKey,
                    AddressTypeKey = clientAddress.AddressTypeKey,
                };
            }
            query.Result = new ServiceQueryResult<GetClientStreetAddressQueryResult>(new List<GetClientStreetAddressQueryResult>() 
            { 
                result
            });
            return systemMessages;
        }
    }
}
