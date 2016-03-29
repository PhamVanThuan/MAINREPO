using Machine.Specifications;
using SAHL.Core.SystemMessages;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Valuation_Required.OnStart
{
    [Subject("Activity => Review_Valuation_Required => OnStart")]
    internal class when_review_valuation_not_required : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.RequireValuation = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Review_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_add_valuation_not_required_domainservice_message = () =>
        {
            messages.AllMessages.ShouldContain(x => x.Message == "Valuation is not required" && x.Severity == SystemMessageSeverityEnum.Error);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}