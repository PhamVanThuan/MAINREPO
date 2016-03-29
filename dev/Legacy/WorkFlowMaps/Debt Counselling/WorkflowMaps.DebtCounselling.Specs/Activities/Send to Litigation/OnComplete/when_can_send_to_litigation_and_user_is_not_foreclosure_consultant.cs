using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_to_Litigation.OnComplete
{
    internal class when_can_send_to_litigation_and_user_is_not_foreclosure_consultant : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static int callCount;

        private Establish context = () =>
        {
            string eStageName;
            string eFolderID;
            string aduserName;
            callCount = 0;
            workflowData.SentToLitigation = true;
            result = true;
            client = An<IDebtCounselling>();
            wfa = An<IWorkflowAssignment>();
            client.WhenToldTo(x => x.CheckSendToLitigationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
            wfa.WhenToldTo(x => x.CheckUserInWorkflowRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(),
                Param.IsAny<int>())).Return(false);
            client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName,
                out eFolderID, out aduserName)).OutRef("Test", "Test", "Test")
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Send_to_Litigation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_get_the_ework_data_for_the_loss_control_case = () =>
            {
                callCount.ShouldEqual(1);
            };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}