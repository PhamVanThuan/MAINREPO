using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetClientAddressQuery : ServiceQuery<GetClientAddressQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetClientAddressQueryResult>
    {
        [Required]
        public int ClientAddressKey { get; protected set; }

        public GetClientAddressQuery(int clientAddressKey)
        {
            this.ClientAddressKey = clientAddressKey;
        }
    }
}