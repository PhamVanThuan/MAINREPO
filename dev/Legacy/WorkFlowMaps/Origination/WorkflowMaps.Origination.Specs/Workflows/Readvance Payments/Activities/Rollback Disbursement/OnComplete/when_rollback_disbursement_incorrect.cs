using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Rollback_Disbursement.OnComplete
{
    [Subject("State => Rollback_Disbursement => OnComplete")]
    internal class when_rollback_disbursement_incorrect : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL fl;
        private static ICommon common;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            fl = An<IFL>();
            common = An<ICommon>();
            fl.WhenToldTo(x => x.CanRollbackReadvanceCorrectionTransaction((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
            common.WhenToldTo(x => x.GetApplicationType((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(2);
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Rollback_Disbursement(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_remove_detail_from_account = () =>
        {
            common.WasNotToldTo(x => x.RemoveDetailFromAccount((IDomainMessageCollection)messages, workflowData.ApplicationKey, SAHL.Common.Constants.DetailTypes.NewLegalAgreementSigned));
        };

        private It should_be_able_to_rollback_transactions = () =>
        {
            fl.WasToldTo(x => x.CanRollbackReadvanceCorrectionTransaction((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

		private It should_return_true = () =>
		{
			result.ShouldBeFalse();
		};
    }
}