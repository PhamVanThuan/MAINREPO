using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline_with_Offer.OnComplete
{
    [Subject("Activity => Decline_With_Offer => OnComplete")]
    internal class when_decline_with_offer_and_rules_fail : WorkflowSpecCredit
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
            result = workflow.OnCompleteActivity_Decline_with_Offer(instanceData, workflowData, paramsData, messages, ref message);
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