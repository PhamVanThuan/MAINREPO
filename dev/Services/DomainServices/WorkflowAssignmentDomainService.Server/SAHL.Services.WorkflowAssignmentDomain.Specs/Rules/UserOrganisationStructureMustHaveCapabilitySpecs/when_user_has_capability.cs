using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Rules.UserOrganisationStructureMustHaveCapabilitySpecs
{
    public class when_user_has_capability : WithFakes
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
            capabilityKey = 2;
            capabilities = new List<CapabilityDataModel>
            {
                new CapabilityDataModel(2, "Capability2"),
                new CapabilityDataModel(3, "Capability3"),
            };

            dataManager = An<IWorkflowCaseDataManager>();
            dataManager.WhenToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrganisationStructureKey))
                .Return(capabilities);
            rule = new UserOrganisationStructureMustHaveCapabilityRule(dataManager);
            messages = SystemMessageCollection.Empty();
            model = new UserHasCapabilityRuleModel(userOrganisationStructureKey, capabilityKey);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_have_no_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}