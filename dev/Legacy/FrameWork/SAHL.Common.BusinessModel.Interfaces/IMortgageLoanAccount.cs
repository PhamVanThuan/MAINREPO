using System;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Defines common methods and properties for all secured mortgageloan accounts.
    /// </summary>
    public interface IMortgageLoanAccount : IAccount
    {
        /// <summary>
        /// Returns the secured variable loan mortgage loan object.
        /// </summary>
        IMortgageLoan SecuredMortgageLoan
        {
            get;
        }

        /// <summary>
        /// Returns a collection of all the unsecured mortageloans.
        /// </summary>
        IReadOnlyEventList<IMortgageLoan> UnsecuredMortgageLoans
        {
            get;
        }

        IAccountHOC HOCAccount { get; }

        IAccountLifePolicy LifePolicyAccount { get; }

        IReadOnlyEventList<ILegalEntity> MainApplicants
        {
            get;
        }

        IReadOnlyEventList<ILegalEntity> ActiveMainApplicants
        {
            get;
        }

        IReadOnlyEventList<ILegalEntity> Suretors
        {
            get;
        }

        IApplicationMortgageLoan CurrentMortgageLoanApplication
        {
            get;
        }

        /// <summary>
        /// The Capitalised Life of the MortgageLoanAccount
        /// (No calcs or interest should be calculated on capitalised life)
        /// </summary>
        double CapitalisedLife { get; }

        /// <summary>
        /// The Current Balance of the Mortgage Loan
        /// This is the sum of the current balances on the variable and the fixed (if exists) legs
        /// </summary>
        double LoanCurrentBalance { get; }

        ///// <summary>
        ///// Calls the stored procedure [2AM].[dbo].pLoanCalcAccruedInterest
        ///// </summary>
        ///// <param name="FinancialServiceKey">Identifies the FinancialService to be checked</param>
        ///// <param name="FromDate"></param>
        ///// <param name="ToDate"></param>
        ///// <param name="Interest">An output parameter that will receive the value of any accrued interest</param>
        ///// <param name="LoyaltyBenefit">An output parameter that will receive the value of any Loyalty Benefit</param>
        ///// <param name="CoPayment">An output parameter that will receive the value of any CoPayment</param>
        ///// <param name="CAPPrePayment">An output parameter that will receive the value of any CAP PrePayment</param>
        //void CalcAccruedInterest(int FinancialServiceKey, DateTime FromDate, DateTime ToDate, out double Interest, out double LoyaltyBenefit, out double CoPayment, out double CAPPrePayment);

        /// <summary>
        /// Calculates the intrest from the beginning of the month to today and another intrest rate to the end of the month.
        ///
        /// </summary>
        /// <param name="financialServiceKey">The financial service key.</param>
        /// <param name="interestMonthToDate">The interest of month to date.</param>
        /// <param name="interestTotalForMonth">The interest total for month.</param>
        void CalculateInterest(int financialServiceKey, out double interestMonthToDate, out double interestTotalForMonth);

        /// <summary>InterestTotalforMonth
        /// Get the Balance on account based on a date
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        double GetAccountBalanceByDate(DateTime dte);

        /// <summary>
        /// A property that shows if an account is under Cancellation
        /// </summary>
        bool CancellationRegistered
        {
            get;
        }
    }
}