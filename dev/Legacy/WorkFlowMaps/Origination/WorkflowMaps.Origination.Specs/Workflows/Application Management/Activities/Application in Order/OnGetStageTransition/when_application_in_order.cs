using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Application_in_Order.OnGetStageTransition
{
    [Subject("Activity => Application_in_Order => OnGetStageTransition")]
    internal class when_application_in_order : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Application_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_application_in_order = () =>
        {
            result.ShouldMatch("Application in Order");
        };
    }
}