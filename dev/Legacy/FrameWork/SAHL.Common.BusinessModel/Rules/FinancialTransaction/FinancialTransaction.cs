using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.FinancialTransaction
{
    [RuleDBTag("PostTransactionNonPerformingLoan967",
    "The 967 transaction can only be posted against a loan that is currently marked as non-performing AND that has had a 966 transaction posted against it.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan967")]
    [RuleInfo]
    public class PostTransactionNonPerformingLoan967 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The PostTransactionNonPerformingLoan967 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The PostTransactionNonPerformingLoan967 rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IAccount account = Parameters[0] as IAccount;

            if (financialServiceRepo.IsLoanNonPerforming(account.Key))
            {
                var hasResults = account.FinancialServices.Any(fs =>
                                                               fs.AccountStatus.Key == (int)AccountStatuses.Open &&
                                                               fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan &&
                                                               fs.FinancialServices.Any(cfs =>
                                                                                            cfs.AccountStatus.Key == (int)AccountStatuses.Open &&
                                                                                            cfs.FinancialTransactions.Any(ft => ft.TransactionType.Key == (int)TransactionTypes.NonPerformingInterest)));

                if (hasResults)
                    return 1;

                string errMsg = "This transaction can only be posted to a loan that has one or more 966 transactions posted against it";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }
            else
            {
                string errMsg = "This transaction can only be posted to a loan that is currently marked as non-performing";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }
        }
    }

    [RuleDBTag("PostTransactionNonPerformingLoan236_966",
    "The 236 and 966 transactions can only be posted against a loan that is currently marked as non-performing. ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_966")]
    [RuleInfo]
    public class PostTransactionNonPerformingLoan236_966 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The PostTransactionNonPerformingLoan236_966 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The PostTransactionNonPerformingLoan236_966 rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IAccount account = Parameters[0] as IAccount;

            if (!financialServiceRepo.IsLoanNonPerforming(account.Key))
            {
                string errMsg = "This transaction can only be posted to a loan that is currently marked  as non-performing";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("PostTransactionNonPerformingLoan236_967",
    "The sum of all 966, 967 and 236 transactions should not be less than 0.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967")]
    [RuleInfo]
    public class PostTransactionNonPerformingLoan236_967 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The PostTransactionNonPerformingLoan236_967 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The PostTransactionNonPerformingLoan236_967 rule expects the following objects to be passed: IAccount.");
            if (!(Parameters[1] is double))
                throw new ArgumentException("The PostTransactionNonPerformingLoan236_967 rule expects the following objects to be passed: Loan Transaction Amount.");

            #endregion Check for allowed object type(s)

            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IAccount account = Parameters[0] as IAccount;
            double loanTransactionAmount = (double)Parameters[1];

            double total = account.FinancialServices.Where(x =>
                                x.FinancialServiceParent != null &&
                                x.FinancialServiceParent.AccountStatus.Key == (int)AccountStatuses.Open &&
                                x.AccountStatus.Key == (int)AccountStatuses.Open &&
                                x.FinancialServiceType.Key == (int)FinancialServiceTypes.SuspendedInterest).Sum(x => x.Balance.Amount);

            total -= loanTransactionAmount;

            var rounded = decimal.Round(Convert.ToDecimal(total), 2);

            if (rounded < 0)
            {
                string errMsg = "Unable to proceed – amount to be written off cannot exceed the current suspended interest amount";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("RollbackTransactionNonPerformingLoan236",
    "The 1236 rollback transaction can only be confirmed against a loan that is currently marked as non-performing.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.RollbackTransactionNonPerformingLoan236")]
    [RuleInfo]
    public class RollbackTransactionNonPerformingLoan236 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The RollbackTransactionNonPerformingLoan236 rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IAccount))
                throw new ArgumentException("The RollbackTransactionNonPerformingLoan236 rule expects the following objects to be passed: IAccount.");

            #endregion Check for allowed object type(s)

            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IAccount account = Parameters[0] as IAccount;

            if (!financialServiceRepo.IsLoanNonPerforming(account.Key))
            {
                string errMsg = "This transaction can only be posted to a loan that is currently marked as non-performing";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("PostTransactionCheckDateLessThanFirstOfCurrentMonth",
    "Cannot post a transaction that has an effective date prior to the 1st of the current month.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionCheckDateLessThanFirstOfCurrentMonth")]
    [RuleInfo]
    public class PostTransactionCheckDateLessThanFirstOfCurrentMonth : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            DateTime transactionEffectiveDate = new DateTime();
            if (Parameters[0] != null)
            {
                transactionEffectiveDate = Convert.ToDateTime(Parameters[0]);
            }

            DateTime validateDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (transactionEffectiveDate < validateDate)
            {
                string errMsg = "Cannot post a transaction with an effective date prior to the 1st of this month.";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("PostTransactionCheckEffectiveDate",
    "The effective date for payment txns must be after the first of last month. All other transactions must be effective this month.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionCheckEffectiveDate")]
    [RuleInfo]
    public class PostTransactionCheckEffectiveDate : BusinessRuleBase
    {
        public PostTransactionCheckEffectiveDate(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters[0] == null || Parameters[1] == null)
            {
                string errMsg = "Rule PostTransactionCheckEffectiveDate requires effective date and Transaction type.";
                AddMessage(errMsg, errMsg, Messages);
                return 0;
            }

            string query = UIStatementRepository.GetStatement("Rule", "PostTransactionCheckEffectiveDate");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@effectiveDate", Parameters[0]));
            prms.Add(new SqlParameter("@txnTypeKey", Parameters[1]));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO), prms);
            if (o != null)
            {
                AddMessage(o.ToString(), o.ToString(), Messages);
                return 1;
            }

            return 0;
        }
    }
}