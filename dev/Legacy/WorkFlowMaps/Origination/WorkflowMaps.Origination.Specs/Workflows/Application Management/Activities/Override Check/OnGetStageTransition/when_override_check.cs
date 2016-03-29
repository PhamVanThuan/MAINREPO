using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Override_Check.OnGetStageTransition
{
    [Subject("Activity => Override_Check => OnGetStageTransition")]
    internal class when_override_check : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Override_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_override_check = () =>
        {
            result.ShouldEqual("Override Check");
        };
    }
}