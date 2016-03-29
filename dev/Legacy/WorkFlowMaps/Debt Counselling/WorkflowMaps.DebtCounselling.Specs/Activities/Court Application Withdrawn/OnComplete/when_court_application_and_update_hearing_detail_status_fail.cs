using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Application_Withdrawn.OnStart
{
    [Subject("Activity => Court_Application_Withdrawn => OnComplete")]
    internal class when_court_application_and_update_hearing_detail_status_fail : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;
            workflowData.DebtCounsellingKey = 1;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.UpdateHearingDetailStatusToInactive(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Court_Application_Withdrawn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}