using Machine.Specifications;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Create_Instance.OnComplete
{
    [Subject("Activity => Create_Instance => OnComplete")]
    internal class when_creating_an_instance : WorkflowSpecLoanAdjustments
    {
        private static string message;
        private static int accountKey;
        private static bool result;

        private Establish context = () =>
            {
                result = false;
                message = string.Empty;
                accountKey = 1234;
                workflowData.RequestApproved = true;
                workflowData.RequestUser = @"SAHL\ClintonS";
                workflowData.AccountKey = accountKey;
                workflowData.LoanAdjustmentType = 2;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Create_Instance(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_set_request_approved_data_property_to_false = () =>
            {
                workflowData.RequestApproved.ShouldBeFalse();
            };

        private It should_set_name_instance_data_property = () =>
            {
                instanceData.Name.ShouldMatch(string.Format(@"Loan Adjustment: {0}", workflowData.AccountKey));
            };

        private It should_set_subject_instance_data_property = () =>
            {
                instanceData.Subject.ShouldMatch(string.Format("Loan Adjustment: {0} Change type {1}", workflowData.AccountKey, workflowData.LoanAdjustmentType));
            };

        private It should_set_activity_message = () =>
            {
                message = string.Format(@"Request Created by {0}", workflowData.RequestUser);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}