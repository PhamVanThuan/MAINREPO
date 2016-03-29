using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Create_Cap2_Lead.OnComplete
{
    [Subject("Activity => Create_Cap2_Lead => OnComplete")]
    public class When_the_legal_entity_name_is_longer_than_128_chars : WorkflowSpecCap2
    {
        private static int accountKey;
        private static string activitymessage;
        private static bool result;

        private Establish context = () =>
            {
                result = false;
                // mock the workflow data
                workflowData.LegalEntityName = "abcdebfghijklmnopqrstuvwxyzabcdebfghijklmnopqrstuvwxyzabcdebfghijklmnopqrstuvwxyzabcdebfghijklmnopqrstuvwxyzabcdebfghijklmnopqrstuvwxyz";
                // setup an account key
                workflowData.AccountKey = 12345;
                activitymessage = string.Empty;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Create_CAP2_lead(instanceData, workflowData, paramsData, messages, ref activitymessage);
            };

        private It should_set_instancedata_subject_to_the_legal_name_truncated_to_128_characters = () =>
            {
                instanceData.Subject.Length.ShouldEqual<int>(128);
            };

        private It should_add_ellipses_to_the_truncated_legal_name = () =>
            {
                instanceData.Subject.Substring(instanceData.Subject.Length - 3, 3).ShouldEqual<string>("...");
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