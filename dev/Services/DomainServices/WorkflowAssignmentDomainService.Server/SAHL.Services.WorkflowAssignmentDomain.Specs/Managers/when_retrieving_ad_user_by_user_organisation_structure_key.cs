using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Managers
{
    public class when_retrieving_ad_user_by_user_organisation_structure : WithFakes
    {
        private static IDbFactory dbFactory;
        private static WorkflowCaseDataManager dataManager;
        private static int userOrganisationStructureKey;
        private static ADUserDataModel result;
        private static IEnumerable<ADUserDataModel> expectedADUsers;

        private Establish that = () =>
        {
            dbFactory = An<IDbFactory>();
            dataManager = new WorkflowCaseDataManager(dbFactory);

            expectedADUsers = new ADUserDataModel[] { new ADUserDataModel("Some user", 1, "password", null, null, 1001) };
            userOrganisationStructureKey = 1;
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WhenToldTo(a => a.Select(Param<GetADUserByOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == userOrganisationStructureKey))).Return(expectedADUsers);
        };

        private Because of = () =>
        {
            result = dataManager.GetADUserByUserOrganisationStructureKey(userOrganisationStructureKey);
        };

        private It should_have_executed_the_statement = () =>
        {
            dbFactory
                .NewDb()
                .InReadOnlyAppContext()
                .WasToldTo(a => a.Select(Param<GetADUserByOrganisationStructureKeyStatement>
                    .Matches(b => b.UserOrganisationStructureKey == userOrganisationStructureKey)))
                .OnlyOnce();
        };

        private It should_return_the_expected_user = () =>
        {
            result.Equals(expectedADUsers.First());
        };
    }
}