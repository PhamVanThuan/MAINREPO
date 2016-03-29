using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.MortgageLoanDomain;
using SAHL.Services.Interfaces.MortgageLoanDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DomainServices
{
    public class MortgageLoanDomainService : DomainServiceClientBase<IMortgageLoanDomainServiceClient>, IMortgageLoanDomainService
    {
        public MortgageLoanDomainService(IMortgageLoanDomainServiceClient mortgageLoanDomainServiceClient)
            : base(mortgageLoanDomainServiceClient)
        { }

        public int GetDebitOrderDayByAccount(int accountKey)
        {
            var query = new GetDebitOrderDayByAccountQuery(accountKey);
            var response = this.PerformQuery(query);
            var result = query.Result.Results.FirstOrDefault();
            return result.DebitOrderDay;
        }
    }
}
