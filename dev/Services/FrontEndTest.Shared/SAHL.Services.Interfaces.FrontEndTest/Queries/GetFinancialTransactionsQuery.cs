using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetFinancialTransactionsQuery : ServiceQuery<GetFinancialTransactionsQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetFinancialTransactionsQueryResult>
    {
        public GetFinancialTransactionsQuery(int financialServiceKey)
        {
            this.FinancialServiceKey = financialServiceKey;
        }

        public int FinancialServiceKey { get; protected set; }
    }
}
