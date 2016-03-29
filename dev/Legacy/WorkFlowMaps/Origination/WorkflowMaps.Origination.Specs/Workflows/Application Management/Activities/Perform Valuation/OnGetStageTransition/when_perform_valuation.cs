using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Perform_Valuation.OnGetStageTransition
{
    [Subject("Activity => Perform_Valuation => OnGetStageTransition")]
    internal class when_perform_valuation : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Perform_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_perform_valuation = () =>
        {
            result.ShouldEqual("Perform Valuation");
        };
    }
}