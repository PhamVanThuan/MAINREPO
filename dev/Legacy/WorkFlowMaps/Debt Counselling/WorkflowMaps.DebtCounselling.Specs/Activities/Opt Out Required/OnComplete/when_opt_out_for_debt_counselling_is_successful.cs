using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out_Required.OnComplete
{
    [Subject("Activity => Opt_Out_Required => OnComplete")]
    internal class when_opt_out_for_debt_counselling_is_successful : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static string message;

        private Establish context = () =>
            {
                ((ParamsDataStub)paramsData).ADUserName = "test";
                workflowData.AccountKey = 1;
                result = false;
                client = An<IDebtCounselling>();
                client.WhenToldTo(x => x.ProcessDebtCounsellingOptOut(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                    Param.IsAny<string>())).Return(true);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Opt_Out_Required(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_process_the_debt_counselling_opt_out = () =>
            {
                client.WasToldTo(x => x.ProcessDebtCounsellingOptOut((IDomainMessageCollection)messages, workflowData.AccountKey, paramsData.ADUserName));
            };
    }
}