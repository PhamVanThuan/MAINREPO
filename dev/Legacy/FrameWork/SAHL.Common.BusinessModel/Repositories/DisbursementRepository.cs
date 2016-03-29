using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IDisbursementRepository))]
    public class DisbursementRepository : AbstractRepositoryBase, IDisbursementRepository
    {
        public DisbursementRepository()
        {
            if (this.castleTransactionService == null)
            {
                this.castleTransactionService = new CastleTransactionsService();
            }
        }

        public DisbursementRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;
        private IRuleService _ruleServ;

        /// <summary>
        /// Set the Margin and/or BaseRate on all MortgageLoans for the Account.
        /// Category will be assessed and updated if required.
        /// This method saves objects internally, so no need to save again on completion.
        /// NB! This method must be called within a Transaction.
        /// </summary>
        /// <param name="account">IAccount to update</param>
        /// <param name="margin">IMargin to save against the Account</param>
        /// <param name="userid">User making the change</param>
        /// <param name="baseRateReset">For CAP, updating the Base Rate</param>
        public void UpdateRate(IAccount account, IMargin margin, string userid, bool baseRateReset)
        {
            #region Repo's

            ICreditMatrixRepository crRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
            ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection messages = spc.DomainMessages;

            #endregion Repo's

            IMortgageLoanAccount mla = account as IMortgageLoanAccount;

            //Get the variable ML
            IMortgageLoan vml = mla.SecuredMortgageLoan;
            IMortgageLoan fml = null;

            #region Variable rate update

            if (vml != null)
            {
                IRateConfiguration rcVar = crRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(margin.Key, vml.RateConfiguration.MarketRate.Key);
                pLoanUpdateRate(vml.Key, rcVar.Key, userid);
            }

            #endregion Variable rate update

            #region Fixed rate update

            double fixBalance = 0;
            double fixInstal = 0;
            IAccountVariFixLoan faccount = account as IAccountVariFixLoan;
            if (faccount != null)
            {
                fml = faccount.FixedSecuredMortgageLoan;

                //Fix current balance
                fixBalance = fml.CurrentBalance;

                IRateConfiguration rcFix = crRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(margin.Key, fml.RateConfiguration.MarketRate.Key);
                pLoanUpdateRate(fml.Key, rcFix.Key, userid);

                //Get the new fix instalment
                fixInstal = fml.Payment;
            }

            #endregion Fixed rate update

            /*

            #region Update the category

            #region Get values to determine CM

            int employmentTypeKey = account.GetEmploymentTypeKey();

            //default to Salaried if Unemployed or Unknown
            if (employmentTypeKey > (int)EmploymentTypes.Unemployed)
                employmentTypeKey = (int)EmploymentTypes.Salaried;

            //Get the property value
            double valuationAmount = vml.GetActiveValuationAmount();

            //Get loan balance
            double loanBalance = vml.CurrentBalance + fixBalance;

            //calc LTV
            double ltv = 0;
            if (valuationAmount > 0)
                ltv = (loanBalance) / valuationAmount;

            //calc PTI - always on amortising
            double pti = 0;
            if (account.GetHouseholdIncome() > 0)
                pti = (vml.Payment + fixInstal) / account.GetHouseholdIncome();

            #endregion Get values to determine CM

            //GetCMByOSP & LTV
            //use LTV and PTI to get best CC and use Category
            IReadOnlyEventList<ICreditCriteria> ccList = ccRepo.GetCreditCriteriaByLTV(messages, account.OriginationSource.Key, account.Product.Key, vml.MortgageLoanPurpose.Key, employmentTypeKey, loanBalance, ltv);
            ICategory category = null;
            foreach (ICreditCriteria cc in ccList)
            {
                if (pti < (cc.PTI / 100))
                {
                    category = cc.Category;
                    break;
                }
            }

            #endregion Update the category

             * */
        }

        /// <summary>
        /// Domain implementation of stored procedure 2AM.pLoanUpdateRate
        /// NB: This method MUST be called within a transaction scope
        /// </summary>
        private void pLoanUpdateRate(int financialServiceKey, int rateConfigurationKey, string userID)
        {
            // Create a collection
            ParameterCollection prms = new ParameterCollection();

            //Add the required parameters
            prms.Add(new SqlParameter("@FinancialServiceKey", financialServiceKey));
            prms.Add(new SqlParameter("@RateConfigurationKey", rateConfigurationKey));
            prms.Add(new SqlParameter("@UserId", userID));

            // execute
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateInterestRate", prms);
        }

        public IDisbursement GetDisbursementByKey(int Key)
        {
            return base.GetByKey<IDisbursement, Disbursement_DAO>(Key);
        }

        public void DeleteDisbursement(IDisbursement IObj)
        {
            Disbursement_DAO dao = (Disbursement_DAO)(IObj as IDAOObject).GetDAOObject();

            dao.DeleteAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        private void DeleteDisbursement(int Key)
        {
            IDisbursement D = new Disbursement(Disbursement_DAO.Find(Key));
            DeleteDisbursement(D);
        }

        public IReadOnlyEventList<IDisbursementTransactionType> GetDisbursementTransactionTypes(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            string groups = spc.GetCachedRolesAsStringForQuery(true, false);

            string sql = @"select distinct
	                            dtt.* 
                            from 
	                            [2am]..DisbursementTransactionType dtt (nolock)
                            join 
	                            [2am].fin.TransactionType tt (nolock) on tt.TransactionTypeKey = dtt.TransactionTypeNumber
                            join 
	                            [2am]..TransactionTypeDataAccess tta (nolock) on tta.TransactionTypeKey = dtt.TransactionTypeNumber
                            where
	                            tta.ADCredentials in (" + groups + ")";

            SimpleQuery<DisbursementTransactionType_DAO> q = new SimpleQuery<DisbursementTransactionType_DAO>(QueryLanguage.Sql,sql);
            q.AddSqlReturnDefinition(typeof(DisbursementTransactionType_DAO), "disb");
            DisbursementTransactionType_DAO[] res = q.Execute();
            List<DisbursementTransactionType_DAO> list = new List<DisbursementTransactionType_DAO>(res);
            IEventList<IDisbursementTransactionType> evList = new DAOEventList<DisbursementTransactionType_DAO, IDisbursementTransactionType, DisbursementTransactionType>(list);
            return new ReadOnlyEventList<IDisbursementTransactionType>(evList);
        }

        /// <summary>
        /// This method will get the Disbursement(s), if any, linked to a particular FinancialTransaction.
        /// Disbursements are related to Loan Transactions through the DisbursementLoanTransaction table,
        /// modelled in the domain by a LoanTransactions collection on the Disbursement object.
        /// This is useful when one needs to rollback a loan transaction, because one needs to set the status of the linked disbursement(s) to 'Rolled Back'.
        /// </summary>
        /// <param name="LoanTransactionNumber"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IDisbursement> GetDisbursementByLoanTransactionNumber(decimal LoanTransactionNumber)
        {
            string sql = @"Select disb.*
                from dbo.Disbursement disb
                inner join dbo.DisbursementFinancialTransaction disbFinTran
                on disb.DisbursementKey = disbFinTran.DisbursementKey
                inner join fin.FinancialTransaction finTran
                on finTran.FinancialTransactionKey = disbFinTran.FinancialTransactionKey
                WHERE disbFinTran.FinancialTransactionKey = ?";

            SimpleQuery<Disbursement_DAO> q = new SimpleQuery<Disbursement_DAO>(QueryLanguage.Sql, sql, LoanTransactionNumber);
            q.AddSqlReturnDefinition(typeof(Disbursement_DAO), "disb");
            Disbursement_DAO[] res = q.Execute();
            List<Disbursement_DAO> list = new List<Disbursement_DAO>(res);
            IEventList<IDisbursement> evList = new DAOEventList<Disbursement_DAO, IDisbursement, Disbursement>(list);
            return new ReadOnlyEventList<IDisbursement>(evList);
        }

        public IReadOnlyEventList<IDisbursement> GetDisbursmentsByParentAccountKeyAndStatus(int ParentAccountKey, int DisbursementStatusKey)
        {
            string HQL = "from Disbursement_DAO d where d.Account.Key = ? and d.DisbursementStatus.Key = ? order by d.ActionDate desc";
            SimpleQuery<Disbursement_DAO> q = new SimpleQuery<Disbursement_DAO>(HQL, ParentAccountKey, DisbursementStatusKey);
            q.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            Disbursement_DAO[] res1 = q.Execute();

            HQL = "from Disbursement_DAO d join d.Account.RelatedChildAccounts ra where d.Account.Key = ra.Key and d.Account.Key = ? and d.DisbursementStatus.Key = ? order by d.ActionDate desc";
            q = new SimpleQuery<Disbursement_DAO>(HQL, ParentAccountKey, DisbursementStatusKey);
            q.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            Disbursement_DAO[] res2 = q.Execute();

            List<Disbursement_DAO> list = new List<Disbursement_DAO>(res1);

            for (int i = 0; i < res2.Length; i++)
            {
                if (!list.Contains(res2[i]))
                {
                    if (res2[i].ActionDate.HasValue)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (list[j].ActionDate < res2[i].ActionDate)
                                list.Insert(j, res2[i]);
                        }
                    }
                    else
                        list.Add(res2[i]);
                }
            }

            IEventList<IDisbursement> evList = new DAOEventList<Disbursement_DAO, IDisbursement, Disbursement>(list);
            return new ReadOnlyEventList<IDisbursement>(evList);
        }

        public DataTable GetDisbursementLoanTransactions(int AccountKey, SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            string roles = spc.GetCachedRolesAsStringForQuery(false, false);

            string sql = UIStatementRepository.GetStatement("COMMON", "GetDisbursementLoanTransactions");

            string query = String.Format(sql, roles);

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", AccountKey));
            prms.Add(new SqlParameter("@ADUserName", principal.Identity.Name));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        public DataTable GetDisbursementRollbackTransactions(int AccountKey, int[] LoanTransactionNumbers)
        {
            // Refactor FXCop Error Message - DoNotConcatenateStringsInsideLoops
            //string numbers = "";
            StringBuilder numbers = new StringBuilder();

            for (int i = 0; i < LoanTransactionNumbers.Length; i++)
            {
                //numbers = numbers + "," + LoanTransactionNumbers[i].ToString();
                numbers.AppendFormat(",{0}", LoanTransactionNumbers[i].ToString());
            }

            if (numbers.Length > 0)
                numbers = numbers.Remove(0, 1);

            string sql = UIStatementRepository.GetStatement("COMMON", "GetDisbursementRollbackTransactions");
            string query = String.Format(sql, numbers.ToString());
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", AccountKey));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        public IDisbursement CreateEmptyDisbursement()
        {
            return base.CreateEmpty<IDisbursement, Disbursement_DAO>();

            //return new Disbursement(new Disbursement_DAO());
        }

        private void SaveDisbursement(IDisbursement disbursement)
        {
            base.Save<IDisbursement, Disbursement_DAO>(disbursement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementList"></param>
        /// <param name="totalAmount"></param>
        public void SaveDisbursement(IReadOnlyEventList<IDisbursement> disbursementList, double totalAmount)
        {
            if (disbursementList.Count < 1)
            {
                throw new Exception("The list of disbursements to save has a count of zero.");
            }

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            ValidateDisbursement(spc.DomainMessages, disbursementList, totalAmount);

            if (spc.DomainMessages.Count > 0)
                throw new Exception();

            IReadOnlyEventList<IDisbursement> savedDisbursements = GetDisbursmentsByParentAccountKeyAndStatus(disbursementList[0].Account.Key, Convert.ToInt32(DisbursementStatuses.Pending));
            for (int savedIndex = 0; savedIndex < savedDisbursements.Count; savedIndex++)
            {
                bool deleted = true;
                for (int i = 0; i < disbursementList.Count; i++)
                {
                    if (disbursementList[i].Key == savedDisbursements[savedIndex].Key)
                        deleted = false;
                }

                if (deleted)
                    DeleteDisbursement(savedDisbursements[savedIndex].Key);
            }

            for (int i = 0; i < disbursementList.Count; i++)
            {
                //if (disbursementList[i].Key == 0)
                //{
                if (spc.DomainMessages.Count == 0)
                    SaveDisbursement(disbursementList[i]);

                //}
            }
        }

        private void ValidateDisbursement(IDomainMessageCollection messages, IReadOnlyEventList<IDisbursement> disbursementList, double totalAmount)
        {
            double sumOfIndividualAmounts = 0;

            //int disbTranTypeNum = 0;

            for (int i = 0; i < disbursementList.Count; i++)
            {
                //disbTranTypeNum = disbursementList[i].DisbursementTransactionType.TransactionTypeNumber.HasValue ? disbursementList[i].DisbursementTransactionType.TransactionTypeNumber.Value : 0;
                sumOfIndividualAmounts += Math.Round(Convert.ToDouble(disbursementList[i].Amount), 2);
            }

            IMortgageLoanAccount mlAcct = disbursementList[0].Account as IMortgageLoanAccount;
            if (disbursementList[0].DisbursementTransactionType.Key != (int)DisbursementTransactionTypes.CancellationRefund && disbursementList[0].DisbursementTransactionType.Key != (int)DisbursementTransactionTypes.Refund)
                RuleServ.ExecuteRule(messages, "CATSDisbursementLoanAgreementAmountValidate", disbursementList, mlAcct);

            RuleServ.ExecuteRule(messages, "CATSDisbursementTransactionAmountValidate", Math.Round(sumOfIndividualAmounts, 2), Math.Round(totalAmount, 2));

            if (disbursementList[0].DisbursementTransactionType.Key == (int)DisbursementTransactionTypes.ReAdvance && disbursementList[0].DisbursementStatus.Key != (int)DisbursementStatuses.RolledBack)
            {
                RuleServ.ExecuteRule(messages, "CATSDisbursementReAdvanceNotDisbursed", disbursementList[0].Account, sumOfIndividualAmounts);
                RuleServ.ExecuteRule(messages, "CATSDisbursementReAdvanceNotDisbursedExt", disbursementList[0].Account, sumOfIndividualAmounts);
            }

            List<string> rulesToRun = new List<string>();
            rulesToRun.Add("CATSDisbursementValidateUpdateRecord");
            rulesToRun.Add("CATSDisbursementValidateAmount");
            rulesToRun.Add("CATSDisbursementValidateTypeCancRefundCurrentBalance");
            rulesToRun.Add("CATSDisbursementValidateReadvanceDebtCounselling");
            rulesToRun.Add("CATSDisbursementValidateTypeCAP2AddRecord");
            rulesToRun.Add("CATSDisbursementQuickCashDisbursementValidate");
            rulesToRun.Add("CATSDisbursementSPVCheck");

            //rulesToRun.Add("CATSDisbursementReAdvanceNotDisbursed"); needs application
            RuleServ.ExecuteRuleSet(messages, rulesToRun, new object[] { disbursementList });
        }

        private IRuleService RuleServ
        {
            get
            {
                if (_ruleServ == null)
                    _ruleServ = ServiceFactory.GetService<IRuleService>();

                return _ruleServ;
            }
        }

        /// <summary>
        /// Added as part of revamp. New API for posting the fin tran and linking the disbursement ref: #20307
        /// </summary>
        /// <param name="disbursementKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="reference"></param>
        /// <param name="userID"></param>
        public void PostDisbursementTransaction(int disbursementKey, DateTime effectiveDate, string reference, string userID)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@DisbursementKey", disbursementKey));
            prms.Add(new SqlParameter("@EffectiveDate", effectiveDate));
            prms.Add(new SqlParameter("@Reference", reference));
            prms.Add(new SqlParameter("@UserId", userID));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "DisbursementTransaction", prms);
        }

        /// <summary>
        /// Trac Ticket 20582
        /// </summary>
        /// <param name="accountKey"></param>
        public void ReturnDisbursedLoanToRegistration(int accountKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "ReturnDisbursedLoanToRegistration", prms);
        }

        public void DisburseFundsForUnsecuredLendingApplication(int applicationKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OfferKey", applicationKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DisbursementRepository", "DisburseFundsForUnsecuredLendingApplication", prms);
        }
    }
}