using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Rules.UserOrganisationStructureMustHaveCapabilitySpecs
{
    public class when_user_does_not_have_capability : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static UserOrganisationStructureMustHaveCapabilityRule rule;
        private static ISystemMessageCollection messages;
        private static int userOrganisationStructureKey;
        private static int capabilityKey;
        private static UserHasCapabilityRuleModel model;
        private static List<CapabilityDataModel> capabilities;

        private Establish that = () =>
        {
            userOrganisationStructureKey = 1;
            capabilityKey = 10;
            capabilities = new List<CapabilityDataModel>
            {
                new CapabilityDataModel(2, "Capability2"),
                new CapabilityDataModel(3, "Capability3"),
            };

            dataManager = An<IWorkflowCaseDataManager>();
            dataManager
                .WhenToldTo(a => a.GetCapabilitiesForUserOrganisationStructureKey(userOrganisationStructureKey))
                .Return(capabilities);

            rule = new UserOrganisationStructureMustHaveCapabilityRule(dataManager);
            messages = SystemMessageCollection.Empty();
            model = new UserHasCapabilityRuleModel(userOrganisationStructureKey, capabilityKey);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_have_an_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_error_message = () =>
        {
            messages
                .ErrorMessages()
                .Any(a => a.Message.Equals("The organisation structure in which the user belongs no longer has specified capability")
                    && a.Severity == SystemMessageSeverityEnum.Error)
                .ShouldBeTrue();
        };
    }
}