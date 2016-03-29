using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.Services.ApplicationDomain.CommandHandlers.Internal
{
    public class UpdateAffordabilityAssessmentIncomeContributorsCommandHandler : IServiceCommandHandler<UpdateAffordabilityAssessmentIncomeContributorsCommand>
    {
        private IADUserManager adUserManager;
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;

        public UpdateAffordabilityAssessmentIncomeContributorsCommandHandler(IADUserManager adUserManager, IAffordabilityAssessmentManager affordabilityAssessmentManager)
        {
            this.adUserManager = adUserManager;
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateAffordabilityAssessmentIncomeContributorsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            int? _ADUserKey = adUserManager.GetAdUserKeyByUserName(metadata.UserName);
            if (_ADUserKey == null || _ADUserKey.Value <= 0)
            {
                systemMessageCollection.AddMessage(new SystemMessage("Failed to retrieve ADUserKey.", SystemMessageSeverityEnum.Error));
                return systemMessageCollection;
            }

            affordabilityAssessmentManager.UpdateAffordabilityAssessmentAndIncomeContributors(command.AffordabilityAssessment, _ADUserKey.Value);

            return systemMessageCollection;
        }
    }
}