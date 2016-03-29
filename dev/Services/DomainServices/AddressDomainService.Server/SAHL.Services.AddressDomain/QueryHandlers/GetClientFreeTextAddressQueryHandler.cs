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
    public class GetClientFreeTextAddressQueryHandler : IServiceQueryHandler<GetClientFreeTextAddressQuery>
    {
        public IAddressDataManager addressDatamanager { protected set; get; }
        public GetClientFreeTextAddressQueryHandler(IAddressDataManager addressDatamanager)
        {
            this.addressDatamanager = addressDatamanager;
        }

        public ISystemMessageCollection HandleQuery(GetClientFreeTextAddressQuery query)
        {
            GetClientFreeTextAddressQueryResult result = null;
            
            var systemMessages = SystemMessageCollection.Empty();
            var freeTextAddress = addressDatamanager.FindAddressFromFreeTextAddress(query.Address).FirstOrDefault();
            LegalEntityAddressDataModel clientAddress = null;
            if (freeTextAddress != null)
            {
                clientAddress = addressDatamanager.GetExistingActiveClientAddress(query.ClientKey, freeTextAddress.AddressKey, query.AddressTypeKey);
            }
            if (clientAddress != null)
            {
                result = new GetClientFreeTextAddressQueryResult()
                {
                    ClientAddressKey = clientAddress.LegalEntityAddressKey,
                    ClientKey = clientAddress.LegalEntityKey,
                    AddressKey = clientAddress.AddressKey,
                    AddressTypeKey = clientAddress.AddressTypeKey,
                };
            }
            query.Result = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(new List<GetClientFreeTextAddressQueryResult>() 
            { 
               result
            });
            return systemMessages;
        }
    }
}
