using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.LoanAdjustments;

namespace WorkflowMaps.LoanAdjustments.Specs.Activities.Agree_With_Decision.OnComplete
{
    [Subject("Activity => Agree_With_Decision => OnComplete")]
    internal class when_can_approve_and_term_change_successful : WorkflowSpecLoanAdjustments
    {
        private static ILoanAdjustments client;
        private static string message;
        private static bool result = true;

        private Establish context = () =>
        {
            ((InstanceDataStub)instanceData).ActivityADUserName = @"SAHL\ClintonS";
            workflowData.ProcessUser = @"SAHL\HaloUser";
            workflowData.RequestApproved = false;
            message = string.Empty;
            client = An<ILoanAdjustments>();
            client.WhenToldTo(x => x.CheckIfCanApproveTermChangeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>(),
                Param.IsAny<bool>())).Return(true);
            client.WhenToldTo(x => x.ApproveTermChangeRequest(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<ILoanAdjustments>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Agree_With_Decision(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_process_user_data_property_to_activity_aduser = () =>
            {
                workflowData.ProcessUser.ShouldBeTheSameAs(instanceData.ActivityADUserName);
            };

        private It should_set_request_approved_data_property_to_true = () =>
            {
                workflowData.RequestApproved.ShouldBeTrue();
            };
    }
}