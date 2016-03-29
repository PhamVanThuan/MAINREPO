using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresUnconfirmedAffordabilityAssessmentHandler : IDomainCommandCheckHandler<IRequiresUnconfirmedAffordabilityAssessment>
    {
        private IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        public RequiresUnconfirmedAffordabilityAssessmentHandler(IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager)
        {
            this.affordabilityAssessmentDataManager = affordabilityAssessmentDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresUnconfirmedAffordabilityAssessment command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(command.AffordabilityAssessmentKey);
            if (affordabilityAssessmentDataModel.AffordabilityAssessmentStatusKey != (int)AffordabilityAssessmentStatus.Unconfirmed)
            {
                systemMessages.AddMessage(new SystemMessage("Affordability assessment status should be unconfirmed to perform this operation.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}