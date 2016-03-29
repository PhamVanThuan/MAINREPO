using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Projections.WorkflowAssignment.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.WorkflowCaseSpecs.Statements
{
    public class when_creating_an_GetUserNameForUserOrganisationStructureKey_statement : WithFakes
    {
        private Establish that = () =>
        {
            userOrganisationStructureKey = 3;
        };

        private Because of = () =>
        {
            statement = new GetUserNameForUserOrganisationStructureKeyStatement(userOrganisationStructureKey);
        };

        private It should_have_set_the_expected_fields = () =>
        {
            statement.UserOrganisationStructureKey.ShouldEqual(userOrganisationStructureKey);
        };

        private static int userOrganisationStructureKey;
        private static GetUserNameForUserOrganisationStructureKeyStatement statement;
    }
}
