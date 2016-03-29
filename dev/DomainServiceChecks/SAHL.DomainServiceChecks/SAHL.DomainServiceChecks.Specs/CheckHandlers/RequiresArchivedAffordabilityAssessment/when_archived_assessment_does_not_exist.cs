using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AffordabilityAssessmentDataManager;
using System;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresOpenApplication
{
    public class when_archived_assessment_does_not_exist : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static ISystemMessageCollection systemMessages;
        private static RequiresArchivedAffordabilityAssessmentHandler handler;
        private static IRequiresArchivedAffordabilityAssessment command;
        private static AffordabilityAssessmentDataModel affordabilityAssessmentDataModel;

        private Establish context = () =>
        {
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            command = An<IRequiresArchivedAffordabilityAssessment>();
            command.WhenToldTo(x => x.AffordabilityAssessmentKey).Return(0);
            handler = new RequiresArchivedAffordabilityAssessmentHandler(affordabilityAssessmentDataManager);
            affordabilityAssessmentDataModel = new AffordabilityAssessmentDataModel(0, 0, 0, (int)AffordabilityAssessmentStatus.Confirmed, (int)GeneralStatus.Active, 0, DateTime.Now, 0, 0, 0, null, null, null);
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentByKey(Param.IsAny<int>())).Return(affordabilityAssessmentDataModel);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(command);
        };

        private It should_get_the_affordability_assessment = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.GetAffordabilityAssessmentByKey(0));
        };

        private It should_return_error_messages = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("Affordability assessment status should be archived to perform this operation.");
        };
    }
}