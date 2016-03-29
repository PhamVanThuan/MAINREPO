using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown
{
    public class AttorneyInvoiceMonthlyBreakdownDataManager : IAttorneyInvoiceMonthlyBreakdownDataManager
    {
        private readonly IDbFactory dbFactory;

        public AttorneyInvoiceMonthlyBreakdownDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AdjustUnprocessedCount(Guid thirdpartyId, int valueToAdd)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new AdjustUnprocessedStatement(thirdpartyId, valueToAdd);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public void ClearAttorneyInvoiceMonthlyBreakdownManagerTable()
        {
            using (var context = dbFactory.NewDb().InAppContext())
            {
                var statement = new ClearAttorneyInvoiceMonthlyBreakdownManagerTableStatement();
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public void AdjustProcessedCount(Guid thirdpartyId, int valueToAdd)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var incrementProcessedStatement = new AdjustProcessedStatement(thirdpartyId, valueToAdd);
                context.ExecuteNonQuery(incrementProcessedStatement);
                context.Complete();
            }
        }

        public void MergeAttorneyMonthlyBreakdownRecordForAttorney(Guid thirdpartyId, string attorneyRegisteredName)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new MergeAttorneyMonthlyBreakdownRecordForAttorneyStatement(thirdpartyId, attorneyRegisteredName);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public string GetRegisteredNameForAttorney(Guid thirdpartyId)
        {
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetAttorneyRegisteredNameStatement(thirdpartyId);
                var RegisteredName = context.SelectOne(statement);
                return RegisteredName;
            }
        }

        public void IncrementRejectedCount(Guid thirdPartyId)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementRejectedCountStatement(thirdPartyId);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public Guid GetThirdPartyIdByThirdPartyInvoiceKey(int thirdPartyInvoiceKey)
        {
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetThirdPartyIdForInvoiceStatement(thirdPartyInvoiceKey);
                return context.SelectOne(statement);
            }
        }

        public void IncrementPaidCount(Guid thirdPartyId)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new IncrementPaidCountStatement(thirdPartyId);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public ThirdPartyInvoiceDataModel GetThirdPartyInvoiceByThirdPartyInvoiceKey(int thirdPartyInvoiceKey)
        {
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetThirdPartyInvoiceStatement(thirdPartyInvoiceKey);
                return context.SelectOne(statement);
            }
        }

        public DebtCounsellingDataModel GetOpenDebtCounsellingByAccountKey(int accountKey)
        {
            using (var context = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetOpenDebtCounsellingByAccountStatement(accountKey);
                return context.SelectOne(statement);
            }
        }

        public void UpdatePaymentFieldsForAttorney(Guid attorneyId, decimal debtReview, decimal paidBySPV, decimal capitalised)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new AdjustPaymentFieldsStatement(capitalised, debtReview, paidBySPV, attorneyId);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public void AdjustAccountsPaidCount(Guid attorneyId, int value)
        {
            using (var context = this.dbFactory.NewDb().InAppContext())
            {
                var statement = new AdjustAccountsPaidStatement(attorneyId, value);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }
    }
}