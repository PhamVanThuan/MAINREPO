using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Further_Lending_Calc.OnComplete
{
    [Subject("Activity => Further_Lending_Calc => OnComplete")] // AutoGenerated
    internal class when_further_lending_calc : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Further_Lending_Calc(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}