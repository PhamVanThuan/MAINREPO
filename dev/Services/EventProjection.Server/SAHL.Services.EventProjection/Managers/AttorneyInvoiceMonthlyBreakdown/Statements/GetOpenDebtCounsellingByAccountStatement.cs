using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class GetOpenDebtCounsellingByAccountStatement : ISqlStatement<DebtCounsellingDataModel>
    {
        public int AccountKey { get; protected set; }

        public GetOpenDebtCounsellingByAccountStatement(int accountKey)
        {
            this.AccountKey = accountKey;
        }

        public string GetStatement()
        {
            return "select top 1 * from [2am].debtcounselling.debtcounselling where accountKey = @accountKey and debtCounsellingStatusKey = 1";
        }
    }
}