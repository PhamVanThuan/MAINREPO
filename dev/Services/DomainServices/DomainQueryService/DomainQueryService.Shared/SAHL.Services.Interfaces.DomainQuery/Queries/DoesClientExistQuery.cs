using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class DoesClientExistQuery : ServiceQuery<DoesClientExistQueryResult>, IDomainQueryQuery
    {
        [Required]
        public int ClientKey { get; protected set; }

        public DoesClientExistQuery(int clientKey)
        {
            this.ClientKey = clientKey;
        }
    }
}