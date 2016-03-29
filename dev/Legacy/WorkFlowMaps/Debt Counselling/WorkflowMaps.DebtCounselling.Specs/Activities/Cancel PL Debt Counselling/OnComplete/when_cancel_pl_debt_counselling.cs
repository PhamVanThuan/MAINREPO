using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Cancel_PL_Debt_Counselling.OnComplete
{
    [Subject("Activity => Cancel_PL_Debt_Counselling => OnComplete")]
    internal class when_cancel_pl_debt_counselling : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.DebtCounsellingKey = 1;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cancel_PL_Debt_Counselling(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_cancel_the_PL_debt_counselling_case = () =>
        {
            client.WasToldTo(x => x.UpdateDebtCounsellingStatus((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, (int)SAHL.Common.Globals.DebtCounsellingStatuses.Cancelled));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}