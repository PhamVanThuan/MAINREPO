using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Accept_RPAR.OnComplete
{
    [Subject("Activity => Accept_RPAR => OnComplete")]
    internal class when_policy_is_not_to_be_replaced : WorkflowSpecLife
    {
        private static bool result;
        private static string message = string.Empty;

        private Establish context = () =>
            {
                result = false;
                workflowData.RPARInsurer = string.Empty;
                workflowData.RPARDone = 0;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Accept_RPAR(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_not_set_the_rpar_done_data_property = () =>
            {
                workflowData.RPARDone.ShouldEqual(0);
            };
    }
}