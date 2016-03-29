using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Queries
{
    public class GetClientStreetAddressByClientKeyQuery : ServiceQuery<GetClientStreetAddressByClientKeyQueryResult>, IAddressDomainQuery, 
        IRequiresClient
    {
        [Required]
        public int ClientKey { get; protected set; }

        public GetClientStreetAddressByClientKeyQuery(int clientKey)
        {
            ClientKey = clientKey;
        }
    }
}
