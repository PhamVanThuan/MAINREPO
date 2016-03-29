using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disburse_Funds.OnStart
{
    [Subject("Activity => Disburse_Funds => OnStart")]
    internal class when_disbursing_funds_before_cut_off_time_with_no_life_policy : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static ICommon commonClient;

        private Establish context = () =>
            {
                workflowData.ApplicationKey = 1;
                result = false;
                client = An<IPersonalLoan>();
                wfa = An<IWorkflowAssignment>();
                commonClient = An<ICommon>();
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);

                client.WhenToldTo(x => x.CheckExternalPolicyIsCededRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning)).Return(false);
            };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Disburse_Funds(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_check_external_policy_details_fully_captured_and_ceded = () =>
        {
            client.WasToldTo(x => x.CheckExternalPolicyIsCededRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_not_bother_checking_disbursement_cut_off_time = () =>
        {
            client.WasNotToldTo(x => x.CheckDisbursementCutOffTimeRule((IDomainMessageCollection)messages, paramsData.IgnoreWarning));
        };
    }
}