using SAHL.Core.Services;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Queries
{
    public class ClientHasOpenAccountOrApplicationQuery : ServiceQuery<ClientHasOpenAccountOrApplicationQueryResult>, IClientDomainQuery
    {
        [Required]
        public int ClientKey { get; protected set; }

        public ClientHasOpenAccountOrApplicationQuery(int clientKey)
        {
            ClientKey = clientKey;
        }
    }
}