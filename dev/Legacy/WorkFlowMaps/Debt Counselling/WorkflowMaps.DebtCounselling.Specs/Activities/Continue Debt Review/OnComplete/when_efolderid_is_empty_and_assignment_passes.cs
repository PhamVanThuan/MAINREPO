using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Continue_Debt_Review.OnComplete
{
    [Subject("Activity => Continue_Debt_Review => OnComplete")]
    internal class when_efolderid_is_empty_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static string message;
        private static bool result;
        private static string eFolderID;
        private static string eStageName;
        private static string aduserName;
        private static IDebtCounselling client;
        private static ICommon commonClient;
        private static IWorkflowAssignment wfa;
        private static List<string> statesToInclude = new List<string> { "Review Notification" };

        private Establish context = () =>
        {
            result = false;
            workflowData.CourtCase = false;
            client = An<IDebtCounselling>();
            commonClient = An<ICommon>();
            wfa = An<IWorkflowAssignment>();
            client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName,
            out eFolderID, out aduserName)).OutRef(string.Empty, string.Empty, string.Empty);
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(), Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Continue_Debt_Review(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_not_perform_the_ework_action = () =>
            {
                commonClient.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>(),
                    Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()));
            };

        private It should_load_balance_assign_to_debt_counselling_administrator = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingAdminD, "Review Notification", statesToInclude, true, workflowData.CourtCase));
            };
    }
}