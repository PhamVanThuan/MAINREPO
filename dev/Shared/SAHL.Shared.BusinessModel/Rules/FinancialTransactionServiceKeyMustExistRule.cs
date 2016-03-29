using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Rules
{
    public class FinancialTransactionServiceKeyMustExistRule : IDomainRule<TransactionRuleModel>
    {
        private ITransactionDataManager transactionDataManager;

        public FinancialTransactionServiceKeyMustExistRule(ITransactionDataManager transactionDataManager)
        {
            this.transactionDataManager = transactionDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, TransactionRuleModel ruleModel)
        {
            if (!transactionDataManager.DoesFinancialTransactionKeyExist(ruleModel.TransactionKey))
            {
                messages.AddMessage(new SystemMessage("The financial service key provided does not exist.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
