using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;

namespace SAHL.Shared.BusinessModel.Transactions
{
    public class LoanTransactions : ILoanTransactions
    {
        private ITransactionDataManager transactionDataManager;
        private IDomainRuleManager<TransactionRuleModel> domainRuleManager;

        public LoanTransactions(ITransactionDataManager transactionDataManager, IDomainRuleManager<TransactionRuleModel> domainRuleManager)
        {
            this.transactionDataManager = transactionDataManager;
            this.domainRuleManager = domainRuleManager;
        }

        public ISystemMessageCollection PostTransaction(PostTransactionModel postTransactionModel)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new EffectiveDateCannotBeInTheFutureRule());
            domainRuleManager.RegisterRule(new EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule());
            domainRuleManager.RegisterRule(new TransactionTypeMustBeValidRule(transactionDataManager));

            var ruleModel = new TransactionRuleModel { EffectiveDate = postTransactionModel.EffectiveDate, TransactionTypeKey = postTransactionModel.TransactionTypeKey };
            domainRuleManager.ExecuteRules(messages, ruleModel);

            if (!messages.HasErrors)
            {
                var message = transactionDataManager.PostTransaction(postTransactionModel);
                if (!string.IsNullOrEmpty(message))
                {
                    messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
                }
            }

            return messages;
        }

        public ISystemMessageCollection postTransactionReversal(ReversalTransactionModel reversalTransactionModel)
        {
            var messages = SystemMessageCollection.Empty();
            var ruleModel = new TransactionRuleModel { TransactionKey = reversalTransactionModel.FinancialTransactionKey };

            domainRuleManager.RegisterRule(new FinancialTransactionServiceKeyMustExistRule(transactionDataManager));
            domainRuleManager.ExecuteRules(messages, ruleModel);
            if (!messages.HasErrors)
            {
                transactionDataManager.PostReversalTransaction(reversalTransactionModel);
            }

            return messages;
        }
    }
}