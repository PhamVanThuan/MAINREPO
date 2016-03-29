using System;
using SAHL.Core.Data;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear.Statements;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear
{
    public class AttorneyInvoicesNotProcessedThisYearDataManager : IAttorneyInvoicesNotProcessedThisYearDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesNotProcessedThisYearDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void IncrementCountAndIncreaseYearlyValue(decimal invoiceValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementCountAndIncreaseYearlyValueStatement(invoiceValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void DecrementCountAndDecreaseYearlyValue(decimal invoiceValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new DecrementCountAndDecreaseYearlyValueStatement(invoiceValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void AdjustYearlyValue(decimal adjustmentValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new AdjustYearlyValueStatement(adjustmentValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }
    }
}