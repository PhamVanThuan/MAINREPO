using SAHL.Core.Data;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements;
using System;

namespace SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly
{
    public class AccountsPaidForAttorneyInvoicesDataManager : IAccountsPaidForAttorneyInvoicesDataManager
    {
        private readonly IDbFactory dbFactory;

        public AccountsPaidForAttorneyInvoicesDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int GetDistinctCountOfAccountsForAttorney(Guid guid)
        {
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetDistinctCountOfAccountsForAttorneyStatement(guid);
                int count = context.SelectOne<int>(statement);
                return count;
            }
        }

        public void InsertRecord(Guid thirdPartyId, int thirdPartyInvoiceKey, int accountKey)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new InsertAccountsPaidForAttorneyInvoicesMonthlyStatement(thirdPartyInvoiceKey, accountKey, thirdPartyId);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public void ClearAccountsPaidForAttorneyInvoicesMonthly()
        {
            using (var context = dbFactory.NewDb().InAppContext())
            {
                var statement = new ClearAccountsPaidForAttorneyInvoicesMonthlyStatement();
                context.Delete(statement);
                context.Complete();
            }
        }
    }
}