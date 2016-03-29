using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.States.Archive_Litigation.OnEnter
{
    [Subject("State => Archive_Litigation => OnEnter")]
    internal class when_archive_litigation : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Litigation(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_debt_counselling_status_to_closed = () =>
            {
                client.WasToldTo(x => x.UpdateDebtCounsellingStatus((IDomainMessageCollection)(IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, (int)SAHL.Common.Globals.DebtCounsellingStatuses.Closed));
            };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}