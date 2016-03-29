using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Credit_Approval.OnStart
{
    [Subject("Activity => Credit_Approval => OnStart")]
    internal class when_credit_approval : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Credit_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}