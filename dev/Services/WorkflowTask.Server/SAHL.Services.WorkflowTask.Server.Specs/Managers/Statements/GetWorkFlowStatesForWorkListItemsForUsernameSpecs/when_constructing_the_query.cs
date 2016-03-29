using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.WorkflowTask.Server.Managers.Statements;

namespace SAHL.Services.WorkflowTask.Server.Specs.Managers.Statements.GetWorkFlowStatesForWorkListItemsForUsernameSpecs
{
    public class when_constructing_the_query : WithFakes
    {
        private static string username;
        private static GetWorkFlowStatesForWorkListItemsForUsernameStatement statement;
        private static List<string> roles;

        private Establish that = () =>
        {
            username = @"SAHL\HaloUser";
            roles = new List<string>
            {
                "@Some Arbitrary Active Directory Role",
            };
        };

        private Because of = () =>
        {
            statement = new GetWorkFlowStatesForWorkListItemsForUsernameStatement(username, roles);
        };

        private It should_have_the_username_and_roles_property_referenced_in_the_query_statement = () =>
        {
            statement.GetStatement().ShouldContain("@UsernameAndRoles");
        };

        private It should_have_three_values_in_the_username_and_roles_property = () =>
        {
            statement.UsernameAndRoles.Count().ShouldEqual(3);
        };

        private It should_have_the_username_in_the_username_and_roles_property = () =>
        {
            statement.UsernameAndRoles.ShouldContain(username);
        };

        private It should_contain_the_eveyrone_group = () =>
        {
            statement.UsernameAndRoles.ShouldContain("Everyone");
        };

        private It should_have_the_role_in_the_username_and_roles_property = () =>
        {
            statement.UsernameAndRoles
                .Where(a => a != username && a != "Everyone")
                .All(a => roles.Contains(a)) //all roles in role field should be in UsernameAndRoles
                .ShouldBeTrue();
        };
    }
}
