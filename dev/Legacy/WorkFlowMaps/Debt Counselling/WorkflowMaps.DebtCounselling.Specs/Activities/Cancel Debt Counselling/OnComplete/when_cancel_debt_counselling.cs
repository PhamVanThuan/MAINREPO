using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Cancel_Debt_Counselling.OnComplete
{
    [Subject("Activity => Cancel_Debt_Counselling => OnComplete")]
    internal class when_cancel_debt_counselling : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IDebtCounselling client;
        private static string eFolderID;
        private static string eStageName;
        private static string aduserName;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.LatestReasonDefinitionKey = 1;
            workflowData.DebtCounsellingKey = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.IsRCSAccount(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);

            client.Expect(x => x.GetEworkDataForLossControlCase((IDomainMessageCollection)messages, workflowData.AccountKey, out eStageName,
           out eFolderID, out aduserName)).OutRef("Test", "Test", "Test");
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Cancel_Debt_Counselling(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_cancel_the_debt_counselling_case = () =>
        {
            client.WasToldTo(x => x.CancelDebtCounselling((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, workflowData.LatestReasonDefinitionKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}