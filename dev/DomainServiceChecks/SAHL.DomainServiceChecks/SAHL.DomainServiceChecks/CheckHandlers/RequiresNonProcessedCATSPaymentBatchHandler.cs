using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;
using SAHL.DomainServiceChecks.Managers.CatsDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresProcessedCATSPaymentBatchHandler : IDomainCommandCheckHandler<IRequiresCATSPaymentBatch>
    {
        private ICatsDataManager catsDataManager;

        public RequiresProcessedCATSPaymentBatchHandler(ICatsDataManager catsDataManager)
        {
            this.catsDataManager = catsDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresCATSPaymentBatch command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            if (!catsDataManager.DoesCATSPaymentBatchExist(command.CATSPaymentBatchKey))
            {
                systemMessages.AddMessage(new SystemMessage(string.Format(@"Invalid batch number: {0}. The batch number provided does not exist."
                    , command.CATSPaymentBatchKey), SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}