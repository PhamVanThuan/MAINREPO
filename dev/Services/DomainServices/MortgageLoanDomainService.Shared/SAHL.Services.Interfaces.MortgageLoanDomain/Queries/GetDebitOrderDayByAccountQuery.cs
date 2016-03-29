using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.MortgageLoanDomain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.MortgageLoanDomain.Queries
{
    public class GetDebitOrderDayByAccountQuery : ServiceQuery<GetDebitOrderDayByAccountQueryResult>, IMortgageLoanDomainQuery, ISqlServiceQuery<GetDebitOrderDayByAccountQueryResult>
    {

        public GetDebitOrderDayByAccountQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public int AccountKey { get; protected set; }
    }
}
