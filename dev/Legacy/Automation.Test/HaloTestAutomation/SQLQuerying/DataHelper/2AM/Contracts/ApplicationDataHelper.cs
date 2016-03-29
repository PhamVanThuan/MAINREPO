using Automation.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.DataAccess.DataHelper._2AM.Contracts
{
    public class ApplicationDataHelper : IApplicationDataHelper
    {
        private IDataContext dataContext;
        public ApplicationDataHelper(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        /// <summary>
        ///   Retrieves the latest offer information record for an offer and an indication of whether or not OfferInformationRateOverride
        ///   or OfferInformationVarifixLoan records exist for the offer.
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>OfferInformation.*,OfferInformationVarifixLoan.OfferInformationKey,OfferInformationRateOverride.OfferInformationKey</returns>
        public QueryResults GetLatestOfferInformationByOfferKey(int offerKey)
        {
            string query =
                string.Format(@"select oi.*, isnull(oiv.offerinformationkey,0) as VariFixOI, isnull(oifa.offerinformationkey,0) as EdgeOI,
                p.description as ProductDescription, oivl.CategoryKey, oivl.Term, oivl.ExistingLoan, oivl.CashDeposit, oivl.PropertyValuation, oivl.HouseholdIncome, oivl.FeesTotal, oivl.InterimInterest, oivl.MonthlyInstalment, oivl.LifePremium,
                oivl.HOCPremium, oivl.MinLoanRequired, oivl.MinBondRequired, oivl.PreApprovedAmount, oivl.MinCashAllowed, oivl.MaxCashAllowed, oivl.LoanAmountNoFees, oivl.RequestedCashAmount, oivl.LoanAgreementAmount, oivl.BondToRegister,
                oivl.LTV, oivl.PTI, oivl.MarketRate, oivl.SPVKey, oivl.EmploymentTypeKey, oivl.RateConfigurationKey, oivl.CreditMatrixKey, oivl.CreditCriteriaKey
                from [2am].dbo.offer o (nolock)
                join (
                select max(offerinformationkey) oikey, offerkey
                from [2am].dbo.offerinformation oi (nolock) where oi.offerkey = {0}
                group by oi.offerkey
                ) as maxoi on o.offerkey=maxoi.offerkey
                join [2am].dbo.offerinformation oi (nolock) on maxoi.oikey=oi.offerinformationkey
                left join [2am].dbo.offerinformationvariableloan oivl (nolock) on oi.offerinformationkey =oivl.offerinformationkey
                left join [2am].dbo.offerinformationFinancialAdjustment oifa (nolock) on oi.offerinformationkey = oifa.offerinformationkey
                left join [2am].dbo.offerinformationvarifixloan oiv (nolock) on oi.offerinformationkey =oiv.offerinformationkey
                left join [2am].dbo.product p (nolock) on oi.productkey=p.productkey
				where o.offerkey = {1}", offerKey, offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}
