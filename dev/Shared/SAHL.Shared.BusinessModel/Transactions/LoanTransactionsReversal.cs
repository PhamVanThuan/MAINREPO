using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;

namespace SAHL.Shared.BusinessModel.Transactions
{
    public class LoanTransactionsReversal : ILoanTransactionsReversal
    {
        private IDomainRuleManager<TransactionRuleModel> domainRuleManager;
        private ITransactionDataManager transactionDataManager;

        public LoanTransactionsReversal(ITransactionDataManager transactionDataManager, IDomainRuleManager<TransactionRuleModel> domainRuleManager)
        {
            this.transactionDataManager = transactionDataManager;
            this.domainRuleManager = domainRuleManager;

            domainRuleManager.RegisterRule(new FinancialTransactionServiceKeyMustExistRule(transactionDataManager));
        }

        public ISystemMessageCollection postTransaction(ReversalTransactionModel transactionModel)
        {
            var messages = SystemMessageCollection.Empty();
            var ruleModel = new TransactionRuleModel { TransactionKey = transactionModel.FinancialTransactionKey };
            domainRuleManager.ExecuteRules(messages, ruleModel);
            if (!messages.HasErrors)
            {
                transactionDataManager.PostReversalTransaction(transactionModel);
            }

            return messages;
        }
    }
}
