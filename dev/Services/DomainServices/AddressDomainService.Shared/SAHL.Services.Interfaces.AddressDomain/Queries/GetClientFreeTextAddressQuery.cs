using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Queries
{
    public class GetClientFreeTextAddressQuery : ServiceQuery<GetClientFreeTextAddressQueryResult>, IAddressDomainQuery
    {
        [Required]
        public AddressType AddressTypeKey { get; protected set; }

        [Required]
        public FreeTextAddressModel Address { get; protected set; }

        [Required]
        public int ClientKey { get; protected set; }

        public GetClientFreeTextAddressQuery(int clientKey, FreeTextAddressModel address, AddressType addressTypeKey)
        {
            ClientKey = clientKey;
            Address = address;
            AddressTypeKey = addressTypeKey;
        }

    }
}