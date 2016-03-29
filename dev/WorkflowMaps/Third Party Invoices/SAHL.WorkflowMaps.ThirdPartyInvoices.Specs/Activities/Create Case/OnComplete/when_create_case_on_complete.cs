using Machine.Specifications;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Create_Case
{
    internal class when_creating_a_case_on_complete : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Case(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_subject = () =>
        {
            instanceData.Subject.ShouldEqual(string.Format("Received from: {0}", (string)paramsData.Data));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}