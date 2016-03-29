using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Respond_to_Debt_Counsellor.OnComplete
{
    [Subject("Activity => Respond_to_Debt_Counsellor => OnComplete")]
    internal class when_can_assign_and_efolderID_is_empty : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static IDebtCounselling client;
        private static ICommon commonClient;
        private static string message;
        private static bool result;
        private static string eFolderID;
        private static string eStageName;
        private static string aduserName;
        private static int callCount;
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
            {
                workflowData.AccountKey = 1;
                ((InstanceDataStub)instanceData).ID = 1;
                workflowData.DebtCounsellingKey = 1;
                workflowData.CourtCase = false;
                result = false;
                wfa = An<IWorkflowAssignment>();
                client = An<IDebtCounselling>();
                commonClient = An<ICommon>();
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                    Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
                client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName, out eFolderID, out aduserName)).OutRef(string.Empty, string.Empty, string.Empty)
                    .WhenCalled((y) => { callCount++; });
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Respond_to_Debt_Counsellor(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_load_balance_assign_to_debt_counselling_consultant_using_exclusion_states = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Pend Proposal", exclusionStates, false, workflowData.CourtCase));
            };

        private It should_get_the_ework_loss_control_data = () =>
            {
                callCount.ShouldBeGreaterThan(0);
            };

        private It should_not_perform_the_ework_action = () =>
            {
                commonClient.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(),
                    Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()));
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}