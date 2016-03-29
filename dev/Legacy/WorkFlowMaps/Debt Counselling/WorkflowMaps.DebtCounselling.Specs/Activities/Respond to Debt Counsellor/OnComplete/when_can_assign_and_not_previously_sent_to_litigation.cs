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
    internal class when_can_assign_and_not_previously_sent_to_litigation : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static IDebtCounselling client;
        private static ICommon commonClient;
        private static string message;
        private static bool result;
        private static string eFolderID;
        private static string eStageName;
        private static string aduserName;

        private Establish context = () =>
        {
            eStageName = "Stage";
            eFolderID = "eFolder";
            workflowData.AccountKey = 1;
            workflowData.DebtCounsellingKey = 1;
            workflowData.CourtCase = false;
            workflowData.SentToLitigation = false;
            ((InstanceDataStub)instanceData).ID = 1;
            result = false;
            wfa = An<IWorkflowAssignment>();
            client = An<IDebtCounselling>();
            commonClient = An<ICommon>();
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            commonClient.WhenToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(),
                Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(true);
            client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName,
                out eFolderID, out aduserName)).OutRef("Stage", "eFolder", "adUser");
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
        };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Respond_to_Debt_Counsellor(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_perform_the_x2_debt_counselling_ework_action = () =>
            {
                commonClient.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, eFolderID, SAHL.Common.Constants.EworkActionNames.X2DebtCounselling, workflowData.DebtCounsellingKey,
                    string.Empty, eStageName));
            };
    }
}