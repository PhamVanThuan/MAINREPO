using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class IsClientANaturalPersonQuery : ServiceQuery<IsClientANaturalPersonQueryResult>, IDomainQueryQuery
    {
        [Required]
        public int ClientKey { get; protected set; }

        public IsClientANaturalPersonQuery(int clientKey)
        {
            this.ClientKey = clientKey;
        }
    }
}