using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disburse_Funds.OnStart
{
    [Subject("Activity => Disburse_Funds => OnStart")]
    internal class when_disbursing_funds_before_cut_off_time_with_ceded_external_life : WorkflowSpecPersonalLoans
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
                client.WhenToldTo(x => x.CheckDisbursementCutOffTimeRule((IDomainMessageCollection)messages, paramsData.IgnoreWarning)).Return(true);
                client.WhenToldTo(x => x.CheckExternalPolicyIsCededRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnStartActivity_Disburse_Funds(instanceData, workflowData, paramsData, messages);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_check_disbursement_cut_off_time = () =>
        {
            client.WasToldTo(x => x.CheckDisbursementCutOffTimeRule((IDomainMessageCollection)messages, paramsData.IgnoreWarning));
        };

        private It should_check_external_policy_is_ceded = () =>
        {
            client.WasToldTo(x => x.CheckExternalPolicyIsCededRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };
    }
}