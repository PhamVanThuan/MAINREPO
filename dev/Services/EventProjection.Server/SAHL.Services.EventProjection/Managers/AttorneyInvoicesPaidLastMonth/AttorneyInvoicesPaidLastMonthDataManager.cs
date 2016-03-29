using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth.Statements;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth
{
    public class AttorneyInvoicesPaidLastMonthDataManager : IAttorneyInvoicesPaidLastMonthDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesPaidLastMonthDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void UpdateAttorneyInvoicesPaidLastMonth(AttorneyInvoicesPaidThisMonthDataModel model)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new UpdateAttorneyInvoicesPaidLastMonthStatement(model.Count, model.Value);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

    }
}