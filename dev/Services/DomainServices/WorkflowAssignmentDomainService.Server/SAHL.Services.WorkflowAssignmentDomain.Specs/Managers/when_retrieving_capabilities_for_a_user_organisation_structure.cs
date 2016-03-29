using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_retrieving_capabilities_for_a_user_organisation_structure : WithFakes
    {
        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            dataManager = new WorkflowCaseDataManager(dbFactory);

            userOrganisationStructureKey = 1;
        };

        private Because of = () =>
        {
            result = dataManager.GetCapabilitiesForUserOrganisationStructureKey(userOrganisationStructureKey);
        };

        private It should_have_executed_the_statement = () =>
        {
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(a => a.Select(Param<GetCapabilitiesForUserOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == userOrganisationStructureKey)))
                .OnlyOnce();
        };

        private static IDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
        private static int userOrganisationStructureKey;
        private static IEnumerable<CapabilityDataModel> result;
    }
}