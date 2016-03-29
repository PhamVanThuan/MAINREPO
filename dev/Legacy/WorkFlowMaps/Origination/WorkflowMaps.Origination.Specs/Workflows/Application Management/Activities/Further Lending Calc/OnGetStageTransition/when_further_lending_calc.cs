using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Further_Lending_Calc.OnGetStageTransition
{
    [Subject("Activity => Further_Lending_Calc => OnGetStageTransition")]
    internal class when_further_lending_calc : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Further_Lending_Calc(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_further_lending_calc = () =>
        {
            result.ShouldMatch("Further Lending Calc");
        };
    }
}