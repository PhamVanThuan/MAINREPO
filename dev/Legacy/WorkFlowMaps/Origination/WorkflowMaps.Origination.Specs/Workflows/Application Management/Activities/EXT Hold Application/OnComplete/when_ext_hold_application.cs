using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.EXT_Hold_Application.OnComplete
{
    [Subject("Activity => EXT_Hold_Application => OnComplete")]
    internal class when_ext_hold_application : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Hold_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}