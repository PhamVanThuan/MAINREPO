using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.LoanAdjustments;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Agree_With_Decision.OnComplete
{
    [Subject("Activity => Agree_With_Decision => OnComplete")]
    internal class when_cannot_approve : WorkflowSpecLoanAdjustments
    {
        private static bool result;
        private static string message;
        private static ILoanAdjustments client;

        private Establish context = () =>
        {
            client = An<ILoanAdjustments>();
            result = true;
            message = string.Empty;
            client.WhenToldTo(x => x.CheckIfCanApproveTermChangeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<long>(), Param.IsAny<bool>())).Return(false);
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