using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Helpers
{
    /// <summary>
    /// This class is defined as a helper to ensure that common properties and methods are implemented in one place. It will mostly implent what is defined in IMortgageloanAccount.
    /// </summary>
    internal static class MortgageLoanAccountHelper
    {
        public static double MonthlyServiceFee(IAccount account)
        {
            double total = 0D;
            if (account.FinancialServices == null || account.FinancialServices.Count == 0)
                return total;

            foreach (IFinancialService fs in account.FinancialServices)
            {
                foreach (IFee fee in fs.Fees)
                {
                    if (fee.FeeType.TransactionType.Key == (int)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee)
                    {
                        if (fee.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                            total += fee.Amount;
                    }
                }
            }
            return total;
        }

        public static double TotalLoanInstallment(IAccount account)
        {
            //sum of financialservice.payment of account and related children where fs.AccountStatusKey IN (1,5)

            IEventList<IAccount> list = account.GetNonProspectRelatedAccounts();
            list.Add(new DomainMessageCollection(), account);

            double sum = 0;

            foreach (IAccount acc in list)
            {
                if (acc is IMortgageLoanAccount)
                {
                    foreach (IFinancialService fs in acc.FinancialServices)
                    {
                        IMortgageLoan ml = fs as IMortgageLoan;
                        if (ml != null)
                        {
                            if (ml.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open || ml.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
                            {
                                if (ml.MortgageLoanPurpose.MortgageLoanPurposeGroup.Key == (int)SAHL.Common.Globals.MortgageLoanPurposeGroups.MortgageLoan)
                                    sum += ml.Payment;
                            }
                        }
                    }
                }
            }
            return sum;
        }

        public static double TotalShortTermLoanInstallment(IAccount account)
        {
            IEventList<IAccount> list = account.GetNonProspectRelatedAccounts();
            list.Add(new DomainMessageCollection(), account);

            double sum = 0;

            foreach (IAccount acc in list)
            {
                if (acc is IMortgageLoanAccount)
                {
                    foreach (IFinancialService fs in acc.FinancialServices)
                    {
                        IMortgageLoan ml = fs as IMortgageLoan;
                        if (ml != null)
                        {
                            if (ml.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open || ml.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
                            {
                                if (ml.MortgageLoanPurpose.MortgageLoanPurposeGroup.Key == (int)SAHL.Common.Globals.MortgageLoanPurposeGroups.CapitalisedShortTermLoan)
                                    sum += ml.Payment;
                            }
                        }
                    }
                }
            }

            return sum;
        }

        public static double TotalPremium(IAccount account)
        {
            IEventList<IAccount> list = account.GetNonProspectRelatedAccounts();
            list.Add(new DomainMessageCollection(), account);

            double sum = 0;

            foreach (IAccount acc in list)
            {
                if (acc is IAccountHOC || acc is IAccountLifePolicy)
                {
                    foreach (IFinancialService fs in acc.FinancialServices)
                    {
                        if (fs.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open || fs.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
                        {
                            if (acc is IAccountLifePolicy)
                            {
                                if (fs.Balance.BalanceType.Key == (int)SAHL.Common.Globals.BalanceTypes.Loan) // we dont want life arrears
                                {
                                    sum += (acc as IAccountLifePolicy).LifePolicy.MonthlyPremium;
                                }
                            }
                            else if (acc is IAccountHOC)
                            {
                                if (fs.Balance.BalanceType.Key == (int)SAHL.Common.Globals.BalanceTypes.Loan)
                                {
                                    sum += (acc as IAccountHOC).MonthlyPremium;
                                }
                            }
                            else
                            {
                                sum += fs.Payment;
                            }
                        }
                    }
                }
            }

            if (account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open || account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
                sum += GetRegentPremium(account);

            return sum;
        }

        public static double TotalAmountDue(IAccount account)
        {
            double totalAmountDue = Math.Round(TotalLoanInstallment(account) + TotalShortTermLoanInstallment(account) + TotalPremium(account) + MonthlyServiceFee(account),2);
            return totalAmountDue;
        }

        public static double GetRegentPremium(IAccount account)
        {
            string HQL = "from Regent_DAO regent where regent.Key = ? and regent.RegentStatus.Key = ?";
            SimpleQuery<Regent_DAO> q = new SimpleQuery<Regent_DAO>(HQL, account.Key, (int)SAHL.Common.Globals.RegentStatus.NewBusiness);
            Regent_DAO[] regent = q.Execute();

            if (regent.Length == 0)
                return 0;
            else
                return regent[0].RegentPremium;
        }

        public static double TotalArrearBalance(IAccount account)
        {
            double sum = 0D;

            if (account.FinancialServices == null || account.FinancialServices.Count == 0)
                return sum;

            foreach (IFinancialService fs in account.FinancialServices)
            {
                IMortgageLoan ml = fs as IMortgageLoan;
                if (ml != null && ml.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open)
                {
                    sum += ml.ArrearBalance;
                }
            }

            return sum;
        }

        public static double MonthsInArrears(IAccount account)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            return accRepo.GetCurrentMonthsInArrears(account.Key);
        }

        public static double AmortisingInstallment(IAccount account)
        {
            IMortgageLoan vml = account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan) as IMortgageLoan;
            IMortgageLoan fml = null;

            IFinancialService ffs = account.GetFinancialServiceByType(FinancialServiceTypes.FixedLoan);

            if (ffs != null)
                fml = ffs as IMortgageLoan;

            double v = 0;
            double f = 0;

            if (vml != null)
                if (vml.CurrentBalance > 0 && vml.InterestRate > 0 && vml.RemainingInstallments > 0)
                    v = LoanCalculator.CalculateInstallment(vml.CurrentBalance, vml.InterestRate, vml.RemainingInstallments, false);

            if (fml != null && fml.CurrentBalance > 0 && fml.InterestRate > 0 && fml.RemainingInstallments > 0)
                f = LoanCalculator.CalculateInstallment(fml.CurrentBalance, fml.InterestRate, fml.RemainingInstallments, false);
            return v + f;
        }

        public static bool IsInterestOnly(IAccount account)
        {
            foreach (IFinancialService fs in account.FinancialServices)
            {
                IMortgageLoan ml = fs as IMortgageLoan;

                if (ml != null && ml.HasInterestOnly())
                    return true;
            }

            return false;
        }

        public static bool CancellationRegistered(IAccount account)
        {
            //detailType 251 on AccountKey
            string HQL = string.Format("from Detail_DAO det where det.Account.Key = ? and det.DetailType.Key = 251");
            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(HQL, account.Key);
            Detail_DAO[] det = q.Execute();

            if (det.Length == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Calculates the interest.
        /// </summary>
        /// <param name="financialServiceKey">The financial service key.</param>
        /// <param name="interestMonthToDate">The interest of month to date.</param>
        /// <param name="interestTotalForMonth">The interest totalfor month.</param>
        public static void CalculateInterest(int financialServiceKey, out double interestMonthToDate, out double interestTotalForMonth)
        {
            interestMonthToDate = 0.0;
            interestTotalForMonth = 0.0;
            DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime toDate = new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1);
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            IMortgageLoan ml = mlRepo.GetMortgageLoanByKey(financialServiceKey);
            if (ml != null && ml.Account.AccountStatus.Key != (int)AccountStatuses.Closed)
            {
                interestMonthToDate = ml.AccruedInterestMTD.Value;

                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@FSKey", financialServiceKey));
                prms.Add(new SqlParameter("@FromDate", fromDate));
                prms.Add(new SqlParameter("@ToDate", toDate));
                SqlParameter amount = new SqlParameter("@IntAmt", SqlDbType.Float);
                amount.Direction = ParameterDirection.Output;
                prms.Add(amount);

                CastleTransactionsServiceHelper.ExecuteHaloAPI_uiS_OnCastleTranForRead("MortgageLoan", "CalculateInterestByFinancialServiceKey", prms);
                
                if (amount.Value != DBNull.Value)
                {
                    interestTotalForMonth = Convert.ToDouble(prms[3].Value);
                }
            }
        }

        public static double CalcAccountPTI(IAccount account)
        {
            double instalment = 0;
            if (IsInterestOnly(account))
            {
                //Need to calculate the Amortising Instalment and then the PTI
                instalment = AmortisingInstallment(account);
            }
            else
            {
                //Use the current values
                foreach (IFinancialService fs in account.FinancialServices)
                {
                    IMortgageLoan ml = fs as IMortgageLoan;
                    if (ml != null && ml.AccountStatus.Key == (int)AccountStatuses.Open)
                    {
                        instalment += fs.Payment;
                    }
                }
            }

            //Now calc the PTI
            return LoanCalculator.CalculatePTI(instalment, account.GetHouseholdIncome());
        }

        public static double GetCurrentBalance(IAccount account)
        {
            double retval = 0;

            if (account.AccountStatus.Key == (int)AccountStatuses.Open || account.AccountStatus.Key == (int)AccountStatuses.Dormant)
            {
                //get arrears balances
                var currentBalance = (from fs in account.FinancialServices
                                      where fs.Account.Key == account.Key &&
                                            fs.FinancialServiceParent == null &&
                                            fs.Balance.BalanceType.Key == (int)BalanceTypes.Loan
                                      select fs.Balance.Amount).Sum();
                return currentBalance;
            }

            return retval;
        }

        /// <summary>
        /// Returns the variable loan mortgage loan object.
        /// </summary>
        /// <param name="account">The Account to filter by.</param>
        /// <returns>An IMortgageLoan object.</returns>
        public static IMortgageLoan GetSecuredMortgageLoan(IAccount account)
        {
            string hql = UIStatementRepository.GetStatement("FinancialService", "GetSecuredMortgageLoan");

            SimpleQuery<MortgageLoan_DAO> q1 = new SimpleQuery<MortgageLoan_DAO>(hql, account.Key,
                Convert.ToInt32(Globals.MortgageLoanPurposeGroups.MortgageLoan),
                Convert.ToInt32(Globals.FinancialServiceTypes.VariableLoan)
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

        /// <summary>
        /// Returns a collection of unsecured mortgageloans.
        /// </summary>
        /// <param name="account">The account to filter by.</param>
        /// <returns>A collection of IMortgageLoan.</returns>
        public static IReadOnlyEventList<IMortgageLoan> GetUnsecuredMortgageLoans(IAccount account)
        {
            List<IMortgageLoan> list = new List<IMortgageLoan>();

            string HQL = String.Format("from MortgageLoan_DAO ml where ml.Account.Key = {0} and ml.MortgageLoanPurpose.MortgageLoanPurposeGroup.Key = 2 ", account.Key);
            SimpleQuery<MortgageLoan_DAO> q = new SimpleQuery<MortgageLoan_DAO>(HQL);
            MortgageLoan_DAO[] Mls = q.Execute();

            for (int i = 0; i < Mls.Length; i++)
            {
                list.Add(new MortgageLoan(Mls[i]));
            }

            return new ReadOnlyEventList<IMortgageLoan>(list);
        }

        /// <summary>
        /// Gets the child HOC account for the MortgageLoan account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>an IHOCAccount or null if not found</returns>
        public static IAccountHOC GetHOCAccount(IAccount account)
        {
            for (int i = 0; i < account.RelatedChildAccounts.Count; i++)
            {
                if (account.RelatedChildAccounts[i] is IAccountHOC)
                    return account.RelatedChildAccounts[i] as IAccountHOC;
            }

            return null;
        }

        /// <summary>
        /// Gets the latest child LifePolicy account for the MortgageLoan account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>an IAccountLifePolicy or null if not found</returns>
        public static IAccountLifePolicy GetLifeAccount(IAccount account)
        {
            IAccountLifePolicy accountLifePolicy = null;
            for (int i = 0; i < account.RelatedChildAccounts.Count; i++)
            {
                IAccountLifePolicy accLifePolicy = account.RelatedChildAccounts[i] as IAccountLifePolicy;
                if (accLifePolicy == null)
                    continue;

                if (accountLifePolicy == null)
                    accountLifePolicy = accLifePolicy;
                else
                {
                    if (accountLifePolicy.Key < accLifePolicy.Key)
                        accountLifePolicy = accLifePolicy;
                }
            }

            return accountLifePolicy;
        }

        /// <summary>
        /// Returns a list of Legalentities that have a role of main applicant defined on this account.
        /// </summary>
        /// <param name="account">The account to filter by.</param>
        /// <returns>A list of the legalentities that are main applicants.</returns>
        public static IReadOnlyEventList<ILegalEntity> GetMainApplicants(IAccount account)
        {
            List<ILegalEntity> list = new List<ILegalEntity>();

            string HQL = String.Format("select l from LegalEntity_DAO l inner join l.Roles r where r.Account.Key = {0} and r.RoleType.Key = 2", account.Key);
            SimpleQuery<LegalEntity_DAO> q = new SimpleQuery<LegalEntity_DAO>(HQL);
            LegalEntity_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(results[i]));
            }

            return new ReadOnlyEventList<ILegalEntity>(list);
        }

        public static IReadOnlyEventList<ILegalEntity> GetActiveMainApplicants(IAccount account)
        {
            List<ILegalEntity> list = new List<ILegalEntity>();

            string HQL = String.Format("select l from LegalEntity_DAO l inner join l.Roles r where r.Account.Key = {0} and r.RoleType.Key = 2 and r.GeneralStatus.Key = 1", account.Key);
            SimpleQuery<LegalEntity_DAO> q = new SimpleQuery<LegalEntity_DAO>(HQL);
            LegalEntity_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(results[i]));
            }

            return new ReadOnlyEventList<ILegalEntity>(list);
        }

        /// <summary>
        /// Returns a list of Legalentities that have a role of suretor defined on this account.
        /// </summary>
        /// <param name="account">The account to filter by.</param>
        /// <returns>A list of legale</returns>
        public static IReadOnlyEventList<ILegalEntity> GetSuretors(IAccount account)
        {
            List<ILegalEntity> list = new List<ILegalEntity>();

            string HQL = String.Format("select l from LegalEntity_DAO l inner join l.Roles r where r.Account.Key = {0} and r.RoleType.Key = 3", account.Key);
            SimpleQuery<LegalEntity_DAO> q = new SimpleQuery<LegalEntity_DAO>(HQL);
            LegalEntity_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(results[i]));
            }

            return new ReadOnlyEventList<ILegalEntity>(list);
        }

        /// <summary>
        /// The Capitalised Life of the MortgageLoanAccount
        /// (No calcs or interest should be calculated on capitalised life)
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public static double CapitalisedLife(IAccount Account)
        {
            IReadOnlyEventList<IFinancialService> _lFS;
            IAccount _lAccount = null;

            //Get Capitalised Life
            foreach (IAccount acc in Account.RelatedChildAccounts)
            {
                if (acc.AccountType == SAHL.Common.Globals.AccountTypes.Life && (acc.AccountStatus.Key == (int)AccountStatuses.Open || acc.AccountStatus.Key == (int)AccountStatuses.Dormant))
                {
                    _lAccount = acc;
                    break;
                }
            }

            //double _CapitalisedLife = 0;
            if ((_lAccount as IAccountLifePolicy) != null)
            {
                if (_lAccount.AccountStatus.Key == (Int32)SAHL.Common.Globals.AccountStatuses.Open || _lAccount.AccountStatus.Key == (Int32)SAHL.Common.Globals.AccountStatuses.Dormant)
                {
                    // TODO: CRAIGF please check that the account status types are correct below.
                    // Get the Life Policy Object
                    _lFS = _lAccount.GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes.LifePolicy, new SAHL.Common.Globals.AccountStatuses[] { SAHL.Common.Globals.AccountStatuses.Open, SAHL.Common.Globals.AccountStatuses.Dormant });
                    foreach (IFinancialService fS in _lFS)
                    {
                        ILifePolicy lP = fS.LifePolicy;
                        if (lP != null)
                        {
                            if (lP.LifePolicyStatus.Key == Convert.ToInt32(SAHL.Common.Globals.LifePolicyStatuses.Inforce))
                            {
                                return lP.FinancialService.Balance.Amount + lP.FinancialService.Balance.LoanBalance.InitialBalance;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// The Current Balance of the Mortgage Loan
        /// This is the sum of the current balances on the variable and the fixed (if exists) legs
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public static double LoanCurrentBalance(IAccount Account)
        {
            IMortgageLoanAccount mortgageLoanAccount = Account as IMortgageLoanAccount;

            return mortgageLoanAccount.SecuredMortgageLoan.CurrentBalance;
        }

        /// <summary>
        /// Get the Balance on account based on a date
        /// </summary>
        /// <param name="account"></param>
        /// <param name="dte"></param>
        /// <returns></returns>
        public static double GetAccountBalanceByDate(IAccount account, DateTime dte)
        {
            string query = UIStatementRepository.GetStatement("Account", "GetAccountBalanceByDate");

            // Create a collection and add the required parameters
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", account.Key));
            prms.Add(new SqlParameter("@Date", dte.Date));

            // execute
            object returnVal = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(Account_DAO), prms);

            // Get the Return Values
            return Convert.ToDouble(returnVal);
        }
    }
}