using System;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AccountVariFixLoan : Account, IAccountVariFixLoan
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
#warning Rules Turned off here - Paul C (Can not run CreateAccountForApplication from WF. GetSecuredMOrtgageLoan fails (returns Null))
            //Rules.Add("AttributeThirtyTermVarifixRule");
        }

        #region Properties

        /// <summary>
        /// Returns the fixed loan mortgage loan object.
        /// This will return the first mortgage loan found by the query.
        /// i.e.: There could be more than one closed Fixed ML, this will return the last one that was active
        /// if no open ML are found.
        /// </summary>
        public IMortgageLoan FixedSecuredMortgageLoan
        {
            get
            {
                string hql = UIStatementRepository.GetStatement("FinancialService", "GetSecuredMortgageLoan");

                SimpleQuery<MortgageLoan_DAO> q1 = new SimpleQuery<MortgageLoan_DAO>(hql, _DAO.Key,
                    Convert.ToInt32(Globals.MortgageLoanPurposeGroups.MortgageLoan),
                    Convert.ToInt32(Globals.FinancialServiceTypes.FixedLoan)
                    );
                MortgageLoan_DAO[] Morts = q1.Execute();
                if (Morts.Length == 0)
                    return null;
                else
                {
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return bmtm.GetMappedType<IMortgageLoan>(Morts[0]);
                }
            }
        }

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

        //public IFinancialService VariableFinancialService
        //{
        //    get { return MortgageLoanAccountHelper.GetVariableFinancialService(this as Account); }
        //}

        //public IFinancialService FixedFinancialService
        //{
        //    get { return MortgageLoanAccountHelper.GetFixedFinancialService(this as Account); }
        //}

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
                // Get the Current balance on the variable leg of the mortgage loan
                double loanCurrentBalance = this.SecuredMortgageLoan.CurrentBalance;

                // Add the Current balance on the fixed leg of the mortgage loan if there is one
                if (this.FixedSecuredMortgageLoan != null)
                    loanCurrentBalance += this.FixedSecuredMortgageLoan.CurrentBalance;

                return loanCurrentBalance;
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
    }
}