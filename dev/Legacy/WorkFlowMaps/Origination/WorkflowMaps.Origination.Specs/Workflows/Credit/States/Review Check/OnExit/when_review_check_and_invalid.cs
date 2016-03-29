using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Credit.Specs.States.Review_Check.OnExit
{
    [Subject("State => Review_Check => OnExit")] // AutoGenerated
    internal class when_review_check_and_invalid : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit creditHost;

        private Establish context = () =>
        {
            creditHost = An<ICredit>();
            workflowData.ActionSource = "Decline with Offer";
            domainServiceLoader.RegisterMockForType<ICredit>(creditHost);
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Review_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_ask_credit_host_to_send_decision_mail = () =>
        {
            creditHost.WasNotToldTo(x => x.SendCreditDecisionMail(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>()));
        };
    }
}