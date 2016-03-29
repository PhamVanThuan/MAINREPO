using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Escalate_to_Exceptions_Manager.OnComplete
{
    [Subject("Activity => Escalate_to_Exceptions_Manager => OnComplete")]
    internal class when_escalate_to_exceptions_manager : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 0;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);

            ((ParamsDataStub)paramsData).Data = "testData";
            ((InstanceDataStub)instanceData).Name = "testInstanceName";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Escalate_to_Exceptions_Manager(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_assign_workflow_role_for_aduser = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignWorkflowRoleForADUser(
                (IDomainMessageCollection)messages,
                instanceData.ID,
                paramsData.Data.ToString(),
                SAHL.Common.Globals.WorkflowRoleTypes.PLCreditExceptionsManagerD,
                workflowData.ApplicationKey,
                instanceData.Name))
            .OnlyOnce();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}