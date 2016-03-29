using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out_Required.OnComplete
{
    [Subject("Activity => Opt_Out_Required => OnComplete")]
    internal class when_opt_out_for_debt_counselling_is_unsuccessful : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static string message;

        private Establish context = () =>
        {
            result = true;
            client = An<IDebtCounselling>();
            client.WhenToldTo(x => x.ProcessDebtCounsellingOptOut(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<string>())).Return(false);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Opt_Out_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}