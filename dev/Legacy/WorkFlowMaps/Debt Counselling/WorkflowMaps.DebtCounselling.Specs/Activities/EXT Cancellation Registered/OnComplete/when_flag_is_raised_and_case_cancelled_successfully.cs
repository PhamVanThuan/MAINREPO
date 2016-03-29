using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.EXT_Cancellation_Registered.OnComplete
{
    [Subject("Activity => EXT_Cancellation_Registered => OnComplete")]
    internal class when_flag_is_raised_and_case_cancelled_successfully : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                result = false;
                client = An<IDebtCounselling>();
                client.WhenToldTo(x => x.CancelDebtCounselling_EXT(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>(),
                    Param.IsAny<string>())).Return(true);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_EXT_Cancellation_Registered(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_cancel_the_debt_counselling_case = () =>
            {
                client.WasToldTo(x => x.CancelDebtCounselling_EXT((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, instanceData.ID,
                    "Capture Recoveries Proposal"));
            };
    }
}