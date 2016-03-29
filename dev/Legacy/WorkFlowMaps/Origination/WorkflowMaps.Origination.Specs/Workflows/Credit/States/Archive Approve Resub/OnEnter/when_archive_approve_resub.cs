using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Archive_Approve_Resub.OnEnter
{
    [Subject("State => Archive_Approve_Resub => OnEnter")]
    internal class when_archive_approve_resub : WorkflowSpecCredit
    {
        private static bool result;
        private static List<string> roles = new List<string> { "Credit Underwriter D", "Credit Supervisor D", "Credit Manager D", "Credit Exceptions D" };
        private static IWorkflowAssignment wfa;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;

            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Approve_Resub(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_the_users_for_this_instance = () =>
        {
            wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}