using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Perform_Further_Valuation.OnGetStageTransition
{
    [Subject("Activity => Perform_Further_Valuation => OnGetStageTransition")]
    internal class when_perform_further_valuation : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Perform_Further_Valuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_perform_further_valuation = () =>
        {
            result.ShouldEqual("Perform Further Valuation");
        };
    }
}