using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.EXT_Reinstate.OnComplete
{
    [Subject("Activity => EXT_Reinstate => OnComplete")]
    internal class when_ext_reinstate : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            workflowData.PreviousState = "PrevousStateTest";
            workflowData.IsFL = true;
            workflowData.AppCapIID = 3;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Reinstate(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_handle_app_man_roles_on_return_from_ntu_to_prev_state = () =>
        {
            assignment.WasToldTo(x => x.HandleApplicationManagementRolesOnReturnFromNTUtoPreviousState((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                workflowData.PreviousState,
                workflowData.IsFL,
                workflowData.AppCapIID,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}