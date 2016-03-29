using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Refer_Senior_Analyst.OnStart
{
    [Subject("Activity => Refer_Senior_Analyst => OnStart")] // AutoGenerated
    internal class when_refer_senior_analyst : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Refer_Senior_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}