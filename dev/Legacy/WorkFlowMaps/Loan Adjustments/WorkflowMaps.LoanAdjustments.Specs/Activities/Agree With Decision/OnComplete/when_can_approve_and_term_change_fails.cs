using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.LoanAdjustments;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Agree_With_Decision.OnComplete
{
    [Subject("Activity => Agree_With_Decision => OnComplete")]
    internal class when_can_approve_and_term_change_fails : WorkflowSpecLoanAdjustments
    {
        private static ILoanAdjustments client;
        private static string message;
        private static bool result = true;

        private Establish context = () =>
        {
            message = string.Empty;
            client = An<ILoanAdjustments>();
            client.WhenToldTo(x => x.CheckIfCanApproveTermChangeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>(),
                Param.IsAny<bool>())).Return(true);
            client.WhenToldTo(x => x.ApproveTermChangeRequest(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ILoanAdjustments>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Agree_With_Decision(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}