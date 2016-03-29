using Common.Enums;
using System;
using System.Data.SqlClient;

namespace Automation.DataAccess.DataHelper
{
    /// <summary>
    ///
    /// </summary>
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Fetches a financial adjustment for an account by its rate override type and general status
        /// </summary>
        /// <param name="AccountKey">Account Number</param>
        /// <param name="financialAdjustmentTypeSource">financialAdjustmentTypeSource</param>
        /// <param name="fadjStatus">FinancialAdjustmentStatus</param>
        /// <returns></returns>
        public QueryResults GetFinAdjustmentByAccountFinAdjustmentTypeAndStatus(int AccountKey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource,
            FinancialAdjustmentStatusEnum fadjStatus)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                    @"select * from [2am].fin.FinancialAdjustment fa
                    join [2am].dbo.FinancialService fs on fa.financialServiceKey=fs.financialServiceKey
                    join [2am].fin.FinancialAdjustmentTypeSource fats
                    on fa.financialAdjustmentTypeKey=fats.financialAdjustmentTypeKey
                    and fa.financialAdjustmentSourceKey=fats.financialAdjustmentSourceKey
                    where fs.AccountKey = {0} and fats.financialAdjustmentTypeSourceKey = {1}
                    and fa.FinancialAdjustmentStatusKey = {2}", AccountKey, (int)financialAdjustmentTypeSource, (int)fadjStatus);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the OfferInformationFinancialAdjustment record for a given Offer only if the provided FinancialAdjustmentTypeSource exists against the offer.
        /// </summary>
        /// <param name="offerKey">Offer</param>
        /// <param name="finAdjustmentTypeSource">FinancialAdjustmentTypeSource</param>
        /// <returns></returns>
        public QueryResults GetOfferInformationFinancialAdjustmentByOfferAndType(int offerKey, FinancialAdjustmentTypeSourceEnum finAdjustmentTypeSource)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(@"select oifa.*
                from [2am].[dbo].offer ofr (nolock)
                join (select max(OfferInformationKey) as OfferInformationKey, OfferKey
                from [2am].[dbo].offerInformation (nolock)
                group by offerKey) as OIKeys
                on OIKeys.OfferKey = ofr.OfferKey
                join [2am].[dbo].OfferInformationFinancialAdjustment oifa (nolock)
                on OIKeys.OfferInformationKey = oifa.OfferInformationKey
                where oifa.financialAdjustmentTypeSourceKey = {0} and ofr.offerKey = {1}", (int)finAdjustmentTypeSource, offerKey);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts a rate override against the variable financial service for an account.
        /// </summary>
        /// <param name="accountKey">Account</param>
        /// <param name="finAdjustmentTypeSource">FinancialAdjustmentTypeSource</param>
        /// <param name="discount">Discount to apply to loan for Staff Loans/Discounted Link Rate</param>
        public void StartFinancialAdjustment(int accountKey, FinancialAdjustmentTypeSourceEnum finAdjustmentTypeSource, double discount)
        {
            var proc = new SQLStoredProcedure { Name = "test.StartFinancialAdjustment" };
            proc.AddParameter(new SqlParameter("@accountKey", accountKey));
            proc.AddParameter(new SqlParameter("@financialAdjustmentTypeSourceKey", (int)finAdjustmentTypeSource));
            proc.AddParameter(new SqlParameter("@discount", discount));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Deletes any financial adjustments linked to the account
        /// </summary>
        /// <param name="accountKey"></param>
        public void CancelFinancialAdjustments(int accountKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.CancelFinancialAdjustments" };
            proc.AddParameter(new SqlParameter("@accountKey", accountKey));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Gets all the accounts with the specified rate overrides
        /// </summary>
        /// <param name="fAdjTypeSource"></param>
        /// <param name="fAdjStatus"></param>
        /// <returns></returns>
        public QueryResults GetFinancialAdjustmentsByTypeAndStatus(FinancialAdjustmentTypeSourceEnum fAdjTypeSource, FinancialAdjustmentStatusEnum fAdjStatus)
        {
            SQLStatement statement = new SQLStatement();
            string query =
            String.Format(@"select acc.accountKey,acc.rrr_productKey as productKey, fas.Description as SourceDescription,
                fat.Description as TypeDescription, convert(varchar(12),fa.fromdate,111) as FromDate, fa.FinancialAdjustmentKey
                from [2am].[dbo].account acc (nolock)
                join [2am].[dbo].FinancialService fs (nolock) on acc.accountKey = fs.accountKey
                join [2am].[fin].FinancialAdjustment fa (nolock) on fs.financialServiceKey=fa.financialServiceKey
                join [2am].[fin].FinancialAdjustmentTypeSource fats
                on fa.financialAdjustmentTypeKey=fats.financialAdjustmentTypeKey and fa.financialAdjustmentSourceKey=fats.financialAdjustmentSourceKey
                join [2am].[fin].FinancialAdjustmentType fat on fa.FinancialAdjustmentTypeKey=fat.FinancialAdjustmentTypeKey
                join [2am].[fin].FinancialAdjustmentSource fas on fa.FinancialAdjustmentSourceKey=fas.FinancialAdjustmentSourceKey
                where fa.financialAdjustmentStatusKey = {0} and fats.financialAdjustmentTypeSourceKey = {1}
                and acc.accountStatusKey=1
                group by
                acc.accountKey, fas.Description, fat.Description, acc.rrr_productKey, convert(varchar(12), fa.fromdate,111), fa.FinancialAdjustmentKey
                having count(acc.accountKey) = 1
                order by newid()", (int)fAdjStatus, (int)fAdjTypeSource);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        public QueryResults GetAppliedOfferInformationRateAdjustment(int offerKey, double expectedAdjustment, string expectedRateAdjustmentElement)
        {
            var statement = new SQLStatement();
            string sql = string.Format(@"
                select * from [2am].dbo.offer o
                join [2am].dbo.offerinformation oi on o.offerkey=oi.offerkey
                join [2am].dbo.offerinformationvariableloan vl on oi.offerinformationkey=vl.offerinformationkey
                join [2am].dbo.offerinformationfinancialAdjustment fadj on vl.offerinformationkey=fadj.offerinformationkey
                join [2am].dbo.offerinformationappliedRateAdjustment ra on fadj.offerinformationfinancialAdjustmentkey=ra.offerinformationfinancialAdjustmentkey
                join [2am].dbo.rateAdjustmentElement rae on ra.rateAdjustmentElementKey = rae.rateAdjustmentElementKey
                where o.offerkey={0}
                and fadj.discount = {1}
                and rae.description = '{2}'
                and fadj.financialAdjustmentTypeSourceKey = rae.financialAdjustmentTypeSourceKey", offerKey, expectedAdjustment, expectedRateAdjustmentElement);
            statement.StatementString = sql;
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}