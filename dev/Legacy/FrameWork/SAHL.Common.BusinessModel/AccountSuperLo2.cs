using System;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AccountSuperLo : Account, IAccountSuperLo
    {
        #region Properties

        /// <summary>
        /// Returns the variable loan mortgage loan object.
        /// </summary>
        public IMortgageLoan SecuredMortgageLoan
        {
            get
            {
                return MortgageLoanAccountHelper.GetSecuredMortgageLoan(this as Account);
            }
        }

        public IReadOnlyEventList<IMortgageLoan> UnsecuredMortgageLoans
        {
            get
            {
                return MortgageLoanAccountHelper.GetUnsecuredMortgageLoans(this as Account);
            }
        }

        public IAccountHOC HOCAccount
        {
            get
            {
                return MortgageLoanAccountHelper.GetHOCAccount(this as Account);
            }
        }

        public IAccountLifePolicy LifePolicyAccount
        {
            get
            {
                return MortgageLoanAccountHelper.GetLifeAccount(this as Account);
            }
        }

        /// <summary>
        /// Returns a list of Legalentities that have a role of main applicant defined on this account.
        /// </summary>
        public IReadOnlyEventList<ILegalEntity> MainApplicants
        {
            get
            {
                return MortgageLoanAccountHelper.GetMainApplicants(this as Account);
            }
        }

        /// <summary>
        /// Returns a list of Legalentities that have a role of main applicant and is marked as active.
        /// </summary>
        public IReadOnlyEventList<ILegalEntity> ActiveMainApplicants
        {
            get
            {
                return MortgageLoanAccountHelper.GetActiveMainApplicants(this as Account);
            }
        }

        /// <summary>
        /// Returns a list of Legalentities that have a role of suretor defined on this account.
        /// </summary>
        public IReadOnlyEventList<ILegalEntity> Suretors
        {
            get
            {
                return MortgageLoanAccountHelper.GetSuretors(this as Account);
            }
        }

        /// <summary>
        /// The Capitalised Life of the MortgageLoanAccount
        /// (No calcs or interest should be calculated on capitalised life)
        /// </summary>
        public double CapitalisedLife
        {
            get
            {
                return MortgageLoanAccountHelper.CapitalisedLife(this as Account);
            }
        }

        public override SAHL.Common.Globals.AccountTypes AccountType
        {
            get { return SAHL.Common.Globals.AccountTypes.MortgageLoan; }
        }

        //LPB
        //public void CalcAccruedInterest(int FinancialServiceKey, DateTime FromDate, DateTime ToDate, out double Interest, out double LoyaltyBenefit, out double CoPayment, out double CAPPrePayment)
        //{
        //    MortgageLoanAccountHelper.CalcAccruedInterest(FinancialServiceKey, FromDate, ToDate, out Interest, out LoyaltyBenefit, out CoPayment, out CAPPrePayment);
        //}
        public void CalculateInterest(int FinancialServiceKey, out double InterestOfMonthToDate, out double InterestTotalforMonth)
        {
            MortgageLoanAccountHelper.CalculateInterest(FinancialServiceKey, out InterestOfMonthToDate, out InterestTotalforMonth);
        }

        /// <summary>
        /// Get the Balance on account based on a date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public double GetAccountBalanceByDate(DateTime date)
        {
            return MortgageLoanAccountHelper.GetAccountBalanceByDate(this, date);
        }

        public IApplicationMortgageLoan CurrentMortgageLoanApplication
        {
            get
            {
                string HQL = "FROM Application_DAO a WHERE a.Account.Key = ? AND a.ApplicationType.Key in (6,7,8) ORDER BY a.Key desc";

                SimpleQuery<Application_DAO> q = new SimpleQuery<Application_DAO>(HQL, this.Key);
                Application_DAO[] list = q.Execute();

                if (list.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationMortgageLoan, Application_DAO>(list[0]);
                }
                return null;
            }
        }

        /// <summary>
        /// The Current Balance of the Mortgage Loan
        /// This is the sum of the current balances on the variable and the fixed (if exists) legs
        /// </summary>
        public double LoanCurrentBalance
        {
            get
            {
                return MortgageLoanAccountHelper.LoanCurrentBalance(this as Account);
            }
        }

        /// <summary>
        /// This oproperty reflects whether the account is under cancellation
        /// </summary>
        public bool CancellationRegistered
        {
            get
            {
                return MortgageLoanAccountHelper.CancellationRegistered(this as Account);
            }
        }

        #endregion Properties

        /// <summary>
        ///
        /// </summary>
        public ISuperLo SuperLo
        {
            get
            {
                string hql = @"select sl from SuperLo_DAO sl where sl.FinancialServiceAttribute.FinancialService.Account.Key = ? and sl.GeneralStatus.Key = ? and sl.FinancialServiceAttribute.GeneralStatus.Key = ? and sl.FinancialServiceAttribute.FinancialServiceAttributeType.Key = ?";
                SimpleQuery<SuperLo_DAO> query = new SimpleQuery<SuperLo_DAO>(hql, this.Key, (int)GeneralStatuses.Active, (int)GeneralStatuses.Active, (int)FinancialServiceAttributeTypes.SuperLo);
                SuperLo_DAO[] result = query.Execute();
                if (result != null && result.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ISuperLo, SuperLo_DAO>(result[0]);
                }
                return null;
            }
        }
    }
}