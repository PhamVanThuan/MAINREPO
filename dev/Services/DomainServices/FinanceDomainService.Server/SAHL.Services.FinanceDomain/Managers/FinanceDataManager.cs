using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.FinanceDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Enum;

namespace SAHL.Services.FinanceDomain.Managers
{
    public class FinanceDataManager : IFinanceDataManager
    {
        private IDbFactory dbFactory;

        public FinanceDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int? GetVariableLoanFinancialServiceKeyByAccount(int accountNumber)
        {
            var query = new GetFinancialServiceKeyByServiceTypeStatement(accountNumber, FinancialServiceTypeEnum.VariableLoan);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(query);
            }
        }

        public int GetFinancialTransactionKeyByReference(string reference)
        {
            var query = new GetFinancialTransactionKeyByReferenceStatement(reference);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<int>(query);
            }
        }

        public void UpdateInvoiceStatus(int thirdPartyInvoiceKey, InvoiceStatus invoiceStatus)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var query = new UpdateInvoiceStatusStatement(thirdPartyInvoiceKey, invoiceStatus);
                db.Update(query);
                db.Complete();
            }
        }
    }
}
