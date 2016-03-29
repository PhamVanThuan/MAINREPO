using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth
{
    public class AttorneyInvoicesPaidThisMonthDataManager : IAttorneyInvoicesPaidThisMonthDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesPaidThisMonthDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void IncrementCountAndAddInvoiceToValueColumn(decimal invoiceTotalValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementMonthlyCountAndToValueColumnStatement(invoiceTotalValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void ClearAttorneyInvoicesPaidThisMonthStatement()
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new ClearAttorneyInvoicesPaidThisMonthStatement();
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public AttorneyInvoicesPaidThisMonthDataModel GetAttorneyInvoicesPaidThisMonthStatement()
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<AttorneyInvoicesPaidThisMonthDataModel>(new GetAttorneyInvoicesPaidThisMonthStatement());
            }
        }
    }
}