using Machine.Specifications;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Life.Specs.Roles
{
    internal class when_getting_dynamic_role_default : WorkflowSpecLife
    {
        private static string result;
        private static string roleName;

        private Establish context = () =>
        {
            roleName = "test";
            ((ParamsDataStub)paramsData).WorkflowName = "test";
        };

        private Because of = () =>
        {
            result = workflow.GetDynamicRoleInternal(instanceData, workflowData, roleName, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldEqual(string.Empty);
        };
    }
}