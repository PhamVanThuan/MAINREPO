﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.States.Termination_Archive.OnEnter
{
    [Subject("State => Termination_Archive => OnEnter")]
    public class when_terminate_debt_counselling_fails : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static ICommon commonClient;
        private static IDebtCounselling debtcounsellingClient;

        private static string stageName;
        private static string folderID;
        private static string adUserName;

        private Establish context = () =>
        {
            result = true;
            commonClient = An<ICommon>();
            debtcounsellingClient = An<IDebtCounselling>();

            debtcounsellingClient.WhenToldTo(x => x.TerminateDebtCounselling((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, workflowData.AssignADUserName)).Return(false);

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

        private It should_not_get_ework_data = () =>
        {
            debtcounsellingClient.WasNotToldTo(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out stageName, out folderID, out adUserName));
        };

        private It should_not_perform_ework_action_to_terminate_case = () =>
        {
            commonClient.WasNotToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, folderID, SAHL.Common.Constants.EworkActionNames.TerminateDebtCounselling, workflowData.DebtCounsellingKey, adUserName, stageName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}