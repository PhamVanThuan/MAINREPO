using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.States.Termination_Archive.OnEnter
{
    [Subject("State => Termination_Archive => OnEnter")]
    internal class when_terminate_debt_counselling_passes_with_an_eworks_case : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static ICommon commonClient;
        private static IDebtCounselling debtcounsellingClient;

        private static string stageName;
        private static string folderID;
        private static string adUserName;

        private Establish context = () =>
        {
            result = false;
            commonClient = An<ICommon>();
            debtcounsellingClient = An<IDebtCounselling>();

            debtcounsellingClient.WhenToldTo(x => x.TerminateDebtCounselling((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, workflowData.AssignADUserName)).Return(true);
            debtcounsellingClient.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out stageName, out folderID, out adUserName)).OutRef("test", "test", "test");
            commonClient.WhenToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, "test", SAHL.Common.Constants.EworkActionNames.TerminateDebtCounselling, workflowData.DebtCounsellingKey, "test", "test")).Return(true);

            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(debtcounsellingClient);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Termination_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_terminate_debt_counselling = () =>
        {
            debtcounsellingClient.WasToldTo(x => x.TerminateDebtCounselling((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, workflowData.AssignADUserName));
        };

        private It should_get_ework_data = () =>
        {
            debtcounsellingClient.WasToldTo(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out stageName, out folderID, out adUserName));
        };

        private It should_perform_ework_action_to_return_to_x2 = () =>
        {
            commonClient.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, "test", SAHL.Common.Constants.EworkActionNames.TerminateDebtCounselling, workflowData.DebtCounsellingKey, "test", "test"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}