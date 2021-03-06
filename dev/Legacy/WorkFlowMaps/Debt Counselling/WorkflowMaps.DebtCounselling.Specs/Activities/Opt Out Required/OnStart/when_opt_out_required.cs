using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out_Required.OnStart
{
    [Subject("Activity => Opt_Out_Required => OnStart")] // AutoGenerated
    internal class when_opt_out_required : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static bool expectedresult;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            expectedresult = true;
            workflowData.AccountKey = 1;
            client = An<IDebtCounselling>();
            client.WhenToldTo(x => x.DebtCounsellingOptOutRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedresult);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Opt_Out_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_perform_debt_counselling_opt_out_required = () =>
        {
            client.WasToldTo(x => x.DebtCounsellingOptOutRequired((IDomainMessageCollection)messages, workflowData.AccountKey));
        };

        private It should_return_debt_counselling_opt_out_required_return_value = () =>
        {
            result.ShouldEqual(expectedresult);
        };
    }
}