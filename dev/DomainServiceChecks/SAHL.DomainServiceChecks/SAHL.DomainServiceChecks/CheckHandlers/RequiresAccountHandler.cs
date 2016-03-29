using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresAccountHandler : IDomainCommandCheckHandler<IRequiresAccount>
    {
        private IAccountDataManager accountDataManager;

        public RequiresAccountHandler(IAccountDataManager accountDataManager)
        {
            this.accountDataManager = accountDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresAccount command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            bool doesAccountExist = accountDataManager.DoesAccountExist(command.AccountKey);
            if (!doesAccountExist)
            {
                systemMessages.AddMessage(new SystemMessage("There is no account for the provided account key.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}