using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements
{
    public class GetDistinctCountOfAccountsForAttorneyStatement : ISqlStatement<int>
    {
        public Guid AttorneyId { get; protected set; }

        public GetDistinctCountOfAccountsForAttorneyStatement(Guid attorneyId)
        {
            this.AttorneyId = attorneyId;
        }

        public string GetStatement()
        {
            return @"select count(distinct accountKey) from eventprojection.projection.AccountsPaidForAttorneyInvoicesMonthly where attorneyId = @attorneyId";
        }
    }
}