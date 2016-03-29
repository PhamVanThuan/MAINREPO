using System;
using SAHL.Core.Data;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth
{
    public class AttorneyInvoicesNotProcessedThisMonthDataManager : IAttorneyInvoicesNotProcessedThisMonthDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesNotProcessedThisMonthDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void IncrementCountAndIncreaseMonthlyValue(decimal invoiceValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementCountAndIncreaseMonthlyValueStatement(invoiceValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void DecrementCountAndDecreaseMonthlyValue(decimal invoiceValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new DecrementCountAndDecreaseMonthlyValueStatement(invoiceValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public void AdjustMonthlyValue(decimal adjustmentValue)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                var statement = new AdjustMonthlyValueStatement(adjustmentValue);
                db.ExecuteNonQuery(statement);
                db.Complete();
            }
        }

        public AttorneyInvoicesNotProcessedThisMonthDataModel GetAttorneyInvoicesNotProcessedThisMonth()
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne(new GetAttorneyInvoicesNotProcessedThisMonthStatement());
            }
        }
    }
}