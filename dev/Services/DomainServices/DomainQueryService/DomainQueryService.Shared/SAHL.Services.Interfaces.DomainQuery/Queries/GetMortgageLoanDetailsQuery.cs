using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetMortgageLoanDetailsQuery : ServiceQuery<GetMortgageLoanDetailsQueryResult>, IDomainQueryQuery, ISqlServiceQuery<GetMortgageLoanDetailsQueryResult>
    {
        [Required]
        public int AccountKey { get; protected set; }

        public GetMortgageLoanDetailsQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}