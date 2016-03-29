using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Rules
{
    public class TransactionTypeMustBeValidRule : IDomainRule<TransactionRuleModel>
    {
        private ITransactionDataManager transactionDataManager;

        public TransactionTypeMustBeValidRule(ITransactionDataManager transactionDataManager)
        {
            this.transactionDataManager = transactionDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, TransactionRuleModel ruleModel)
        {
            var doesTransactionTypeExist = transactionDataManager.DoesTransactionTypeExist(ruleModel.TransactionTypeKey);
            if (!doesTransactionTypeExist)
            {
                var message = "TransactionType provided is not valid";
                messages.AddMessage(new SystemMessage(message, SystemMessageSeverityEnum.Error));
            }
        }
    }
}