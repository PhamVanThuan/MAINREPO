using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Contact_Client.OnEnter
{
    [Subject("State => Contact_Client => OnEnter")]
    internal class when_contact_client : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Contact_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_policy_type_key_data_property_to_standard_cover = () =>
        {
            workflowData.PolicyTypeKey.ShouldEqual((int)SAHL.Common.Globals.LifePolicyTypes.StandardCover);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}