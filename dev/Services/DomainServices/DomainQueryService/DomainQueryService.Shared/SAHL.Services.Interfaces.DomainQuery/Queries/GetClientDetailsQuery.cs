using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetClientDetailsQuery : ServiceQuery<GetClientDetailsQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetClientDetailsQueryResult>
    {
        [Required]
        public int ClientKey { get; protected set; }

        public GetClientDetailsQuery(int clientKey)
        {
            this.ClientKey = clientKey;
        }
    }
}