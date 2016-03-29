using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Disputes.OnEnter
{
    [Subject("State => Disputes => OnEnter")]
    internal class when_disputes_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment client;
        private static List<string> workflowRoles;

        private Establish context = () =>
        {
            result = false;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            workflowRoles = new List<string> { "Branch Consultant D", "Branch Admin D", "Branch Manager D", "New Business Processor D", "New Business Manager D" };
            workflowData.IsFL = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Disputes(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_origination_users = () =>
        {
            client.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_branch_users_for_origination = () =>
        {
            client.WasToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages, instanceData.ID, workflowData.AppCapIID, workflowData.ApplicationKey, "Disputes",
                                                                        SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_reactivate_user_or_round_robin = () =>
        {
            client.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Disputes",
                                                                                 SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };
    }
}