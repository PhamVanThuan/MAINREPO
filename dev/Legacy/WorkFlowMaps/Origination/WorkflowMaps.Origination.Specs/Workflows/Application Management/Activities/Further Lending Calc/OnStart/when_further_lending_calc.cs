using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Further_Lending_Calc.OnStart
{
    [Subject("Activity => Further_Lending_Calc => OnStart")] // AutoGenerated
    internal class when_further_lending_calc : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Lending_Calc(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}