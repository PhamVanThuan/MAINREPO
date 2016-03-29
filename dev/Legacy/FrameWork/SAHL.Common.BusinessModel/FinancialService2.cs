using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public partial class FinancialService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialService_DAO>, IFinancialService
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("FinancialServiceBankAccount");
        }

        /// <summary>
        /// Gets the currently active <see cref="IFinancialServiceBankAccount"/> associated with the financial service.
        /// </summary>
        public IFinancialServiceBankAccount CurrentBankAccount
        {
            get
            {
                foreach (IFinancialServiceBankAccount fsba in this.FinancialServiceBankAccounts)
                {
                    if (fsba.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        return fsba;
                }

                return null;
            }
        }

        /// <summary>
        /// Fills a DataTable with Transactions against this FinancialService
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="TransactionTypeKeys">a list of the transaction types to consider</param>
        /// <returns>a populated datatable</returns>
        public DataTable GetTransactions(IDomainMessageCollection Messages, List<int> TransactionTypeKeys)
        {
            return GetTransactions(Messages, (int)GeneralStatuses.Active, TransactionTypeKeys);
        }

        /// <summary>
        /// Fills a DataTable with Transactions against this FinancialService
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="GeneralStatusKey"></param>
        /// <param name="TransactionTypeKeys">a list of the transaction types to consider</param>
        /// <returns>a populated datatable</returns>
        public DataTable GetTransactions(IDomainMessageCollection Messages, int GeneralStatusKey, List<int> TransactionTypeKeys)
        {
            if (TransactionTypeKeys == null || TransactionTypeKeys.Count == 0)
                throw new Exception("At least one TransactionTypeKey must be passed in.");

            string keys = "";

            for (int i = 0; i < TransactionTypeKeys.Count; i++)
            {
                keys += "," + TransactionTypeKeys[i].ToString();
            }

            keys = keys.Remove(0, 1);

            string query = UIStatementRepository.GetStatement("COMMON", "FinancialServiceTransactionsGet");
            string statement = String.Format(query, this.Key, keys);

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(statement, typeof(GeneralStatus_DAO), null);

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        public ILoanBalance LoanBalance
        {
            get
            {
                return this.Balance.LoanBalance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IFinancialServiceType FinancialServiceType
        {
            get
            {
                if (null == _DAO.FinancialServiceType) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialServiceType, FinancialServiceType_DAO>(_DAO.FinancialServiceType);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fats"></param>
        /// <returns></returns>
        public IFinancialAdjustment GetPendingFinancialAdjustmentByTypeSource(FinancialAdjustmentTypeSources fats)
        {
            foreach (FinancialAdjustment fa in this.FinancialAdjustments)
            {
                if (fa.FinancialAdjustmentTypeSource.Key == (int)fats
                    && fa.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Inactive
                    && (fa.FromDate.HasValue && fa.FromDate.Value.Date >= DateTime.Now.Date)
                    && fa.CancellationDate == null)
                {
                    return fa;
                }
            }
            return null;
        }

        public void OnFinancialServiceBankAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFinancialServiceBankAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFinancialServiceConditions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialServiceConditions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialServiceRecurringTransactions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialServiceRecurringTransactions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLifePolicies_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLifePolicies_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnMortgageLoans_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnMortgageLoans_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialAdjustment_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            IRuleService rs = ServiceFactory.GetService<IRuleService>();
            rs.ExecuteRule(spc.DomainMessages, "FinancialAdjustmentCollectNoPaymentAdd", new object[] { Item });
        }

        virtual protected void OnFinancialServiceBankAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        virtual protected void OnFinancialServiceBankAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialServiceConditions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialServiceConditions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialTransactions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialTransactions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialTransactions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnFinancialTransactions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCs_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnHOCs_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLifePolicies_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLifePolicies_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnMortgageLoans_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnMortgageLoans_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLoanTransactions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLoanTransactions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLoanTransactions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnLoanTransactions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// Gets all LifePolicyClaim objects for the given FinancialService
        /// </summary>
        /// <returns></returns>
        public IList<ILifePolicyClaim> GetLifePolicyClaims()
        {
            List<ILifePolicyClaim> list = new List<ILifePolicyClaim>();

            string hql = "SELECT lpc from LifePolicyClaim_DAO lpc WHERE lpc.FinancialService.Key = ?";
            SimpleQuery<LifePolicyClaim_DAO> q = new SimpleQuery<LifePolicyClaim_DAO>(hql, Key);

            LifePolicyClaim_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<ILifePolicyClaim, LifePolicyClaim_DAO>(results[i]));
            }

            list.Sort(delegate(ILifePolicyClaim lpc1, ILifePolicyClaim lpc2) { return lpc2.ClaimDate.CompareTo(lpc1.ClaimDate); });
            return list;
        }

        /// <summary>
        /// Gets the pending LifePolicyClaim object for the given FinancialService
        /// </summary>
        /// <returns></returns>
        public ILifePolicyClaim GetLifePolicyClaimPending()
        {
            List<ILifePolicyClaim> list = new List<ILifePolicyClaim>();

            string hql = "SELECT lpc from LifePolicyClaim_DAO lpc WHERE lpc.FinancialService.Key = ? AND lpc.ClaimStatus.Key = ?";
            SimpleQuery<LifePolicyClaim_DAO> q = new SimpleQuery<LifePolicyClaim_DAO>(hql, Key, (int)ClaimStatuses.Pending);
            q.SetQueryRange(1);

            LifePolicyClaim_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            if (results.Length > 0)
                return BMTM.GetMappedType<ILifePolicyClaim, LifePolicyClaim_DAO>(results[0]);
            else
                return null;
        }
    }
}