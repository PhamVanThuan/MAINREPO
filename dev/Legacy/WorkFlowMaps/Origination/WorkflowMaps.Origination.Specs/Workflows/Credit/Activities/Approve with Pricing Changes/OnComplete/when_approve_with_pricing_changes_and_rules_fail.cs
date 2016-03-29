using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Approve_with_Pricing_Changes.OnComplete
{
    [Subject("Activity => Approve_With_Pricing_Changes => OnComplete")]
    internal class when_approve_with_pricing_changes_and_rules_fail : WorkflowSpecCredit
    {
        private static bool result;
        private static string message;
        private static ICredit client;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
            client.WhenToldTo(x => x.CheckCreditApprovalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Approve_with_Pricing_Changes(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_the_credit_approval_rules = () =>
        {
            client.WasToldTo(x => x.CheckCreditApprovalRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}