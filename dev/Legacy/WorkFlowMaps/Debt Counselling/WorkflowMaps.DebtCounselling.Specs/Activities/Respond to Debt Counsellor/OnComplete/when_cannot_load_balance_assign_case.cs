using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Respond_to_Debt_Counsellor.OnComplete
{
    [Subject("Activity => Respond_to_Debt_Counsellor => OnComplete")]
    internal class when_cannot_load_balance_assign_case : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IWorkflowAssignment wfa;
        private static IDebtCounselling client;
        private static string message;
        private static int callCount;

        private Establish context = () =>
            {
                string eStageName;
                string eFolderID;
                string aduserName;

                result = true;
                client = An<IDebtCounselling>();
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                    Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
                client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName,
                out eFolderID, out aduserName)).OutRef("Test", "Test", "Test")
                .WhenCalled((y) => { callCount++; });
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Respond_to_Debt_Counsellor(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };

        private It should_not_get_the_ework_data = () =>
            {
                callCount.ShouldEqual(0);
            };
    }
}