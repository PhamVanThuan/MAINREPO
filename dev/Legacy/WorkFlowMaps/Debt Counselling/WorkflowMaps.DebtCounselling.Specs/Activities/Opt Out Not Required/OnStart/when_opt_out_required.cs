using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out_Not_Required.OnStart
{
    [Subject("Activity => Opt_Out_Not_Required => OnStart")]
    internal class when_opt_out_required : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static ICommon commonClient;
        private static bool result;

        private Establish context = () =>
            {
                result = true;
                client = An<IDebtCounselling>();
                commonClient = An<ICommon>();
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
                client.WhenToldTo(x => x.DebtCounsellingOptOutRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnStartActivity_Opt_Out_Not_Required(instanceData, workflowData, paramsData, messages);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };

        private It should_not_convert_the_loan_into_debt_counselling = () =>
            {
                client.WasNotToldTo(x => x.ConvertDebtCounselling(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                    Param.IsAny<string>()));
            };

        private It should_not_check_if_a_non_performing_loan_requires_opt_out = () =>
        {
            commonClient.WasNotToldTo(x => x.OptOutNonPerformingLoan(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };
    }
}