using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_user_has_capability : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static bool result;
        private static int capabilityKey, userOrgStructureKey;
        private static WorkflowCaseDataManager dataManager;

        private Establish that = () =>
        {
            userOrgStructureKey = 11001;
            capabilityKey = 1001;
            dbFactory = An<FakeDbFactory>();

            dataManager = new WorkflowCaseDataManager(dbFactory);
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WhenToldTo(a => a.Select(Param<GetCapabilitiesForUserOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == userOrgStructureKey)))
                    .Return(new List<CapabilityDataModel>() { new CapabilityDataModel(capabilityKey, "some capability") });
        };

        private Because of = () =>
        {
            result = dataManager.DoesUserOrganisationStructureKeyHaveCapability(capabilityKey, userOrgStructureKey);
        };

        private It should_get_capabilities_assigned_to_user = () =>
        {
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(a => a.Select(Param<GetCapabilitiesForUserOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == userOrgStructureKey)))
                .OnlyOnce();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}