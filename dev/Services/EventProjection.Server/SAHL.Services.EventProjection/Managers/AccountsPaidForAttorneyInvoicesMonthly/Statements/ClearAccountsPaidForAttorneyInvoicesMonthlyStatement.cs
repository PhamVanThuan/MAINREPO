using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements
{
    public class ClearAccountsPaidForAttorneyInvoicesMonthlyStatement : ISqlStatement<object>
    {
        public string GetStatement()
        {
            return "DELETE FROM [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly]";
        }
    }
}
