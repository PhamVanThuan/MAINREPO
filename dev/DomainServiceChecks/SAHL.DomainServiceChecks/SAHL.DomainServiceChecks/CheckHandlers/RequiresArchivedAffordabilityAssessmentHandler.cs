using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresArchivedAffordabilityAssessmentHandler : IDomainCommandCheckHandler<IRequiresArchivedAffordabilityAssessment>
    {
        private IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;

        public RequiresArchivedAffordabilityAssessmentHandler(IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager)
        {
            this.affordabilityAssessmentDataManager = affordabilityAssessmentDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresArchivedAffordabilityAssessment command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            AffordabilityAssessmentDataModel affordabilityAssessmentDataModel = this.affordabilityAssessmentDataManager.GetAffordabilityAssessmentByKey(command.AffordabilityAssessmentKey);
            if (affordabilityAssessmentDataModel.GeneralStatusKey != (int)GeneralStatus.Inactive)
            {
                systemMessages.AddMessage(new SystemMessage("Affordability assessment status should be archived to perform this operation.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}