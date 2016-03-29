using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Archive.OnEnter
{
    [Subject("State => Archive => OnEnter")]
    internal class when_archive : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", };
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactive_users_for_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}