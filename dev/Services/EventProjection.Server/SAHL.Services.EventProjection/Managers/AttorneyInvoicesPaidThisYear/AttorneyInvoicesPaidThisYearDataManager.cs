using SAHL.Core.Data;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear.Statements;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear
{
    public class AttorneyInvoicesPaidThisYearDataManager : IAttorneyInvoicesPaidThisYearDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesPaidThisYearDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void IncrementCountAndAddInvoiceToValueColumn(decimal invoiceTotalValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementYearlyCountAndAddToValueColumnStatement(invoiceTotalValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void ClearAttorneyInvoicesPaidThisYear()
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new ClearAttorneyInvoicesPaidThisYearStatement();
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }
    }
}