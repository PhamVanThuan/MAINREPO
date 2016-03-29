using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.LifeDataManager;
using System;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresPendingDisabilityClaimHandler : IDomainCommandCheckHandler<IRequiresPendingDisabilityClaim>
    {
        private ILifeDataManager lifeDataManager;

        public RequiresPendingDisabilityClaimHandler(ILifeDataManager lifeDataManager)
        {
            this.lifeDataManager = lifeDataManager;
        }
        public Core.SystemMessages.ISystemMessageCollection HandleCheckCommand(IRequiresPendingDisabilityClaim command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            var disabilityClaim = this.lifeDataManager.GetDisabilityClaimByKey(command.DisabilityClaimKey);
            if (disabilityClaim.DisabilityClaimStatusKey != (int)DisabilityClaimStatus.Pending)
            {
                systemMessages.AddMessage(new SystemMessage("Disability Claim status should be pending to perform this operation.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}