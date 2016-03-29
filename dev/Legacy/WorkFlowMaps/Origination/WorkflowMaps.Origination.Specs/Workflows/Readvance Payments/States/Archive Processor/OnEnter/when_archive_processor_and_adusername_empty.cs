using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Archive_Processor.OnEnter
{
    [Subject("State => Archive_Processor => OnEnter")]
    internal class when_archive_processor_and_adusername_empty : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> workflowRoles;
        private static string adUserName;

        private Establish context = () =>
        {
            result = false;
            ((InstanceDataStub)instanceData).SourceInstanceID = 1;
            adUserName = string.Empty;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.WhenToldTo(x => x.GetLatestUserAcrossInstances((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, 157, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination)).Return(adUserName);
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", };
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_Reassign_Case_To_User = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, (Int64)instanceData.SourceInstanceID, workflowData.ApplicationKey, adUserName, 157, 857, "Archive Processor"));
        };

        private It should_perform_x2_round_robin_for_pointer_description = () =>
        {
            workflowAssignment.WasToldTo(x => x.X2RoundRobinForPointerDescription((IDomainMessageCollection)messages, (Int64)instanceData.SourceInstanceID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, workflowData.ApplicationKey, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}