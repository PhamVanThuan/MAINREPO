using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Defines common methods and properties for all secured mortgageloan accounts.
    /// </summary>
    public interface IAccountMortgageLoan
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

        IHOCAccount HOCAccount { get;}
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

        /// <summary>
        /// The Capitalised Life of the MortgageLoanAccount
        /// (No calcs or interest should be calculated on capitalised life)
        /// </summary>
        double CapitalisedLife { get;}

        /// <summary>
        /// Calls the stored procedure [2AM].[dbo].pLoanCalcAccruedInterest
        /// </summary>
        /// <param name="FinancialServiceKey">Identifies the FinancialService to be checked</param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="Interest">An output parameter that will receive the value of any accrued interest</param>
        /// <param name="LoyaltyBenefit">An output parameter that will receive the value of any Loyalty Benefit</param>
        /// <param name="CoPayment">An output parameter that will receive the value of any CoPayment</param>
        /// <param name="CAPPrePayment">An output parameter that will receive the value of any CAP PrePayment</param>
        void CalcAccruedInterest(int FinancialServiceKey, DateTime FromDate, DateTime ToDate, out double Interest, out double LoyaltyBenefit, out double CoPayment, out double CAPPrePayment);


    }
}
