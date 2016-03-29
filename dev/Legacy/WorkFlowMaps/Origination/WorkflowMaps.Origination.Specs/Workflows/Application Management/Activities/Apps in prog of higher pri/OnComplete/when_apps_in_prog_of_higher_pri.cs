using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Apps_in_prog_of_higher_pri.OnComplete
{
    [Subject("Activity => Apps_in_prog_of_higher_pri => OnStart")]
    internal class when_apps_in_prog_of_higher_pri : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Apps_in_prog_of_higher_pri(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_previous_state_data_property_to_qa = () =>
        {
            workflowData.PreviousState.ShouldMatch("QA");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}