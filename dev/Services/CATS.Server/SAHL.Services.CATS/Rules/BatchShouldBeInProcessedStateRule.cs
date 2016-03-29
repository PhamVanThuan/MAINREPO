using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.CATS.Rules
{
    public class BatchShouldBeInProcessedStateRule : IDomainRule<CatsPaymentBatchRuleModel>
    {
        private ICATSDataManager catsDataManager;

        public BatchShouldBeInProcessedStateRule(ICATSDataManager catsDataManager)
        {
            this.catsDataManager = catsDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, CatsPaymentBatchRuleModel ruleModel)
        {
            var batchRecord = catsDataManager.GetBatchByKey(ruleModel.BatchKey);
            if (batchRecord.CATSPaymentBatchStatusKey != (int)CATSPaymentBatchStatus.Processed)
            {
                messages.AddMessage(new SystemMessage("The batch provided is not in processed state.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
