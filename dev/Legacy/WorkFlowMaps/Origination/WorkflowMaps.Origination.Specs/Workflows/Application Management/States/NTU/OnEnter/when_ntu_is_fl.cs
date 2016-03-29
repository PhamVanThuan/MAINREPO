using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.NTU.OnEnter
{
    [Subject("State => NTU => OnEnter")]
    internal class when_ntu_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_reactivate_user_or_round_robin_157 = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID,
                                                                              "NTU", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_not_reactivate_branchusers_for_origination = () =>
        {
            client.WasNotToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages, instanceData.ID, workflowData.AppCapIID, workflowData.ApplicationKey,
                                                                           "NTU", SAHL.Common.Globals.Process.Origination));
        };

        private It not_should_reactivate_user_or_round_robin_106 = () =>
        {
            client.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "New Business Processor D", workflowData.ApplicationKey, 106, instanceData.ID,
                                                                                 "NTU", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}