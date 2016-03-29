using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Exceptions_Decline_with_Offer.OnComplete
{
    [Subject("Activity => Exceptions_Decline_With_Offer => OnComplete")]
    internal class when_exceptions_decline_with_offer_and_rules_pass : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit client;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            workflowData.ExceptionsDeclineWithOffer = false;

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
            client.WhenToldTo(x => x.CheckCreditApprovalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Exceptions_Decline_with_Offer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_esceptions_decline_with_offer_data_property = () =>
        {
            workflowData.ExceptionsDeclineWithOffer.ShouldBeTrue();
        };

        private It should_check_the_credit_approval_rules = () =>
        {
            client.WasToldTo(x => x.CheckCreditApprovalRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_set_action_source_data_property = () =>
        {
            workflowData.ActionSource.ShouldBeTheSameAs("Exceptions Decline with Offer");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}