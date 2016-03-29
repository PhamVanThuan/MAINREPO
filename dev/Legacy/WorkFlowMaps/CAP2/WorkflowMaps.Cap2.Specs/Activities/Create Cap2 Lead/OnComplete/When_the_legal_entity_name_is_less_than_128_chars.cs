using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Create_Cap2_Lead.OnComplete
{
    [Subject("Activity => Create_Cap2_Lead => OnComplete")]
    internal class When_the_legal_entity_name_is_less_than_128_chars : WorkflowSpecCap2
    {
        private static bool result;
        private static string activitymessage;

        private Establish context = () =>
        {
            workflowData.AccountKey = 1234567;
            workflowData.LegalEntityName = "Mr Clinton Speed";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_CAP2_lead(instanceData, workflowData, paramsData, messages, ref activitymessage);
        };

        private It should_set_instancedata_subject = () =>
        {
            instanceData.Subject.ShouldBeTheSameAs(workflowData.LegalEntityName);
        };

        private It should_add_not_add_ellipses_to_the_truncated_legal_name = () =>
        {
            instanceData.Subject.Substring(instanceData.Subject.Length - 3, 3).ShouldNotEqual<string>("...");
        };

        private It should_set_instancedata_name_to_the_account_key = () =>
        {
            instanceData.Name.ShouldEqual<string>(workflowData.AccountKey.ToString());
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}