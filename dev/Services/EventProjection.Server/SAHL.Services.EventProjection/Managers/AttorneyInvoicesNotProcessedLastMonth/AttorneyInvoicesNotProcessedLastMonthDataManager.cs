using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth.Statements;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth
{
    public class AttorneyInvoicesNotProcessedLastMonthDataManager : IAttorneyInvoicesNotProcessedLastMonthDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoicesNotProcessedLastMonthDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void UpdateAttorneyInvoicesNotProcessedLastMonth(AttorneyInvoicesNotProcessedThisMonthDataModel model)
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(new UpdateAttorneyInvoicesNotProcessedLastMonthStatement(model.Count, model.Value));
                db.Complete();
            }
        }
    }
}