using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetClientBankAccountQuery : ServiceQuery<GetClientBankAccountQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetClientBankAccountQueryResult>
    {
        [Required]
        public int ClientBankAccountKey { get; protected set; }

        public GetClientBankAccountQuery(int clientBankAccountKey)
        {
            this.ClientBankAccountKey = clientBankAccountKey;
        }
    }
}