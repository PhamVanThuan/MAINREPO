using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresConfirmedAffordabilityAssessmentHandler : IDomainCommandCheckHandler<IRequiresConfirmedAffordabilityAssessment>
    {
        private IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        public RequiresConfirmedAffordabilityAssessmentHandler(IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager)
        {
            this.affordabilityAssessmentDataManager = affordabilityAssessmentDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresConfirmedAffordabilityAssessment command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(command.AffordabilityAssessmentKey);
            if (affordabilityAssessmentDataModel.AffordabilityAssessmentStatusKey != (int)AffordabilityAssessmentStatus.Confirmed)
            {
                systemMessages.AddMessage(new SystemMessage("Affordability assessment status should be confirmed to perform this operation.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}