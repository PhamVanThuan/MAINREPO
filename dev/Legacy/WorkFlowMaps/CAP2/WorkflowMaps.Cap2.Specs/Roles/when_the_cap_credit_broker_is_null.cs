using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Roles
{
    internal class when_the_cap_credit_broker_is_null : WorkflowSpecCap2
    {
        private static string username;

        private Establish context = () =>
        {
            workflowData.CapCreditBroker = null;
        };

        private Because of = () =>
        {
            username = workflow.OnGetRole_CAP2_Offers_CapCreditBroker(instanceData, workflowData, string.Empty, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            username.ShouldEqual(string.Empty);
        };
    }
}