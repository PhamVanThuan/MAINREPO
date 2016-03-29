using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class IsAddressAClientAddressQuery : ServiceQuery<IsAddressAClientAddressQueryResult>, IDomainQueryQuery
    {
        [Required]
        public int AddressKey { get; protected set; }

        [Required]
        public int ClientKey { get; protected set; }

        public IsAddressAClientAddressQuery(int addressKey, int clientKey)
        {
            this.AddressKey = addressKey;
            this.ClientKey = clientKey;
        }
    }
}