using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Sys_Sig_Check.OnStart
{
    [Subject("Activity => Sys_Sig_Check => OnStart")]
    internal class when_sys_sig_check_and_review_is_required : WorkflowSpecCredit
    {
        private static bool result;
        private static ICredit client;
        private static IWorkflowAssignment wfa;
        private static List<string> roles = new List<string> { "Credit Underwriter D" };

        private Establish context = () =>
        {
            result = false;
            workflowData.StopRecursing = false;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 1;
            workflowData.ActionSource = "Approve with Pricing Changes";

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
            client.WhenToldTo(x => x.IsReviewRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<string>())).Return(true);

            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Sys_Sig_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_whether_a_review_is_required = () =>
        {
            client.WasToldTo(x => x.IsReviewRequired((IDomainMessageCollection)messages, instanceData.ID, workflowData.ActionSource));
        };

        private It should_deactivate_the_credit_underwriter_roles = () =>
        {
            wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_round_robin_to_the_credit_supervisor_role = () =>
        {
            wfa.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Credit Supervisor D", workflowData.ApplicationKey, 1008,
                instanceData.ID, "Review", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.CreditSupervisor));
        };

        private It should_set_the_stop_recursing_data_property = () =>
        {
            workflowData.StopRecursing.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}