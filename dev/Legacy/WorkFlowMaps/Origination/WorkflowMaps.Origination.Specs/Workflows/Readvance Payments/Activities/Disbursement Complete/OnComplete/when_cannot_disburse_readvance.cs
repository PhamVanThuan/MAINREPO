using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Disbursement_Complete.OnComplete
{
    [Subject("Activity => Disbursement_Complete => OnComplete")]
    internal class when_cannot_disburse_readvance : WorkflowSpecReadvancePayments
    {
        private static IFL client;
        private static bool result;
        private static ICommon commonClient;
        private static string message;

        private Establish context = () =>
        {
            result = true;
            client = An<IFL>();
            commonClient = An<ICommon>();
            client.WhenToldTo(x => x.CheckCanDisburseReadvanceRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Disbursement_Complete(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_add_the_new_legal_agreement_signed_detail_type = () =>
        {
            commonClient.WasNotToldTo(x => x.AddDetailType(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

		private It should_return_false_from_the_action = () =>
		{
			result.ShouldBeFalse();
		};
    }
}